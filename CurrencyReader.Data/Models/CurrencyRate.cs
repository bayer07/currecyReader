public class CurrencyRate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public float Price { get; set; }

    public int CurrencyId { get; set; }

    public Currency? Currency { get; set; }

    public override string ToString() => $"{Date} {Price} {Currency}";
}