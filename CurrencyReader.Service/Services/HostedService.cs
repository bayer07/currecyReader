using CurrencyReader.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class HostedService : IHostedService
{
    public HostedService(
        CnbCzService currencyService,
        Parser parser,
        ILoggerFactory loggerFactory)
    {
        _currencyService = currencyService;
        _parser = parser;
        _logger = loggerFactory.CreateLogger<HostedService>();
    }

    private readonly CnbCzService _currencyService;
    private readonly Parser _parser;
    private readonly ILogger<HostedService> _logger;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync");
        while (true)
        {
            try
            {
                //List<Task> tasks = new List<Task>();
                //for (int year = 2019; year < 2021; year++)
                //{
                //    Task task = ReadRatesByYear(year);
                //    tasks.Add(task);
                //}
                //Task.WaitAll(tasks.ToArray());
                await ReadRatesByYear(DateTime.Now.Year);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                await Task.Delay(60000);
            }
        }
    }

    private async Task ReadRatesByYear(int year)
    {
        string response = await _currencyService.SendRequestByYear(year);
        var rates = _parser.FillCurrencyRateByYearFromText(response);
        //rates = rates.Where(x => x.Date > DateTime.Now.Date.AddDays(-1)); // Comment for full year processing
        using (var db = new ApplicationContext())
        {
            foreach (CurrencyRate rate in rates)
            {
                Currency? currency = db.Currencies.SingleOrDefault(x =>
                    rate.Currency.Name == x.Name &&
                    rate.Currency.Amount == x.Amount);
                if (currency == null)
                {
                    currency = rate.Currency;
                    db.Currencies.Add(currency);
                    await db.SaveChangesAsync();
                    _logger.LogInformation($"Currency Added:{currency}");
                }

                CurrencyRate currencyRate = db.CurrencyRates.SingleOrDefault(x =>
                rate.Date == x.Date &&
                currency.Id == x.CurrencyId);
                if (currencyRate == null)
                {
                    rate.CurrencyId = currency.Id;
                    currencyRate = rate;
                    db.CurrencyRates.Add(currencyRate);
                    await db.SaveChangesAsync();
                    _logger.LogInformation($"CurrencyRate Added:{currencyRate}");
                }
                else if (currencyRate.Price != rate.Price)
                {
                    currencyRate.Price = rate.Price;
                    await db.SaveChangesAsync();
                    _logger.LogInformation($"CurrencyRate Updated:{currencyRate}");
                }
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StopAsync");
    }
}