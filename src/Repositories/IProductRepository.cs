using System.Threading.Tasks;

public interface IProductRepository
{
    public Task<Product> GetProductAsync(long id);
    public Price GetProductPrice(long id);
    public Task<bool> UpdateProductPriceAsync(long id, decimal price);
}
