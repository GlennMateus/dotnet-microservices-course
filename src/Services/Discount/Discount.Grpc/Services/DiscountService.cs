using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CuponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var cupon = await dbContext.Cupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (cupon is null)
            cupon = new Cupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };

        logger.LogInformation("Discount retrieved for ProductName: {productName}, Amount: {amount}", cupon.ProductName, cupon.Amount);

        return cupon.Adapt<CuponModel>();
    }

    public override async Task<CuponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var cupon = request.Cupon.Adapt<Cupon>();
        if (cupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Cupons.Add(cupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discont is successfully created. ProductName: {productName}, Amount: {amount}", cupon.ProductName, cupon.Amount);
        return cupon.Adapt<CuponModel>();
    }

    public override async Task<CuponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var cupon = request.Cupon.Adapt<Cupon>();
        if (cupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

        dbContext.Cupons.Update(cupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discont is successfully updated. ProductName: {productName}, Amount: {amount}", cupon.ProductName, cupon.Amount);
        return cupon.Adapt<CuponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var cupon = await dbContext
           .Cupons
           .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (cupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName: {request.ProductName} not found."));

        dbContext.Cupons.Remove(cupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount for ProductName: {productName} was sucessfully deleted.", request.ProductName);

        return new DeleteDiscountResponse { Success = true };
    }
}
