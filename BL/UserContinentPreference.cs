namespace WorldCountry.BL
{
    public class UserContinentPreference
    {


        public int UserId { get; set; }
        public string ContinentName { get; set; }

        public UserContinentPreference(int userId, string continentName)
        {
            UserId = userId;
            ContinentName = continentName;
        }


        public UserContinentPreference() { }



        public int RegisterContinent()
        {
            DBservices dbs = new DBservices();

          return dbs.RegisterContinent(this);
        }


        


          public bool UpdateUserContinent(UserContinentPreference continent)
        {

            DBservices dbs = new DBservices();

            int result = dbs.UpdateUserContinent(continent);

            return result > 0;


        }


    }
}
