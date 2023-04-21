public class Parser
{
    private const string RowSeparator = "\n";
    private const string ColumnSeparator = "|";
    private const string CurrencySeparator = " ";
    private const string DateFormat = "dd.MM.yyyy";
    private const string NewDateMarker = "Date";


    public IEnumerable<CurrencyRate> FillCurrencyRateByYearFromText(string text)
    {
        var rows = text.Split(RowSeparator);

        var currencies = new List<Currency>();
        var rates = new List<CurrencyRate>();
        for (int i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            var cells = rows[i].Split(ColumnSeparator);
            string first = cells[0];
            if (first == NewDateMarker)
            {
                if (currencies.Any())
                {
                    currencies.Clear();
                }

                for (int j = 1; j < cells.Length; j++)
                {
                    var amountAndName = cells[j].Split(CurrencySeparator);
                    int amount = int.Parse(amountAndName[0]);
                    string name = amountAndName[1];
                    currencies.Insert(j - 1, new Currency { Amount = amount, Name = name });
                }

                continue;
            }
            else if (first == "")
            {
                continue;
            }

            try
            {
                DateOnly date = DateOnly.ParseExact(first, DateFormat);
                for (int j = 1; j < cells.Length; j++)
                {
                    var currency = currencies[j - 1];
                    float price = float.Parse(cells[j]);
                    rates.Insert(j - 1, new CurrencyRate { Date = date, Currency = currency, Price = price });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        return rates;
    }
}