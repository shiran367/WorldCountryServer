namespace WorldCountry.BL
{
    public class UserLanguage
    {
      

        public int UserId { get; set; }
        public string LanguageName { get; set; }
        public string ProficiencyLevel { get; set; }




        public UserLanguage(int userId, string languageName, string proficiencyLevel)
        {
            UserId = userId;
            LanguageName = languageName;
            ProficiencyLevel = proficiencyLevel;
        }


        public UserLanguage() { }




        public int RegisterLanguage()
        {
            DBservices dbs =new DBservices();

            return dbs.RegisterLanguage(this);
        }


        public bool UpdateUserLanguage(UserLanguage language)
        {

            DBservices dbs = new DBservices();

            int result = dbs.UpdateUserLanguage(language);

            return result > 0;


        }



    }


}
