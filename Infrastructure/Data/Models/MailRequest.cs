namespace dotnet_2.Infrastructure.Data.Models
{
    public class MailRequest
    {
        public string? to_email { get; set; }
        public string? subject { get; set; }
        public int body { get; set; }
        public int user_id { get; set; }
        public string? user_name { get; set; }
    }
}