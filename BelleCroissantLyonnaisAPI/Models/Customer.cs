using System.Text.Json.Serialization;

namespace BelleCroissantLyonnaisAPI.Models
{
    public class Customer
    {
        public int customer_id { get; set; }
        public string full_name { get; set; } = string.Empty;
        public short age { get; set; }
        public string gender { get; set; } = string.Empty;
        public string postal_code { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone_number { get; set; } = string.Empty;
        public int membership_id { get; set; }
        public DateOnly join_date { get; set; }
        public DateOnly last_purchase_date { get; set; }
        public decimal total_spending { get; set; }
        public decimal average_order_value { get; set; }
        public int frequency { get; set; }
        public int preferred_category { get; set; }
        public bool churned { get; set; }

        [JsonIgnore]
        public Membership? membership { get; set; }
    }
}
