using Microsoft.Extensions.Hosting;

public class HostedService : IHostedService
{
    public HostedService(
        CnbCzService currencyService,
        Parser parser)
    {
        _currencyService = currencyService;
        _parser = parser;
    }

    private readonly CnbCzService _currencyService;
    private readonly Parser _parser;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var rates = new List<CurrencyRate>();
        for (int year = 1990; year < 2025; year++)
        {
            string text = await _currencyService.SendRequestByYear(year);
            var r = _parser.FillCurrencyRateByYearFromText(text);
            rates.AddRange(r);
        }
        var g = rates.GroupBy(x => x.Currency.Name);
        int c = g.Count();
        var cur = g.First();
        var a = rates.Where(x => x.Currency.Name == "CZK");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}