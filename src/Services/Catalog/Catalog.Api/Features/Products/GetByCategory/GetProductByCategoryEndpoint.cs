
namespace Catalog.Api.Features.Products.GetAll;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategory : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/category/{category}",
        async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            var response = result.Adapt<GetProductByCategoryResponse>();
            return response;
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}
