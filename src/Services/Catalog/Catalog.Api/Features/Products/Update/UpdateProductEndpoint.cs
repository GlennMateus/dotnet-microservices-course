namespace Catalog.Api.Features.Products.Create;

public record UpdateProductRequest(Guid Id
    , string Name
    , List<string> Category
    , string Description
    , string ImageFile
    , decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapPut("/{id}",
            async (Guid id, UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>() with { Id = id };

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateProductResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}
