public class CurrencyRate
{
    public DateOnly Date { get; set; }

    public float Price { get; set; }

    public Currency Currency { get; set; }

    public override string ToString() => $"{Date} {Price} {Currency}";
}