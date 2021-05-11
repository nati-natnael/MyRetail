using System.Threading.Tasks;

public interface IProductRepository
{
    public Task<Product> GetProductAsync(long id);
    public Task<Price> GetProductPriceAsync(long id);
    public Task<bool> UpdateProductPriceAsync(long id, double price);
}
