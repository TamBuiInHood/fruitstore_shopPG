using Product.API.Entities;
using System.Data.Common;
using ILogger = Serilog.ILogger;
namespace Product.API.Presistance
{
    public class ProductContextSeed
    {

        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {
            if (!productContext.Products.Any())
            {
                productContext.AddRange(GetCataLogProducts());
                await productContext.SaveChangesAsync();
                logger.Information("Seeding data for product db assosiate with context {DbContextName}", nameof(productContext));
            }
        }

        private static IEnumerable<CatalogProduct> GetCataLogProducts()
        {
            return new List<CatalogProduct>
            {
                new()
                {
                    No = "Lotus",
                    Name = "Esprit",
                    Summary = "Nondisplace fracture of grater trochanter of right demur",
                    Description = "Nondisplace fracture of grater trochanter of right demur",
                    Price = (decimal)177948.49
                },
                new()
                {
                    No = "Cadilac",
                    Name = "CTS",
                    Summary = "Carbuncle of trunk",
                    Description = "Carbuncle of trunk",
                    Price = (decimal)177564.11
                }
            };
        }
    }
}
