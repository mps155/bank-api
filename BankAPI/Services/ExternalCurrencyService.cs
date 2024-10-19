namespace BankAPI.Services
{
    public class ExternalCurrencyService
    {
        private readonly HttpClient _httpClient;

        public ExternalCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetCurrencyFromExternalApiAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (HttpRequestException e)
            {
                throw e; 
            }
        }
    }
}
