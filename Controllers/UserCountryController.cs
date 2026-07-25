using Microsoft.AspNetCore.Mvc;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCountryController : ControllerBase
    {
        // GET: api/<UserCountryController>
        [HttpGet("getUserCountry")]
        public List<Country> GetUserCountry(string token , string listType)
        {

            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);

            UserCountry userCountry = new UserCountry();
            return userCountry.GetUserCountry(UserId, listType);
        }



        // POST api/<UserCountryController>
        [HttpPost("postUserCountry")]
        public int postUserCountry(string token,[FromBody] UserCountry userCountry)
        {
            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);

            userCountry.UserId = UserId;

            return userCountry.InsertUserCountry();
        }


        // DELETE api/<UserCountryController>/5
        [HttpDelete("DeleteUserCountry")]
        public int Delete(string token,int CountryId)
        {
            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);

            UserCountry userCountry = new UserCountry();
            return userCountry.DeleteUserCountry(UserId, CountryId);
        }
    }
}
