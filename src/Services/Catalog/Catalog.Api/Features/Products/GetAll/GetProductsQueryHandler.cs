namespace Catalog.Api.Features.Products.GetAll;

public record GetProductsQuery(int PageNumber
    , int PageSize)
    : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session
            .Query<Product>()
            .ToPagedListAsync(pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken);

        return new GetProductsResult(products);
    }
}
