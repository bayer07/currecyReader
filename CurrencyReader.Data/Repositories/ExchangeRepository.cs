using CurrencyReader.Data.Interfaces;

namespace CurrencyReader.Data.Repositories
{
    public class ExchangeRepository : IExchangeRepository, IDisposable
    {
        public ExchangeRepository()
        {
            _db = new ApplicationContext();
        }

        private readonly ApplicationContext _db;

        public IQueryable<Currency> Currencies
        {
            get
            {
                return _db.Currencies;
            }
        }

        public IQueryable<CurrencyRate> CurrencyRates
        {
            get
            {
                return _db.CurrencyRates;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Add(Currency currency)
        {
            await _db.Currencies.AddAsync(currency);
        }

        public async Task Add(CurrencyRate currencyRate)
        {
            await _db.CurrencyRates.AddAsync(currencyRate);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
