using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _httpClient;
    private readonly IMongoCollection<BsonDocument> _collection;

    public ProductRepository(HttpClient httpClient, IMongoCollection<BsonDocument> collection)
    {
        _httpClient = httpClient;
        _collection = collection;
    }

    public async Task<Product> GetProductAsync(long id)
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

    public async Task<Price> GetProductPriceAsync(long id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", id);

        IAsyncCursor<BsonDocument> cursor = await _collection.FindAsync(filter);

        BsonDocument result = cursor.FirstOrDefault();

        if (result == null)
        {
            return null;
        }

        result.TryGetValue("current_price", out BsonValue currentPrice);

        if (currentPrice == null)
        {
            return null;
        }

        BsonDocument currentPriceDoc = currentPrice.AsBsonDocument;

        currentPriceDoc.TryGetValue("value", out BsonValue priceValue);
        currentPriceDoc.TryGetValue("currency_code", out BsonValue currency);

        return new Price
        {
            Value = (decimal) (priceValue?.AsDouble ?? 0),
            Currency = currency?.AsString
        };
    }

    public async Task<bool> UpdateProductPriceAsync(long id, decimal price)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", id);
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set<double>("current_price.value", (double)price);

        UpdateResult result = await _collection.UpdateOneAsync(filter, update);

        return result?.IsAcknowledged ?? false;
    }
}
