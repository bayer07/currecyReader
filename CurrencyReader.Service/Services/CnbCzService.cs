public class CnbCzService
{
    public CnbCzService(
        IHttpClientFactory httpClientFactory,
        Parser parser)
    {
        _httpClientFactory = httpClientFactory;
        _parser = parser;
    }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Parser _parser;

    public async Task<string> SendRequestByYear(int year)
    {
        using (HttpClient httpClient = _httpClientFactory.CreateClient())
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={year}");
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}