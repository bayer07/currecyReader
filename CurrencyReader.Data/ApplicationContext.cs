using Microsoft.EntityFrameworkCore;

namespace CurrencyReader.Data;
public class ApplicationContext : DbContext
{
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-191P6LO;Database=currencyReader;persist security info=True;Integrated Security=SSPI;TrustServerCertificate=true");
    }
}
