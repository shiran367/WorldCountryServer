
namespace WorldCountry.BL;

public class Currency
{
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }



   
    public Currency(string currencyCode, string currencyName)
    {
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
    }



    public Currency()
    {
    }


}