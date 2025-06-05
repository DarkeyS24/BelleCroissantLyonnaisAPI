namespace BelleCroissantLyonnaisAPI.Models
{
    public class UserLogin
    {
        public int login_id { get; set; }
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string phone_number { get; set; } = string.Empty;
        public string profile_picture { get; set; } = string.Empty;
        public string security_question { get; set; } = string.Empty;
        public string security_answer { get; set; } = string.Empty;
        public string preferred_delivery_method { get; set; } = string.Empty;

        public List<Delivery_Address> Addresses { get; set; } = new List<Delivery_Address>();
    }
}
