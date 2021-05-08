using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class MyRetailRepository : IMyRetailRepository
{
    private readonly HttpClient _httpClient;

    public MyRetailRepository()
    {
        _httpClient = new HttpClient();
    }

    public async Task<Product> GetProduct(long id)
    {
        string baseUrl = "https://redsky.target.com/v3/pdp/tcin";
        string exclude = "taxonomy,promotion,bulk_ship,rating_and_review_reviews," +
                         "rating_and_review_statistics,question_answer_statistics," +
                         "available_to_promise_network";

        string request = $"{baseUrl}/{id}?excludes={exclude}&key=candidate";

        HttpResponseMessage response = await _httpClient.GetAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        JObject json = JObject.Parse(responseBody);
        JToken productName = json.SelectToken("product.item.product_description.title");

        Product product = new Product
        {
            Id = id,
            Name = productName?.Value<string>()
        };

        return product;
    }

    public Price GetProductPrice(long id)
    {
        return new Price
        {
            Value = 13.80m,
            Currency = "USD"
        };
    }
}
