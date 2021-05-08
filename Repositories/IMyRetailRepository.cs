using System.Threading.Tasks;

public interface IMyRetailRepository
{
    public Task<Product> GetProduct(long id);
    public Price GetProductPrice(long id);
}
