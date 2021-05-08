using System.Threading.Tasks;

public interface IProductRepository
{
    public Task<Product> GetProduct(long id);
    public Price GetProductPrice(long id);
    public Task<bool> UpdateProductPrice(long id, decimal price);
}
