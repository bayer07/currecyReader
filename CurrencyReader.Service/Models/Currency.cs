public class Currency
{
    public string Name { get; set; }

    public int Amount { get; set; }

    public override string ToString() => $"{Amount} {Name}";
}