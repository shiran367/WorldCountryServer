using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("GetWorldApi")]
        public async Task<IActionResult> GetWorld()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    string apiKey = _configuration["ApiKeys:WordApiKey"];

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    var allObjects = new JsonArray();

                    for (int offset = 0; offset <= 200; offset += 100)
                    {
                        string url = $"https://api.restcountries.com/countries/v5?pretty=1&limit=100&offset={offset}";

                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();

                            var jsonNode = JsonNode.Parse(jsonString);
                            var objects = jsonNode?["data"]?["objects"]?.AsArray();

                            if (objects != null)
                            {
                                foreach (var obj in objects)
                                {
                                    if (obj != null)
                                    {
                                        allObjects.Add(JsonNode.Parse(obj.ToJsonString()));
                                    }
                                }
                            }
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            return StatusCode((int)response.StatusCode, $"External API Error at offset {offset}: {errorContent}");
                        }
                    }

                    var finalResponse = new
                    {
                        data = new
                        {
                            objects = allObjects
                        }
                    };

                    return Ok(finalResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<CountryController>/5
        [HttpGet("getAllCountry")]
        public List<Country> GetAllCountry()
        {
            Country country = new Country();
            return country.readAllCountry();
        }



        // POST api/<CountryController>/postFromApi
        [HttpPost("postFromApi")]
        public async Task<IActionResult> PostToDB()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiKey = "rc_live_a01fe77413dd4045b9dd2d1f64af50cb";
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    int savedCount = 0;

                    // לולאה הרצה על ה-Offsets כדי למשוך את כל המדינות בעולם מה-API
                    for (int offset = 0; offset <= 200; offset += 100)
                    {
                        string url = $"https://api.restcountries.com/countries/v5?pretty=1&limit=100&offset={offset}";

                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var jsonNode = JsonNode.Parse(jsonString);

                            var objects = jsonNode?["data"]?["objects"]?.AsArray();

                            if (objects != null)
                            {
                                foreach (var obj in objects)
                                {
                                    if (obj != null)
                                    {

                                        long parsedPopulation = 0;
                                        if (obj["population"] != null)
                                        {
                                            long.TryParse(obj["population"].ToString(), out parsedPopulation);
                                        }


                                        double parsedArea = 0.0;
                                        if (obj["area"]?["kilometers"] != null)
                                        {
                                            double.TryParse(obj["area"]["kilometers"].ToString(), out parsedArea);
                                        }


                                        string capital = null;
                                        var capitalsArray = obj["capitals"]?.AsArray();
                                        if (capitalsArray != null && capitalsArray.Count > 0)
                                        {
                                            capital = capitalsArray[0]?["name"]?.ToString();
                                        }


                                        Country newCountry = new Country
                                        {
                                            NameOfficial = obj["names"]?["official"]?.ToString(),
                                            NameCommon = obj["names"]?["common"]?.ToString(),
                                            CapitalCity = capital,
                                            Region = obj["region"]?.ToString(),
                                            Subregion = obj["subregion"]?.ToString(),
                                            Population = parsedPopulation,
                                            AreaKm2 = parsedArea,
                                            FlagUrl = obj["flag"]?["url_png"]?.ToString(),
                                            LastUpdatedFromApi = DateTime.Now
                                        };

                                        var languagesArray = obj["languages"]?.AsArray();
                                        if (languagesArray != null)
                                        {
                                            foreach (var langObj in languagesArray)
                                            {
                                                var nameProp = langObj?["name"]?.ToString();
                                                if (!string.IsNullOrEmpty(nameProp))
                                                {
                                                    newCountry.Languages.Add(nameProp);
                                                }
                                            }
                                        }

                                        var currenciesArray = obj["currencies"]?.AsArray();
                                        if (currenciesArray != null)
                                        {
                                            foreach (var currObj in currenciesArray)
                                            {
                                                if (currObj != null)
                                                {
                                                    newCountry.Currencies.Add(new Currency
                                                    {
                                                        CurrencyCode = currObj["code"]?.ToString(),
                                                        CurrencyName = currObj["name"]?.ToString(),
                                                    });
                                                }
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(newCountry.NameOfficial))
                                        {
                                            int generatedId = newCountry.InsertCountry();

                                            if (generatedId > 0)
                                            {
                                                savedCount++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            return StatusCode((int)response.StatusCode, $"External API Error at offset {offset}: {errorContent}");
                        }
                    }

                    return Ok(new { message = $"Process complete. Successfully populated/updated {savedCount} countries in the database." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        // POST api/<CountryController>/post
        [HttpPost("postCountry")]

        public int postCountry([FromBody] Country country)
        {
            int numEffected = country.postCountry();

            return numEffected;


        }





    // PUT api/<CountryController>/5
    [HttpPut("Update")]
        public int UpdateCountry(int id, [FromBody] Country country )
        {
            return country.UpdateCountry(id,country);
        }

        // DELETE api/<CountryController>/5
        [HttpDelete("DeleteCountry")]
        public int Delete(int id)
        {
            Country country = new Country();

           return country.DeleteCountry(id);
        }
    }
}

