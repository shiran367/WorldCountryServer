using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorldCountry.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WorldCountry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private const string GROQ_URL = "https://api.groq.com/openai/v1/chat/completions";

        public VacationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("GenerateRecommendations")]
        public async Task<IActionResult> GenerateRecommendations([FromBody] TripQuestion request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            string groqApiKey = _configuration["ApiKeys:GroqApiKey"];

            try
            {
                int totalPlaces = request.Days * 3;

                string prompt = $@"
CRITICAL INSTRUCTION: You MUST strictly customize ALL {totalPlaces} recommendations based ONLY on the user's explicit preferences below. 
You MUST INCLUDE specific restaurants, food markets, or culinary spots that match their food preferences, alongside attractions!
You MUST generate EXACTLY {totalPlaces} places in total ({request.Days} days x 3 places per day).

--- STRICT USER PREFERENCES ---
- Target Country: {request.Country}
- Trip Duration: {request.Days} days
- Group Type: {request.GroupType}
- Pace: {request.Pace}
- Start Time: {request.StartOfDayTime}
- Kid Friendly Required: {(request.KidFriendly ? "YES" : "NO")}
- Accessibility Needs Required: {(request.AccessibilityNeeds ? "YES" : "NO")}
- Kosher Status: {request.KosherStatus}
- Dietary Restrictions: {request.DietaryRestrictions}
- Food Style & Dining Preference: {request.FoodStyle}
- Nightlife Preference: {request.Nightlife}
- Events & Festivals Interest: {request.Events}
- Culture & History Preference: {request.CultureAndHistory}
- Nature & Outdoors Preference: {request.NatureAndOutdoors}
- Shopping Preference: {request.Shopping}
- Extreme & Attractions Preference: {request.ExtremeAndAttractions}
- Preferred Transportation: {request.Transportation}
- Preferred Language: {request.Language}
- Budget Level: {request.BudgetLevel}
- Accommodation Style: {request.AccommodationStyle}
- Preference Style: {request.HiddenGemsVsTourist}
- SPECIAL USER REQUESTS & NOTES: {(string.IsNullOrWhiteSpace(request.FreeTextNotes) ? "None" : request.FreeTextNotes)}

--- OUTPUT INSTRUCTIONS ---
Strictly follow this exact output structure:

[INTRODUCTION]
Write a warm, engaging introduction to {request.Country} (3-4 sentences) explaining explicitly how this itinerary was tailored to match their budget ({request.BudgetLevel}), pace ({request.Pace}), group ({request.GroupType}), dietary needs ({request.KosherStatus} / {request.FoodStyle}), accommodation preference ({request.AccommodationStyle}), and special requests.

[PLACES]
Organize the itinerary strictly by days (Day 1 to Day {request.Days}). Each day MUST contain EXACTLY 3 places.

Format strictly like this:

Day 1:
Place 1: [Place or Restaurant Name, City/Region]
- Address: [Full Street Address or City/Area, Target Country]
- Coordinates: [Latitude, Longitude]
- Why it fits: Explain specifically why this fits their exact preferences (highlighting culinary/kosher fit, kid-friendliness, accessibility, or special notes).
- Recommended duration: [Hours/Half day/Meal time]
- Practical tips: [Best time to visit, reservation tips, or food highlights]

Place 2: [Place or Restaurant Name, City/Region]
- Address: [Full Street Address or City/Area, Target Country]
- Coordinates: [Latitude, Longitude]
- Why it fits: Explain specifically why this fits their exact preferences.
- Recommended duration: [Hours/Half day/Meal time]
- Practical tips: [Best time to visit, reservation tips, or food highlights]

Place 3: [Place or Restaurant Name, City/Region]
- Address: [Full Street Address or City/Area, Target Country]
- Coordinates: [Latitude, Longitude]
- Why it fits: Explain specifically why this fits their exact preferences.
- Recommended duration: [Hours/Half day/Meal time]
- Practical tips: [Best time to visit, reservation tips, or food highlights]

Day 2:
Place 4: [Place or Restaurant Name, City/Region]
...and continue through Day {request.Days} up to Place {totalPlaces}.

Do not include any conversational filler, disclaimers, or meta-commentary.";

                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
                    {
                new
                {
                    role = "system",
                    content = "You are an expert, friendly travel consultant."
                },
                new { role = "user", content = prompt }
            },
                    temperature = 0.5,
                    max_tokens = 3500
                };

                string jsonPayload = JsonConvert.SerializeObject(requestBody);

                using (var client = new HttpClient())
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, GROQ_URL);
                    requestMessage.Headers.Add("Authorization", $"Bearer {groqApiKey}");
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(requestMessage);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { message = "Groq API error", details = errorContent });
                    }

                    string responseString = await response.Content.ReadAsStringAsync();

                    dynamic rawResult = JsonConvert.DeserializeObject(responseString);
                    string recommendationsText = rawResult.choices[0].message.content;

                    return Ok(new
                    {
                        country = request.Country,
                        recommendations = recommendationsText
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error occurred.", error = ex.Message });
            }
        }





        [HttpPost("getMeList")]
        public async Task<IActionResult> GetMeList([FromBody] TripQuestion request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            string groqApiKey = _configuration["ApiKeys:GroqApiKey"];


            try
            {



                string packingPrompt = $@"
You are an expert, highly meticulous personal travel consultant. Your job is to create a fully customized, intelligent packing list based ONLY on the trip and user profile below.

--- TRIP PROFILE & USER PREFERENCES ---
- Target Country: {request.Country}
- Trip Duration: {request.Days} Days
- Group Type: {request.GroupType}
- Pace: {request.Pace}
- Kid Friendly Required: {(request.KidFriendly ? "YES" : "NO")}
- Accessibility Needs Required: {(request.AccessibilityNeeds ? "YES" : "NO")}
- Kosher Status: {request.KosherStatus}
- Dietary Restrictions: {request.DietaryRestrictions}
- Food Style & Dining Preference: {request.FoodStyle}
- Nightlife Preference: {request.Nightlife}
- Culture & History Preference: {request.CultureAndHistory}
- Nature & Outdoors Preference: {request.NatureAndOutdoors}
- Extreme & Attractions Preference: {request.ExtremeAndAttractions}
- Shopping Preference: {request.Shopping}
- Preferred Transportation: {request.Transportation}
- Accommodation Style: {request.AccommodationStyle}
- Preferred Language: {request.Language}
- SPECIAL USER NOTES: {(string.IsNullOrWhiteSpace(request.FreeTextNotes) ? "None" : request.FreeTextNotes)}

--- YOUR TASK & REASONING PROCESS ---
1. CLIMATE ANALYSIS: Determine the geographical location and general climate/weather of {request.Country}. Adapt all clothing, outerwear, and skin/body care to match that climate (e.g., cold weather gear vs. tropical/warm gear vs. rain gear).
2. DURATION CALCULATION: Quantify clothing and essential items based strictly on a {request.Days}-day trip duration (include extra pairs of underwear and socks for safety).
3. PROFILE MATCHING:
   - Carefully read the user's special preferences (Kosher status, kids, dietary restrictions, outdoor activities, nightlife, accessibility, and free text notes).
   - Deduce and generate specific items that directly address and solve these specific needs (without being prompted with item names).
4. COMPREHENSIVENESS: Provide a deep, complete, and highly practical packing list so the user doesn't forget anything.

--- OUTPUT FORMAT INSTRUCTIONS ---
You MUST organize your entire output into EXACTLY these 4 categories.
Do NOT write any intro, disclaimers, or conversational text. Return ONLY the category lines and the bulleted items beneath them.

Category: Essential Documents & Money
(List all necessary travel documents, IDs, payment methods, reservations, and insurance tailored to {request.Country} and user transport)

Category: Clothing, Underwear & Footwear
(List climate-appropriate tops, bottoms, outerwear, footwear for planned activities, sleepwear, and exact quantities of undergarments for {request.Days} days)

Category: Toiletries & Personal Hygiene
(List all personal care, dental, grooming, sun protection, or skin care products adapted to the destination and activities)

Category: Tech, Health & Personal Special Requests
(List all relevant electronics, travel adapters for {request.Country}, health/medication needs, and explicit items derived from user preferences, kids, kosher status, or notes)


                Do NOT write any introductory text, disclaimers, or conversational filler. Return ONLY the categories and items matching the format above.";


                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
      {
                new
                {
                    role = "system",
                    content = "You are an expert, friendly travel consultant."
                },
                new { role = "user", content = packingPrompt }
            },
                    temperature = 0.5,
                    max_tokens = 3500
                };

                string jsonPayload = JsonConvert.SerializeObject(requestBody);

                using (var client = new HttpClient())
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, GROQ_URL);
                    requestMessage.Headers.Add("Authorization", $"Bearer {groqApiKey}");
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(requestMessage);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, new { message = "Groq API error", details = errorContent });
                    }

                    string responseString = await response.Content.ReadAsStringAsync();

                    dynamic rawResult = JsonConvert.DeserializeObject(responseString);
                    string recommendationsText = rawResult.choices[0].message.content;

                    return Ok(new
                    {
                        country = request.Country,
                        recommendations = recommendationsText
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error occurred.", error = ex.Message });
            }
        }






        [HttpGet("GetCountryName")]
        public List<string> GetCountryName()
        {
            Emergency emergency = new Emergency();
            return emergency.EmmergencyCountryName();
        }



        [HttpGet("GetEmergency")]
        public List<Emergency> GetEmenrgency(string CountryName)
        {
            Emergency emergency = new Emergency();
            return emergency.EmergencyCountry(CountryName);
        }
















    }







}



