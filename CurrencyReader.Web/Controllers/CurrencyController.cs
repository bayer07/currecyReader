using CurrencyReader.Data;
using CurrencyReader.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CurrencyReader.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;

    public CurrencyController(ILogger<CurrencyController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Currency> Get()
    {
        try
        {
            using (var repository = new ExchangeRepository())
            {
                var currencies = repository.Currencies.ToArray();
                return currencies;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{currencyId}")]
    public SelectCurrencyModel GetById(int currencyId)
    {
        try
        {
            using (var repository = new ExchangeRepository())
            {
                var result = repository.CurrencyRates
                    .Where(d => d.CurrencyId == currencyId)
                    .GroupBy(_ => 1, (_, records) => new SelectCurrencyModel
                    {
                        MaxDate = records.Max(r => r.Date),
                        MinDate = records.Min(r => r.Date)
                    })
                    .ToList()
                    .Single();
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{currencyId}/{startDate}/{endDate}")]
    public IEnumerable<CurrencyRate> GetRates(int currencyId, DateTime startDate, DateTime endDate)
    {
        try
        {
            using (var repository = new ExchangeRepository())
            {
                var result = repository.CurrencyRates
                    .Where(d => d.CurrencyId == currencyId && d.Date > startDate && d.Date < endDate)
                    .ToList();
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

public class SelectCurrencyModel
{
    public DateTime MinDate { get; set; }
    public DateTime MaxDate { get; set; }
}