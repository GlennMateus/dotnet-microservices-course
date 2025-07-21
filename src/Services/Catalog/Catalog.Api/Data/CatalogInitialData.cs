using Marten.Schema;

namespace Catalog.Api.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync())
            return;

        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new[]
    {
        new Product{
            Id = new Guid("323fefd4-0c49-491c-b500-3b748ca61a63"),
            Name = "IPhone 15",
            Description = "Description",
            ImageFile = "product-1.png",
            Price = 900.00M,
            Category = new List<string>{ "Smart Phone" }
        },
        new Product{
            Id = new Guid("66921c71-2230-4ff6-8f15-cc3abb4d52f3"),
            Name = "IPhone 15 Plus",
            Description = "Description",
            ImageFile = "product-2.png",
            Price = 980.00M,
            Category = new List<string>{ "Smart Phone" }
        },
        new Product{
            Id = new Guid("02f317a1-194b-4ed6-9e57-4f878c5b6b55"),
            Name = "IPhone 15 Pro",
            Description = "Description",
            ImageFile = "product-3.png",
            Price = 1100.00M,
            Category = new List<string>{ "Smart Phone" }
        },
        new Product{
            Id = new Guid("fd9e3216-f950-4cdf-8183-85e911c2e703"),
            Name = "IPhone 15 Pro Max",
            Description = "Description",
            ImageFile = "product-4.png",
            Price = 1300.00M,
            Category = new List<string>{ "Smart Phone" }
        }
    };
}
 