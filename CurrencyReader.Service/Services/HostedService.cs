using CurrencyReader.Data;
using CurrencyReader.Data.Repositories;
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
                for (int year = 2019; year < 2021; year++)
                {
                    await ReadRatesByYear(year);
                }
                //await ReadRatesByYear(DateTime.Now.Year);
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
        using (var repository = new ExchangeRepository())
        {
            foreach (CurrencyRate rate in rates)
            {
                Currency? currency = repository.Currencies.SingleOrDefault(x =>
                    rate.Currency.Name == x.Name &&
                    rate.Currency.Amount == x.Amount);
                if (currency == null)
                {
                    currency = rate.Currency;
                    await repository.Add(currency);
                    await repository.SaveAsync();
                    _logger.LogInformation($"Currency Added:{currency}");
                }

                CurrencyRate currencyRate = repository.CurrencyRates.SingleOrDefault(x =>
                rate.Date == x.Date &&
                currency.Id == x.CurrencyId);
                if (currencyRate == null)
                {
                    currencyRate = rate;
                    currencyRate.CurrencyId = currency.Id;
                    currencyRate.Currency = null;
                    await repository.Add(currencyRate);
                    await repository.SaveAsync();
                    _logger.LogInformation($"CurrencyRate Added:{currencyRate}");
                }
                else if (currencyRate.Price != rate.Price)
                {
                    currencyRate.Price = rate.Price;
                    await repository.SaveAsync();
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