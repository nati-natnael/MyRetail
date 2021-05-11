using System.Text.Json.Serialization;

public class Price
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}
