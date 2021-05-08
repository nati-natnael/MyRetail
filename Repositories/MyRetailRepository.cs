using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class MyRetailRepository : IMyRetailRepository
{
    private const string dbConnectionString = "mongodb+srv://MyRetail:myretail@cluster0.88sdm.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
    private const string dbName = "MyRetail";
    private const string collectioName = "Product";

    private readonly HttpClient _httpClient;

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<BsonDocument> _collection;


    public MyRetailRepository()
    {
        _httpClient = new HttpClient();

        _client = new MongoClient(dbConnectionString);
        _database = _client.GetDatabase(dbName);
        _collection = _database.GetCollection<BsonDocument>(collectioName);
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
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", id);

        BsonDocument result = _collection.Find(filter).FirstOrDefault();

        Price price = new Price
        {
            Value = (decimal)(result["current_price"]["value"]?.AsDouble ?? 0),
            Currency = result["current_price"]["currency_code"]?.AsString ?? string.Empty
        };

        return price;
    }

    public Product UpdateProductPrice(long id, decimal price)
    {
        return null;
    }
}
