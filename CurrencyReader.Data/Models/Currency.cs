using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(Id))]
public class Currency
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public override string ToString() => $"{Amount} {Name}";
}