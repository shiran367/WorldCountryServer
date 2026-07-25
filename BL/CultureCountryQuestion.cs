namespace WorldCountry.BL
{
    public class CultureCountryQuestion
    {


        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; }


        public CultureCountryQuestion(int questionId, string questionText, bool isCorrect, string explanation)
        {
            QuestionId = questionId;
            QuestionText = questionText;
            IsCorrect = isCorrect;
            Explanation = explanation;
        }

        public List<CultureCountryQuestion> AllCultureQuestion()
        {
           DBservices dbs = new DBservices();

            return dbs.AllCultureQuestion();
        }


        public CultureCountryQuestion() { }
    }
}
