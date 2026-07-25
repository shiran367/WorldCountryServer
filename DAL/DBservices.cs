using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using WorldCountry.BL;
using WorldCountry.Controllers;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection.Metadata;



public class DBservices
{

    public DBservices()
    {

    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }








    //--------------------------------------------------------------------------------------------------
    // This method inserts a user into the users table 
    // the model CCEC - Connect, Create Command, Execute, Close
    //--------------------------------------------------------------------------------------------------
    public int Register(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserName", user.Username);
        paramDic.Add("@Email", user.Email);
        paramDic.Add("@PasswordHash", user.PasswordHash);
        paramDic.Add("@FullName", user.FullName);
  

        cmd = CreateCommandWithStoredProcedureGeneral("spRegisterUser_SSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                int newUserId = Convert.ToInt32(result);
                return newUserId; 
            }

            throw new Exception("Failed to retrieve the new User ID.");
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }









    //--------------------------------------------------------------------------------------------------
    // This method inserts a favorite continent into the continent table 
    //--------------------------------------------------------------------------------------------------
    public int RegisterContinent(UserContinentPreference userContinent)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userContinent.UserId);
        paramDic.Add("@continentName", userContinent.ContinentName);



        cmd = CreateCommandWithStoredProcedureGeneral("spRegisterUserContinent_SSR", con, paramDic);          // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }






    //--------------------------------------------------------------------------------------------------
    // This method Locked/dislocked a user
    //--------------------------------------------------------------------------------------------------
    public int Locked(int UserId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);


        cmd = CreateCommandWithStoredProcedureGeneral("spLockUser_SSR", con, paramDic);          // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }








    //--------------------------------------------------------------------------------------------------
    // This method Reads all users
    //--------------------------------------------------------------------------------------------------
    public List<User> AllUsers()
    {
        SqlConnection con = null;
        SqlCommand cmd;
        List<User> users = new List<User>();    
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }

   
        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllUsers_SSR", con, null);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {

            while (dataReader.Read())
            {
                User u = new User();

                u.UserId = Convert.ToInt32(dataReader["UserId"]);
                u.Username = dataReader["Username"].ToString();
                u.Email = dataReader["Email"].ToString();
                u.PasswordHash = dataReader["PasswordHash"].ToString();
                u.FullName = dataReader["FullName"].ToString();
                u.RegistrationDate = Convert.ToDateTime(dataReader["RegistrationDate"]);
                u.IsLocked = Convert.ToBoolean(dataReader["IsLocked"]);
                u.IsAdmin = Convert.ToBoolean(dataReader["IsAdmin"]);
                u.IsActive = Convert.ToBoolean(dataReader["IsActive"]);

                users.Add(u);
            }

            return users;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }














    //--------------------------------------------------------------------------------------------------
    // This method Reads a single user from id
    //--------------------------------------------------------------------------------------------------
    public User ReadUserById(int UserId) 
    {
        SqlConnection con = null;
        SqlCommand cmd;
        User user = null; 

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);

        cmd = CreateCommandWithStoredProcedureGeneral("spReadUserById_SSR", con, paramDic);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
           
            if (dataReader.Read())
            {
                user = new User();

                user.UserId = Convert.ToInt32(dataReader["UserId"]);
                user.Username = dataReader["Username"].ToString();
                user.Email = dataReader["Email"].ToString();
                user.PasswordHash = dataReader["PasswordHash"].ToString();
                user.FullName = dataReader["FullName"].ToString();
                user.RegistrationDate = Convert.ToDateTime(dataReader["RegistrationDate"]);
                user.IsLocked = Convert.ToBoolean(dataReader["IsLocked"]);
                user.IsAdmin = Convert.ToBoolean(dataReader["IsAdmin"]);
                user.IsActive = Convert.ToBoolean(dataReader["IsActive"]);
            }

            return user;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }




    //--------------------------------------------------------------------------------------------------
    // This method inserts a user language the users language table 
    //--------------------------------------------------------------------------------------------------
    public int RegisterLanguage(UserLanguage language)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", language.UserId);
        paramDic.Add("@LanguageName", language.LanguageName);
        paramDic.Add("@ProficiencyLevel", language.ProficiencyLevel);


        cmd = CreateCommandWithStoredProcedureGeneral("spRegisterUserLanguage_SSR", con, paramDic);          // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    //--------------------------------------------------------------------------------------------------
    // This method login user
    //--------------------------------------------------------------------------------------------------
    public (int UserId, string Username, string Email, bool IsAdmin, bool IsLocked) login(string email, string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        int userId = 0;
        string userName = null;
        string userEmail = null;
        bool isAdmin = false;
        bool isLocked = false;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Email", email);
       

        cmd = CreateCommandWithStoredProcedureGeneral("spLoginUser_SSR", con, paramDic);

        SqlDataReader dataReader = cmd.ExecuteReader();

        try
        {
            while (dataReader.Read())
            {
                string hashedPassword = dataReader["PasswordHash"].ToString();

                bool currentIsLocked = Convert.ToBoolean(dataReader["IsLocked"]);
                bool currentIsAdmin = Convert.ToBoolean(dataReader["IsAdmin"]);

                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                {
                    userId = Convert.ToInt32(dataReader["UserId"]);
                    userName = dataReader["Username"].ToString();
                    userEmail = dataReader["Email"].ToString();
                    isAdmin = currentIsAdmin;
                    isLocked = currentIsLocked;
                }
                else
                {
                    isLocked = currentIsLocked;
                }
            }

            dataReader.Close();

            if (userId > 0 && !isLocked)
            {
                string updateQuery = "UPDATE [Users_SSR] SET IsActive = 1 WHERE UserId = @UserId";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                {
                    updateCmd.Parameters.AddWithValue("@UserId", userId);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    updateCmd.ExecuteNonQuery();
                }
            }

            return (userId, userName, userEmail, isAdmin, isLocked);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null )
                con.Close();
        }
    }






    //--------------------------------------------------------------------------------------------------
    // This method update a user
    //--------------------------------------------------------------------------------------------------
    public int UpdateUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


    Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", user.UserId);
        paramDic.Add("@UserName", user.Username);
        paramDic.Add("@Email", user.Email);
        paramDic.Add("@PasswordHash", user.PasswordHash);
        paramDic.Add("@FullName", user.FullName);

        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateUserSSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }




    //--------------------------------------------------------------------------------------------------
    // This method update a user language
    //--------------------------------------------------------------------------------------------------
    public int UpdateUserLanguage(UserLanguage language)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
         

    Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", language.UserId);
        paramDic.Add("@LanguageName", language.LanguageName);
        paramDic.Add("@ProficiencyLevel", language.ProficiencyLevel);
     

        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateUserLanguageSSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    //--------------------------------------------------------------------------------------------------
    // This method update a user continent
    //--------------------------------------------------------------------------------------------------
    public int UpdateUserContinent(UserContinentPreference continent)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", continent.UserId);
        paramDic.Add("@ContinentName", continent.ContinentName);
     


        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateContinentSSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }



    //--------------------------------------------------------------------------------------------------
    // This method logout a user
    //--------------------------------------------------------------------------------------------------
    public int Logout(int UserId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId",UserId);

        cmd = CreateCommandWithStoredProcedureGeneral("spLogoutUserSSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }










    //--------------------------------------------------------------------------------------------------
    // This method read all country
    //--------------------------------------------------------------------------------------------------

    public List<Country> readAllCountry()
    {
        SqlConnection con;
        SqlCommand cmd;

        Dictionary<int, Country> countryDictionary = new Dictionary<int, Country>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllCountry_SSR", con, null);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {
                int countryId = Convert.ToInt32(dataReader["CountryId"]);

                if (!countryDictionary.ContainsKey(countryId))
                {
                    Country c = new Country();
                    c.CountryId = countryId;
                    c.NameOfficial = dataReader["NameOfficial"].ToString();
                    c.NameCommon = dataReader["NameCommon"].ToString();
                    c.CapitalCity = dataReader["CapitalCity"].ToString();
                    c.Region = dataReader["Region"].ToString();
                    c.Subregion = dataReader["Subregion"].ToString();
                    c.Population = Convert.ToInt64(dataReader["Population"]);
                    c.AreaKm2 = Convert.ToDouble(dataReader["AreaKm2"]);
                    c.FlagUrl = dataReader["FlagUrl"].ToString();

                    c.Languages = new List<string>();
                    c.Currencies = new List<Currency>();

                    countryDictionary.Add(countryId, c);
                }

                Country currentCountry = countryDictionary[countryId];

                if (dataReader["LanguageName"] != DBNull.Value)
                {
                    string languageName = dataReader["LanguageName"].ToString();
                    if (!currentCountry.Languages.Contains(languageName))
                    {
                        currentCountry.Languages.Add(languageName);
                    }
                }

                if (dataReader["CurrencyCode"] != DBNull.Value)
                {
                    string currCode = dataReader["CurrencyCode"].ToString();
                    string currName = dataReader["CurrencyName"] != DBNull.Value ? dataReader["CurrencyName"].ToString() : "";

                    if (!currentCountry.Currencies.Any(curr => curr.CurrencyCode == currCode))
                    {
                        currentCountry.Currencies.Add(new Currency
                        {
                            CurrencyCode = currCode,
                            CurrencyName = currName
                        });
                    }
                }
            }

            dataReader.Close();

            return countryDictionary.Values.ToList();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }







    //--------------------------------------------------------------------------------------------------
    // This method inserts a country to the country  table 
    //--------------------------------------------------------------------------------------------------
    public int InsertCountry(Country country)
    {



        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@NameOfficial", country.NameOfficial);
        paramDic.Add("@NameCommon", country.NameCommon);
        paramDic.Add("@CapitalCity", string.IsNullOrEmpty(country.CapitalCity) ? "" : country.CapitalCity);
        paramDic.Add("@Region", string.IsNullOrEmpty(country.Region) ? "Unknown" : country.Region);
        paramDic.Add("@Subregion", string.IsNullOrEmpty(country.Subregion) ? "Unknown" : country.Subregion);
        paramDic.Add("@Population", country.Population);
        paramDic.Add("@AreaKm2", country.AreaKm2);
        paramDic.Add("@FlagUrl", string.IsNullOrEmpty(country.FlagUrl) ? "" : country.FlagUrl);
        paramDic.Add("@LastUpdatedFromApi", country.LastUpdatedFromApi);


    cmd = CreateCommandWithStoredProcedureGeneral("spInsertCountry_SSR", con, paramDic);          // create the command

        int newCountryId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newCountryId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

            if (country.Languages != null && country.Languages.Count > 0)
            {
                foreach (var languageName in country.Languages)
                {
                    Dictionary<string, object> langParams = new Dictionary<string, object>();
                    langParams.Add("@CountryId", newCountryId);
                    langParams.Add("@LanguageName", languageName);

                    SqlCommand langCmd = CreateCommandWithStoredProcedureGeneral("spInsertCountryLanguage_SSR", con, langParams);
                    langCmd.ExecuteNonQuery();
                }
            }

            if (country.Currencies != null && country.Currencies.Count > 0)
            {
                foreach (var currency in country.Currencies)
                {
                    Dictionary<string, object> currParams = new Dictionary<string, object>();
                    currParams.Add("@CountryId", newCountryId);
                    currParams.Add("@CurrencyCode", currency.CurrencyCode);
                    currParams.Add("@CurrencyName", currency.CurrencyName);

                    SqlCommand currCmd = CreateCommandWithStoredProcedureGeneral("spInsertCountryCurrency_SSR", con, currParams);
                    currCmd.ExecuteNonQuery();
                }
            }

            return newCountryId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // This method update a country
    //--------------------------------------------------------------------------------------------------
    public int UpdateCountry(int id,Country country)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("CountryId" ,id);
        paramDic.Add("@NameOfficial", country.NameOfficial);
        paramDic.Add("@NameCommon", country.NameCommon);
        paramDic.Add("@CapitalCity", string.IsNullOrEmpty(country.CapitalCity) ? "" : country.CapitalCity);
        paramDic.Add("@Region", string.IsNullOrEmpty(country.Region) ? "Unknown" : country.Region);
        paramDic.Add("@Subregion", string.IsNullOrEmpty(country.Subregion) ? "Unknown" : country.Subregion);
        paramDic.Add("@Population", country.Population);
        paramDic.Add("@AreaKm2", country.AreaKm2);
        paramDic.Add("@FlagUrl", string.IsNullOrEmpty(country.FlagUrl) ? "" : country.FlagUrl);
        paramDic.Add("@LastUpdatedFromApi", country.LastUpdatedFromApi);



        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateContrySSR", con, paramDic);          // create the command

        int newCountryId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newCountryId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

            if (country.Languages != null && country.Languages.Count > 0)
            {
                foreach (var languageName in country.Languages)
                {
                    Dictionary<string, object> langParams = new Dictionary<string, object>();
                    langParams.Add("@CountryId", newCountryId);
                    langParams.Add("@LanguageName", languageName);

                    SqlCommand langCmd = CreateCommandWithStoredProcedureGeneral("spUpdateCountryLanguage_SSR", con, langParams);
                    langCmd.ExecuteNonQuery();
                }
            }

            if (country.Currencies != null && country.Currencies.Count > 0)
            {
                foreach (var currency in country.Currencies)
                {
                    Dictionary<string, object> currParams = new Dictionary<string, object>();
                    currParams.Add("@CountryId", newCountryId);
                    currParams.Add("@CurrencyCode", currency.CurrencyCode);
                    currParams.Add("@CurrencyName", currency.CurrencyName);

                    SqlCommand currCmd = CreateCommandWithStoredProcedureGeneral("[spUpdateCountryCurrency_SSR]", con, currParams);
                    currCmd.ExecuteNonQuery();
                }
            }


            return newCountryId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    //--------------------------------------------------------------------------------------------------
    // This method deletes a country from the country table 
    //--------------------------------------------------------------------------------------------------
    public int DeleteCountry(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CountryId", id);


        cmd = CreateCommandWithStoredProcedureGeneral("spDeleteCountry_SSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar(); if (result != null)
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }







    //--------------------------------------------------------------------------------------------------
    // This method read all country from User
    //--------------------------------------------------------------------------------------------------

    public List<Country> GetUserCountry(int UserId, string listType)
    {
        SqlConnection con;
        SqlCommand cmd;

        Dictionary<int, Country> countryDictionary = new Dictionary<int, Country>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);
        if (string.IsNullOrEmpty(listType) || listType.ToUpper() == "ALL")
        {
            paramDic.Add("@ListType", DBNull.Value);
        }
        else
        {
            paramDic.Add("@ListType", listType);
        }


        cmd = CreateCommandWithStoredProcedureGeneral("spReadUserCountry_SSR", con, paramDic);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {
                int countryId = Convert.ToInt32(dataReader["CountryId"]);

                if (!countryDictionary.ContainsKey(countryId))
                {
                    Country c = new Country();
                    c.CountryId = countryId;
                    c.NameOfficial = dataReader["NameOfficial"].ToString();
                    c.NameCommon = dataReader["NameCommon"].ToString();
                    c.CapitalCity = dataReader["CapitalCity"].ToString();
                    c.Region = dataReader["Region"].ToString();
                    c.Subregion = dataReader["Subregion"].ToString();
                    c.Population = Convert.ToInt64(dataReader["Population"]);
                    c.AreaKm2 = Convert.ToDouble(dataReader["AreaKm2"]);
                    c.FlagUrl = dataReader["FlagUrl"].ToString();

                    c.Languages = new List<string>();
                    c.Currencies = new List<Currency>();

                    countryDictionary.Add(countryId, c);
                }

                Country currentCountry = countryDictionary[countryId];

                if (dataReader["LanguageName"] != DBNull.Value)
                {
                    string languageName = dataReader["LanguageName"].ToString();
                    if (!currentCountry.Languages.Contains(languageName))
                    {
                        currentCountry.Languages.Add(languageName);
                    }
                }

                if (dataReader["CurrencyCode"] != DBNull.Value)
                {
                    string currCode = dataReader["CurrencyCode"].ToString();
                    string currName = dataReader["CurrencyName"] != DBNull.Value ? dataReader["CurrencyName"].ToString() : "";

                    if (!currentCountry.Currencies.Any(curr => curr.CurrencyCode == currCode))
                    {
                        currentCountry.Currencies.Add(new Currency
                        {
                            CurrencyCode = currCode,
                            CurrencyName = currName
                        });
                    }
                }
            }

            dataReader.Close();

            return countryDictionary.Values.ToList();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }




    

    //--------------------------------------------------------------------------------------------------
    // This method inserts a user country to the user country  table 
    //--------------------------------------------------------------------------------------------------
    public int InsertUserCountry(UserCountry userCountry)
    {



        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userCountry.UserId);
        paramDic.Add("@CountryId", userCountry.CountryId);
        paramDic.Add("@ListType", userCountry.ListType);



        cmd = CreateCommandWithStoredProcedureGeneral("spInsertUserCountry_SSR", con, paramDic);          // create the command

        int newUserCountryId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newUserCountryId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

        return newUserCountryId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }





    //--------------------------------------------------------------------------------------------------
    // This method delete a user country from the UserCountry table 
    //--------------------------------------------------------------------------------------------------
    public int DeleteUserCountry(int UserId,int CountryId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);
        paramDic.Add("@CountryId", CountryId);



        cmd = CreateCommandWithStoredProcedureGeneral("spDeleteUserCountry_SSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar(); if (result != null)
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }



    //--------------------------------------------------------------------------------------------------
    // This method read all Shares 
    //--------------------------------------------------------------------------------------------------

    public List<Share> AllShares()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Share> share = new List<Share>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGeneral("spReadTotalShares_SSR", con, null);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {


                Share s = new Share();
                s.ShareId = Convert.ToInt32(dataReader["ShareId"]);
                s.UserId = Convert.ToInt32(dataReader["UserId"]);
                s.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                s.Content = dataReader["Content"].ToString();
                s.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                s.Rating = Convert.ToInt32(dataReader["Rating"]);
                s.FullName = dataReader["FullName"].ToString();
                share.Add(s);

            }

            return share;


        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // This method read all Shares by country
    //--------------------------------------------------------------------------------------------------

    public List<Share> AllCountryShares(int CountryId)
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Share> share = new List<Share>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CountryId", CountryId);

        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllShares_SSR", con, paramDic);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {


                Share s = new Share();
                s.ShareId = Convert.ToInt32(dataReader["ShareId"]);
                s.UserId = Convert.ToInt32(dataReader["UserId"]);
                s.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                s.Content = dataReader["Content"].ToString();
                s.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                s.Rating = Convert.ToInt32(dataReader["Rating"]);
                s.FullName = dataReader["FullName"].ToString();

                share.Add(s);

            }

            return share;

       
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }




    //--------------------------------------------------------------------------------------------------
    // This method read all Shares by user
    //--------------------------------------------------------------------------------------------------

    public List<Share> AllUserShares(int UserId)
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Share> share = new List<Share>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);

        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllUserShares_SSR", con, paramDic);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {


                Share s = new Share();
                s.ShareId = Convert.ToInt32(dataReader["ShareId"]);
                s.UserId = Convert.ToInt32(dataReader["UserId"]);
                s.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                s.Content = dataReader["Content"].ToString();
                s.CreatedAt = Convert.ToDateTime(dataReader["CreatedAt"]);
                s.Rating = Convert.ToInt32(dataReader["Rating"]);
                s.FullName = dataReader["FullName"].ToString();

                share.Add(s);

            }

            return share;


        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }



    //--------------------------------------------------------------------------------------------------
    // This method inserts a user share to the  share  table 
    //--------------------------------------------------------------------------------------------------
    public int InsertShare(Share share)
    {



        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", share.UserId);
        paramDic.Add("@CountryId", share.CountryId);
        paramDic.Add("@Content", share.Content);
        paramDic.Add("@CreatedAt", share.CreatedAt);
        paramDic.Add("@Rating", share.Rating);





        cmd = CreateCommandWithStoredProcedureGeneral("spInsertUserShare_SSR", con, paramDic);          // create the command

        int newUserCountryId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newUserCountryId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

            return newUserCountryId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }







    //--------------------------------------------------------------------------------------------------
    // This method inserts a user share to the  share  table 
    //--------------------------------------------------------------------------------------------------
    public int UpdateShare(Share share)
    {



        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@ShareId", share.ShareId);
        paramDic.Add("@UserId", share.UserId);
        paramDic.Add("@CountryId", share.CountryId);
        paramDic.Add("@Content", share.Content);
        paramDic.Add("@CreatedAt", share.CreatedAt);
        paramDic.Add("@Rating", share.Rating);





        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateUserShare_SSR", con, paramDic);          // create the command

        int newUserCountryId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newUserCountryId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

            return newUserCountryId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }




    //--------------------------------------------------------------------------------------------------
    // This method delete a user share
    //--------------------------------------------------------------------------------------------------
    public int DeleteShare(int ShareId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@ShareId", ShareId);




        cmd = CreateCommandWithStoredProcedureGeneral("spDeleteShare_SSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar(); if (result != null)
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    //--------------------------------------------------------------------------------------------------
    // This method reads ONE random Flag Question with 3 distractors directly from the DB
    //--------------------------------------------------------------------------------------------------
    public FlagQuiz GetSingleFlagQuestion()
    {
        SqlConnection con = null;
        SqlCommand cmd;
        FlagQuiz quizQuestion = new FlagQuiz { Options = new List<string>() };

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = CreateCommandWithStoredProcedureGeneral("spGetSingleFlagQuestion_SSR", con, null);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string optionName = dataReader["OptionName"].ToString();
                bool isCorrect = Convert.ToBoolean(dataReader["IsCorrect"]);

                quizQuestion.Options.Add(optionName);

                if (isCorrect)
                {
                    quizQuestion.FlagUrl = dataReader["FlagUrl"].ToString();
                    quizQuestion.CorrectAnswer = optionName;
                    quizQuestion.FlagMeaningHistory = dataReader["FlagMeaningHistory"].ToString();
                }
            }
            dataReader.Close();

            Random rand = new Random();
            quizQuestion.Options = quizQuestion.Options.OrderBy(x => rand.Next()).ToList();

            return quizQuestion;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }




    


    //--------------------------------------------------------------------------------------------------
    // This method read all Culture question
    //--------------------------------------------------------------------------------------------------

    public List<CultureCountryQuestion> AllCultureQuestion()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<CultureCountryQuestion> cultureCountryQuestions = new List<CultureCountryQuestion>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllCultureQuestion_SSR", con, null);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {

                  CultureCountryQuestion c = new CultureCountryQuestion();
                c.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
                c.QuestionText = dataReader["QuestionText"].ToString();
                c.IsCorrect = Convert.ToBoolean(dataReader["IsCorrect"]);
                c.Explanation = dataReader["Explanation"].ToString();


                cultureCountryQuestions.Add(c);

            }

            return cultureCountryQuestions;


        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }





    //--------------------------------------------------------------------------------------------------
    // This method reads the specific score by User and Quiz type
    //--------------------------------------------------------------------------------------------------
    public Score UserScore(int userId, string quizType)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        Score score = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw ex;
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userId);
        paramDic.Add("@QuizType", quizType);

        cmd = CreateCommandWithStoredProcedureGeneral("spReadUserScore_SSR", con, paramDic);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                score = new Score();
                score.ScoreId = Convert.ToInt32(dataReader["ScoreId"]);
                score.UserId = Convert.ToInt32(dataReader["UserId"]);
                score.QuizType = dataReader["QuizType"].ToString(); 
                score.ScoreGained = Convert.ToInt32(dataReader["ScoreGained"]);

                if (dataReader["GameDate"] != DBNull.Value)
                {
                    score.GameDate = Convert.ToDateTime(dataReader["GameDate"]);
                }
            }

            dataReader.Close();
            return score;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    

    //--------------------------------------------------------------------------------------------------
    // This method update a score  to the  score  table 
    //--------------------------------------------------------------------------------------------------
    public int UpdateScore(Score score)
    {



        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@ScoreId", score.ScoreId);
        paramDic.Add("@UserId", score.UserId);
        paramDic.Add("@QuizType", score.QuizType);
        paramDic.Add("@ScoreGained", score.ScoreGained);
        paramDic.Add("@GameDate", score.GameDate);

   
       
  



        cmd = CreateCommandWithStoredProcedureGeneral("spUpdateUserScore_SSR", con, paramDic);          // create the command

        int newScoreId = 0;

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                newScoreId = Convert.ToInt32(result);
            }
            else
            {
                throw new Exception("error to ad a new county");
            }

            return newScoreId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }






    //--------------------------------------------------------------------------------------------------
    // This method delete a user score
    //--------------------------------------------------------------------------------------------------
    public int DeleteScore(int UserId, string QuizType)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", UserId);
        paramDic.Add("@QuizType", QuizType);







        cmd = CreateCommandWithStoredProcedureGeneral("spDeleteScore_SSR", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar(); if (result != null)
            {
                return Convert.ToInt32(result); // יחזיר 1 אם נמחק, 0 אם לא נמצא
            }

            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }





    //--------------------------------------------------------------------------------------------------
    // This method Reads a single user from id
    //--------------------------------------------------------------------------------------------------
    public AdminDashboardStats AllData(DateTime fromDate, DateTime toDate)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        AdminDashboardStats adminDashboard = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@fromDate", fromDate);
        paramDic.Add("@toDate", toDate);


        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllData_SSR", con, paramDic);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {

            if (dataReader.Read())
            {
                adminDashboard = new AdminDashboardStats();
                adminDashboard.DailyLogins = dataReader["DailyLogins"] != DBNull.Value ? Convert.ToInt32(dataReader["DailyLogins"]) : 0;
                adminDashboard.CountriesImported = dataReader["CountriesImported"] != DBNull.Value ? Convert.ToInt32(dataReader["CountriesImported"]) : 0;
                adminDashboard.CountriesSaved = dataReader["CountriesSaved"] != DBNull.Value ? Convert.ToInt32(dataReader["CountriesSaved"]) : 0;
                adminDashboard.SharesCreated = dataReader["SharesCreated"] != DBNull.Value ? Convert.ToInt32(dataReader["SharesCreated"]) : 0;
            }

            return adminDashboard;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }





    //--------------------------------------------------------------------------------------------------
    // This method Read all Country from table emmergency
    //--------------------------------------------------------------------------------------------------
    public List<string> EmmergencyCountryName()
    {
        SqlConnection con = null;
        SqlCommand cmd;
        List<string> countryNames = new List<string>();
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }


        cmd = CreateCommandWithStoredProcedureGeneral("spReadAllEmergencyCountry_SSR", con, null);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {

            while (dataReader.Read())
            {

                string CountryName = dataReader["CountryName"].ToString();


                countryNames.Add(CountryName);
            }

            return countryNames;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }





    //--------------------------------------------------------------------------------------------------
    // This method Read emergency by CountryName
    //--------------------------------------------------------------------------------------------------
    public List<Emergency> EmergencyCountry(string CountryName)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        List<Emergency> emergencies = new List<Emergency>();
        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@CountryName", CountryName);

        cmd = CreateCommandWithStoredProcedureGeneral("spReadEmergencyCountry_SSR", con, paramDic);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {

            while (dataReader.Read())
            {

                Emergency e = new Emergency();
                e.EmergencyId = Convert.ToInt32(dataReader["EmergencyId"]);
                e.CountryId = Convert.ToInt32(dataReader["CountryId"]);
                e.CountryName = dataReader["CountryName"].ToString();
                e.EmergencyType = dataReader["EmergencyType"].ToString();
                e.Num= dataReader["Num"].ToString();


                emergencies.Add(e);
            }

            return emergencies;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }














    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }




}
