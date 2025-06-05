namespace BelleCroissantLyonnaisAPI.Models
{
    public class Mailing_Subscription
    {
        public int mailing_id { get; set; }
        public int login_id { get; set; }
        public bool is_subscribed { get; set; }
    }
}
