using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Currency
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public override string ToString() => $"{Amount} {Name}";
}