namespace WorldCountry.BL
{
    public class UserCountry
    {

        public int UserId { get; set; }
        public int CountryId { get; set; }
        public string ListType { get; set; }
        public DateTime SavedAt { get; set; }


        public UserCountry(int userId, int countryId, string listType, DateTime savedAt)
        {
            UserId = userId;
            CountryId = countryId;
            ListType = listType;
            SavedAt = savedAt;
        }

        public UserCountry() { }



        public List<Country> GetUserCountry(int UserId, string listType)
        {
            DBservices dbs = new DBservices();

            return dbs.GetUserCountry(UserId, listType);
        }



        public int InsertUserCountry()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertUserCountry(this);
        }

        public int DeleteUserCountry(int UserId, int CountryId)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteUserCountry(UserId, CountryId);
        }
    }
}
