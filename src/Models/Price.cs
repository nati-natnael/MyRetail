using MongoDB.Bson.Serialization.Attributes;

public class Price
{
    public decimal Value { get; set; }
    public string Currency { get; set; }
}
