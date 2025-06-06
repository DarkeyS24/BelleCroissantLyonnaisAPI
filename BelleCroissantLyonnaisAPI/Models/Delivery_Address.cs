namespace BelleCroissantLyonnaisAPI.Models
{
    public class Delivery_Address
    {
        public int delivery_address_id { get; set; }
        public int login_id { get; set; }
        public string address { get; set; } = string.Empty;
    }
}
