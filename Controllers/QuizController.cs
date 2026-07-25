using Microsoft.AspNetCore.Mvc;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        // GET: api/<QuizController>
        [HttpGet("GetFlagQuiz")]
        public FlagQuiz GetFlagQuiz()
        {
            FlagQuiz flagQuiz = new FlagQuiz();
            return flagQuiz.GetSingleFlagQuestion();
        }

        // GET api/<QuizController>/5
        [HttpGet("GetCultureCountryQuestion")]
        public List<CultureCountryQuestion> GetCultureQuestion()
        {
            CultureCountryQuestion cultureCountryQuestion=new CultureCountryQuestion();

            return cultureCountryQuestion.AllCultureQuestion();
        }

    }
}
