using System.Text.Json.Serialization;

namespace BelleCroissantLyonnaisAPI.Models
{
    public class Order
    {
        public int order_id { get; set; }
        public int customer_id { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly time { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int payment_method_id { get; set; }
        public int channel_id { get; set; }
        public int store_id { get; set; }
        public int promotion_id { get; set; }
        public decimal discount_amount { get; set; }

        [JsonIgnore]
        public Customer customer { get; set; }
        [JsonIgnore]
        public Product product { get; set; }
        [JsonIgnore]
        public Channel channel { get; set; }
        [JsonIgnore]
        public Payment_Method payment { get; set; }
    }
}
