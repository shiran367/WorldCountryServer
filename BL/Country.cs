namespace WorldCountry.BL
{
    public class Country
    {


        public int CountryId { get; set; }
        public string NameOfficial { get; set; }
        public string NameCommon { get; set; }
        public string CapitalCity { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public long Population { get; set; }
        public double AreaKm2 { get; set; }
        public string FlagUrl { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public List<Currency> Currencies { get; set; } = new List<Currency>();
        public DateTime LastUpdatedFromApi { get; set; }



        public Country(int countryId, string nameOfficial, string nameCommon, string capitalCity, string region, string subregion, long population, double areaKm2, string flagUrl, List<string> languages, List<Currency> currencies, DateTime lastUpdatedFromApi)
        {
            CountryId = countryId;
            NameOfficial = nameOfficial;
            NameCommon = nameCommon;
            CapitalCity = capitalCity;
            Region = region;
            Subregion = subregion;
            Population = population;
            AreaKm2 = areaKm2;
            FlagUrl = flagUrl;
            Languages = languages;
            Currencies = currencies;
            LastUpdatedFromApi = lastUpdatedFromApi;
        }


        public Country() { }



        public int InsertCountry()
        {
            DBservices dbs = new DBservices();

            return dbs.InsertCountry(this);
        }

        public List<Country> readAllCountry()
        {
            DBservices dbs = new DBservices();

            return dbs.readAllCountry();

        }


        public int UpdateCountry(int id, Country country)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateCountry(id, country);
        }

        public int postCountry()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertCountry(this);
        }


        public int DeleteCountry(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteCountry(id);
        }
    }
}
