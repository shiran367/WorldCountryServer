using Microsoft.AspNetCore.Mvc;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        // GET: api/<ScoreController>
        [HttpGet("GetScore")]
        public Score GetScore(string token,string QuizType)
        {

            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);

            Score score = new Score();

            Score resultScore = score.UserScore(UserId, QuizType);

            return resultScore;
        }



     

    

        // PUT api/<ScoreController>/5
        [HttpPut("PutScore")]
        public int PutScore(string token, [FromBody] Score score)
        {
            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);

            score.UserId = UserId;

            return score.UpdateScore();

        }




  







        // DELETE api/<ScoreController>/5
        [HttpDelete("DeleteScore")]
        public int Delete(string token, string QuizType)
        {
            byte[] bytes = Convert.FromBase64String(token.Trim());

            string decryptedText = System.Text.Encoding.UTF8.GetString(bytes);
            int UserId = Convert.ToInt32(decryptedText.Split('_')[0]);
            Score score = new Score();

            return score.DeleteScore(UserId, QuizType);



        }
    }
}
