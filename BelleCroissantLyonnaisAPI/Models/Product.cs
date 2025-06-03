using System.Text.Json.Serialization;

namespace BelleCroissantLyonnaisAPI.Models
{
    public class Product
    {
        public int product_id { get; set; }
        public string product_name { get; set; } = string.Empty;
        public int category_id { get; set; }
        public string ingredients { get; set; } = string.Empty;
        public decimal price { get; set; }
        public decimal cost { get; set; }
        public bool seasonal { get; set; }
        public bool active { get; set; }
        public DateOnly introduced_date { get; set; }

        public Category? category { get; set; }
    }
}
