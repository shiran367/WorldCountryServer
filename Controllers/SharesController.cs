using Microsoft.AspNetCore.Mvc;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharesController : ControllerBase
    {
        // GET: api/<SharesController>
        [HttpGet("CountryShares")]
        public List<Share> GetCountryShares(int CountryId)
        {
            Share shares = new Share();
            return shares.AllCountryShares(CountryId); 
        }

        // GET: api/<SharesController>
        [HttpGet("UserShares")]
        public List<Share> GetUserShares(int UserId)
        {
            Share shares = new Share();
            return shares.AllUserShares(UserId);
        }

        // GET: api/<SharesController>
        [HttpGet("AllShares")]
        public List<Share> GetAllShares()
        {
            Share shares = new Share();
            return shares.AllShares();
        }





        // POST api/<SharesController>
        [HttpPost("PostShare")]
        public int PostShare([FromBody] Share share)
        {
            return share.InsertShare();
        }


        // PUT api/<SharesController>/5
        [HttpPut("PutShare")]
        public int PutUserShare([FromBody] Share share)
        {
            return share.UpdateShare(share);
        }

        // DELETE api/<SharesController>/5
        [HttpDelete("DeleteShare")]
        public int DeleteShare(int Shareid)
        {
            Share share = new Share();
            return share.DeleteShare( Shareid);
        }
    }
}
