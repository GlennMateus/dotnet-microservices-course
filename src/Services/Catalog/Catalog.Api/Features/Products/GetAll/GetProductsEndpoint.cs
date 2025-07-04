
namespace Catalog.Api.Features.Products.GetAll;

//public record GetProductsRequest();
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/",
        async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            var response = result.Adapt<GetProductsResponse>();
            return response;
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
