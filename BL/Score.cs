namespace WorldCountry.BL
{
    public class Score
    {
        public int ScoreId { get; set; }
        public int UserId { get; set; }
        public string QuizType { get; set; }
        public int ScoreGained  { get; set; }
        public DateTime GameDate { get; set; }

        public Score(int scoreId, int userId, string quizType, int scoreGained, DateTime gameDate)
        {
            ScoreId = scoreId;
            UserId = userId;
            QuizType = quizType;
            ScoreGained = scoreGained;
            GameDate = gameDate;
        }


        public Score() { }


        public Score UserScore(int UserId, string QuizType)
        {
            DBservices dbs = new DBservices();
            return dbs.UserScore(UserId, QuizType);

        }


        public int UpdateScore()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateScore(this);
        }

        public int DeleteScore(int UserId, string QuizType)
        {
            DBservices dbs = new DBservices();

            return dbs.DeleteScore(UserId, QuizType);

        }

    }
}
