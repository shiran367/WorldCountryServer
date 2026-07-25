using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WorldCountry.BL;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("getUser")]
        public User GetUser(string token)
        {
          
            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int userId = Convert.ToInt32(decryptedText.Split('_')[0]);

            User user = new User();
            return user.Read(userId); 
        }



        // POST api/<UserController>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
           

            try
            {
                int numEffected = user.Register();

                if (numEffected > 0)
                {
                    return Ok(new { message = "User registered successfully!" ,userId = numEffected });
                }
                else
                {
                    return BadRequest(new { message = "Registration failed. No record was created." });
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new { message = "This email address is already registered." }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred during registration.", error = ex.Message });
            }
        }


        [HttpPost("Login")]
        public IActionResult PostLogin(string email, string password)
        {
            User user = new User();

            var result = user.Login(email, password);

            if (result.UserId != 0)
            {

                string rawText = result.UserId + "_SGGSecret";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(rawText);
                string secureToken = Convert.ToBase64String(bytes);



                return Ok(new
                {
                    token = secureToken,
                    name = result.Username,
                    email = result.Email,
                    islocked = result.IsLocked,
                    isAdmin =result.IsAdmin,
                    message = "Login successful! Welcome back."
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "Login failed: Invalid email or password."
                });
            }
        }



        [HttpPut("Update")]
        public IActionResult Put([FromHeader(Name ="mytoken")] string token, [FromBody] User user)
        {
      

            try
            {


                byte[] bytes = Convert.FromBase64String(token.Trim());
                string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);

                int userId = Convert.ToInt32(decryptedText.Split('_')[0]);

                user.UserId = userId;
                bool result = user.UpdateUser(user);

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





        [HttpPost("Logout")]
        public int PostLogout(string token)
        {

            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);


            User user = new User();

            return user.Logout(UserId);

         
        }








    }

}