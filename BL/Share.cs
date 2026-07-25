namespace WorldCountry.BL
{
    public class Share
    {
      

        public int ShareId { get; set; }
        public int UserId { get; set; }
        public int CountryId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt {  get; set; }
        public int Rating { get; set; }
        public string? FullName { get; set; }




        public Share() { }

        public Share(int shareId, int userId, int countryId, string content, DateTime createdAt, int rating, string fullName)
        {
            ShareId = shareId;
            UserId = userId;
            CountryId = countryId;
            Content = content;
            CreatedAt = createdAt;
            Rating = rating;
            FullName = fullName;
        }

        public List<Share> AllCountryShares(int CountryId)
        {
            DBservices dbs = new DBservices();
            return dbs.AllCountryShares(CountryId);
        }



        public List<Share> AllUserShares(int UserId)
        {
            DBservices dbs = new DBservices();
            return dbs.AllUserShares(UserId);
        }

        public List<Share> AllShares()
        {
            DBservices dbs = new DBservices();
            return dbs.AllShares();
        }

        public int InsertShare()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertShare(this);
        }

       
        public int UpdateShare(Share share)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateShare(share);
        }


        public int DeleteShare(int ShareId)
        {

        DBservices dbs = new DBservices();

         return dbs.DeleteShare(ShareId);

         }


    }
}
