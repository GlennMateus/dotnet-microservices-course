namespace Catalog.Api.Features.Products.GetAll;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductResult>;
public record GetProductResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session
    , ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);
        
        var product = await session
            .LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException();

        return new GetProductResult(product);
    }
}
