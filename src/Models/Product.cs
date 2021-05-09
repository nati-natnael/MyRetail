using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Price Price { get; set; }
}
