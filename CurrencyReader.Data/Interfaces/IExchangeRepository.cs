namespace CurrencyReader.Data.Interfaces
{
    internal interface IExchangeRepository
    {
        IQueryable<Currency> Currencies { get; }
        IQueryable<CurrencyRate> CurrencyRates { get; }
        Task Add(Currency currency);
        Task Add(CurrencyRate currency);
        Task SaveAsync();
    }
}
