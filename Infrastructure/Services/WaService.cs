using Microsoft.Extensions.Options;
using dotnet_2.Infrastructure.Data.Models;
    namespace dotnet_2.Infrastructure.Data.Services
    {
        class WaService : IWaService
        {
            private readonly WaSettings _waSettings;
            public WaService(IOptions<WaSettings> waSettings)
            {
                _waSettings = waSettings.Value;
            }
            public async Task SendWaAsync(MailRequest mailRequest)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://graph.facebook.com/v14.0/105579598927705/messages"),
                    Headers =
            {
                { "Authorization", "Bearer "+_waSettings.token },
            },
                    Content = new StringContent("{\"messaging_product\": \"whatsapp\", \"recipient_type\": \"individual\", \"to\": \"6282346018652\", \"type\": \"text\", \"text\": {   \"preview_url\": false,   \"body\": \"Your OTP code is " + mailRequest.body + "\" }}")
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
        }

    }