using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorldCountry.BL;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {

        // POST api/<UserPreferencesController>
        [HttpPost("RegisterContinentPref")]
        public int PostContinent([FromBody] UserContinentPreference continent)
        {

            return continent.RegisterContinent(); 
        }



        // POST api/<UserPreferencesController>
        [HttpPost("RegisterLanguage")]
        public int PostLanguage([FromBody] UserLanguage language)
        {

            return language.RegisterLanguage();
        }



        [HttpPut("UpdateLanguage")]
        public IActionResult UpadateLanguage([FromHeader(Name = "mytoken")] string token, [FromBody] UserLanguage language)
        {

            try
            {

                byte[] bytes = Convert.FromBase64String(token.Trim());
                string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);

                int userId = Convert.ToInt32(decryptedText.Split('_')[0]);

                language.UserId = userId;





                bool result = language.UpdateUserLanguage(language);

                if (result)
                {
                    return Ok(new
                    {
                        message = "Update success."
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = "Update failed: Invalid id or name or password."
                    });
                }
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Token validation failed. Unauthorized access." });
            }
        }







        [HttpPut("UpdateContinent")]
        public IActionResult UpdateContinent([FromHeader(Name = "mytoken")] string token, [FromBody] UserContinentPreference continent)
        {

            try
            {

                byte[] bytes = Convert.FromBase64String(token.Trim());
                string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);

                int userId = Convert.ToInt32(decryptedText.Split('_')[0]);

                continent.UserId = userId;

                bool result = continent.UpdateUserContinent(continent);

                if (result)
                {
                    return Ok(new
                    {
                        message = "Update success."
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = "Update failed: Invalid id or name or password."
                    });
                }
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Token validation failed. Unauthorized access." });
            }
        }







    }
}




