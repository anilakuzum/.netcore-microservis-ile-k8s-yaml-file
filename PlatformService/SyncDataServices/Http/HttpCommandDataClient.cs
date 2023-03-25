using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private ICommandDataClient _commandDataClient;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration, ICommandDataClient commandDataClient)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _commandDataClient = commandDataClient;

        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
                );


            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync Post to CommandService Was Ok");
            }
            else
            {
                Console.WriteLine("Sync Post to CommandService Was Not Ok");
            }

        }
    }
}
