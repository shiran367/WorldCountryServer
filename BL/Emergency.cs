namespace WorldCountry.BL
{
    public class Emergency
    {
      
        public int EmergencyId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string EmergencyType { get; set; }
        public string Num {  get; set; }

        public Emergency(int emergencyId, int countryId, string countryName, string emergencyType, string num)
        {
            EmergencyId = emergencyId;
            CountryId = countryId;
            CountryName = countryName;
            EmergencyType = emergencyType;
            Num = num;
        }
        public Emergency() { }

        public List<string> EmmergencyCountryName()
        {
            DBservices dbs = new DBservices();

            return dbs.EmmergencyCountryName();
        }


        public List<Emergency> EmergencyCountry(string CountryName)
        {
            DBservices dbs = new DBservices();

            return dbs.EmergencyCountry(CountryName);
        }
    }
}
