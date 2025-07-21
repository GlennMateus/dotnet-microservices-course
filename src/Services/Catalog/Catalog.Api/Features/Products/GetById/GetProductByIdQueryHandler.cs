namespace Catalog.Api.Features.Products.GetAll;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductResult>;
public record GetProductResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session
            .LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        return new GetProductResult(product);
    }
}
