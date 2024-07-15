using System.Text.Json.Serialization;

namespace shop.Models 
{
    public class Product
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("image")]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public string FullImagePath { get; set; }
    }
}