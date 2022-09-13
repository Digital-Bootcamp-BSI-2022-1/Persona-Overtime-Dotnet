namespace dotnet_2.Infrastructure.Dto
{
    public class LoginResponse
    {
        public string? Name {get; set;}
        public string? Email {get; set;}
        public string? Token {get; set;}
        public int ExpiredAt { get; set; }
    }
}

