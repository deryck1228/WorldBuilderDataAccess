using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using WorldBuilderMvcWeb.Models;

namespace WorldBuilderMvcWeb.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _functionCode;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            ILogger<ApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = configuration["ApiBaseUrl"];
            _functionCode = configuration["FunctionCode"];
            _logger = logger;
        }

        private string AppendFunctionCode(string url)
        {
            if (url.Contains("?"))
            {
                return $"{url}&code={_functionCode}";
            }
            else
            {
                return $"{url}?code={_functionCode}";
            }
        }

        public async Task<int> GetTotalCharacterCount()
        {
            var url = AppendFunctionCode($"{_baseUrl}/characters/count");
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(jsonResponse);
        }

        public async Task<List<Character>> GetAllCharacters(int page, int pageSize)
        {
            var url = AppendFunctionCode($"{_baseUrl}/characters?page={page}&pageSize={pageSize}");
            _logger.LogInformation($"Request URL: {url}");

            var response = await _httpClient.GetAsync(url);
            _logger.LogInformation($"Response Status: {response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Character>>(jsonResponse);
        }

        public async Task<Character> GetCharacter(int id)
        {
            var url = AppendFunctionCode($"{_baseUrl}/character/{id}");
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Character>(jsonResponse);
        }

        public async Task<Character> CreateCharacter(Character character)
        {
            var json = JsonConvert.SerializeObject(character);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = AppendFunctionCode($"{_baseUrl}/character");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Character>(jsonResponse);
        }

        public async Task<Character> UpdateCharacter(int id, Character character)
        {
            var json = JsonConvert.SerializeObject(character);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = AppendFunctionCode($"{_baseUrl}/character/{id}");
            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Character>(jsonResponse);
        }
    }
}
