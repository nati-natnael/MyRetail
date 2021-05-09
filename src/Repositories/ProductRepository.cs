using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private const string _dbUsername = "MyRetail";
    private const string _dbPassword = "myretail";
    private const string _dbName = "MyRetailDB";
    private const string _collectioName = "Products";

    private readonly string _dbConnectionString;

    private readonly HttpClient _httpClient;

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<BsonDocument> _collection;


    public ProductRepository()
    {
        _httpClient = new HttpClient();

        _dbConnectionString = $"mongodb+srv://{_dbUsername}:{_dbPassword}@cluster0.88sdm.mongodb.net";

        _client = new MongoClient(_dbConnectionString);
        _database = _client.GetDatabase(_dbName);
        _collection = _database.GetCollection<BsonDocument>(_collectioName);
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

    public Price GetProductPrice(long id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", id);

        BsonDocument result = _collection.Find(filter).FirstOrDefault();

        BsonValue currentPrice = result?["current_price"];

        if (currentPrice == null)
        {
            return null;
        }

        return new Price
        {
            Value = (decimal)(currentPrice["value"]?.AsDouble ?? 0),
            Currency = currentPrice["currency_code"]?.AsString ?? string.Empty
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
