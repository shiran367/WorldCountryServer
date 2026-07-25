namespace WorldCountry.BL
{
    public class FlagQuiz
    {
        

        public int FlagHistoryId { get; set; }
        public int CountryId { get; set; }
        public string FlagUrl { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Options { get; set; } 
        public string FlagMeaningHistory { get; set; }

        public FlagQuiz(int flagHistoryId, int countryId, string flagUrl, string correctAnswer, List<string> options, string flagMeaningHistory)
        {
            FlagHistoryId = flagHistoryId;
            CountryId = countryId;
            FlagUrl = flagUrl;
            CorrectAnswer = correctAnswer;
            Options = options;
            FlagMeaningHistory = flagMeaningHistory;
        }

        public FlagQuiz GetSingleFlagQuestion()
        {
            DBservices dbs = new DBservices();
            return dbs.GetSingleFlagQuestion();
        }

        public FlagQuiz() { }



    }
}
