namespace dotnet_2.Infrastructure.Data.Models{
    public class MailSettings
    {
        public string? host { get; set; }
        public int port { get; set; }
        public string? password { get; set; }
        public string? username { get; set; }
        public string? from { get; set; }
    }
}