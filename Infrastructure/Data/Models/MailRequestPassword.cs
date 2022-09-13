namespace dotnet_2.Infrastructure.Data.Models
{
    public class MailRequestPassword
    {
        public string? to_email { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }
        public int user_id { get; set; }
        public string? username { get; set; }
    }
}