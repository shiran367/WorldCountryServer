namespace WorldCountry.BL
{
    public class AdminDashboardStats
    {
       

        public int DailyLogins { get; set; }
        public int CountriesImported { get; set; }
        public int CountriesSaved { get; set; }
        public int SharesCreated { get; set; }

        public AdminDashboardStats(int dailyLogins, int countriesImported, int countriesSaved, int sharesCreated)
        {
            DailyLogins = dailyLogins;
            CountriesImported = countriesImported;
            CountriesSaved = countriesSaved;
            SharesCreated = sharesCreated;
        }


        public AdminDashboardStats() { }    


        public AdminDashboardStats AllData(DateTime fromDate, DateTime toDate)
        {
            DBservices dbs = new DBservices();

            return dbs.AllData(fromDate, toDate);
        }
    }
}
