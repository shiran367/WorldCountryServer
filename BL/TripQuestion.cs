namespace WorldCountry.BL
{
    public class TripQuestion
    {
       
        public string Country { get; set; }
        public int Days { get; set; }
        public string GroupType { get; set; }
        public string Pace { get; set; } 

        
        public string KosherStatus { get; set; } 
        public string DietaryRestrictions { get; set; } 
        public string FoodStyle { get; set; }

        
        public string Nightlife { get; set; } 
        public string Events { get; set; } 

        
        public string CultureAndHistory { get; set; }
        public string NatureAndOutdoors { get; set; } 
        public string Shopping { get; set; }
        public string ExtremeAndAttractions { get; set; } 

       
        public string Transportation { get; set; } 
        public string Language { get; set; } 
        public bool AccessibilityNeeds { get; set; } 

       
        public string BudgetLevel { get; set; } 
        public string AccommodationStyle { get; set; } 

        
        public string StartOfDayTime { get; set; } 
        public string HiddenGemsVsTourist { get; set; } 
        public bool KidFriendly { get; set; }
        public string FreeTextNotes { get; set; }


        public TripQuestion(string country, int days, string groupType, string pace, string kosherStatus, string dietaryRestrictions, string foodStyle, string nightlife, string events, string cultureAndHistory, string natureAndOutdoors, string shopping, string extremeAndAttractions, string transportation, string language, bool accessibilityNeeds, string budgetLevel, string accommodationStyle, string startOfDayTime, string hiddenGemsVsTourist, bool kidFriendly, string freeTextNotes)
        {
            Country = country;
            Days = days;
            GroupType = groupType;
            Pace = pace;
            KosherStatus = kosherStatus;
            DietaryRestrictions = dietaryRestrictions;
            FoodStyle = foodStyle;
            Nightlife = nightlife;
            Events = events;
            CultureAndHistory = cultureAndHistory;
            NatureAndOutdoors = natureAndOutdoors;
            Shopping = shopping;
            ExtremeAndAttractions = extremeAndAttractions;
            Transportation = transportation;
            Language = language;
            AccessibilityNeeds = accessibilityNeeds;
            BudgetLevel = budgetLevel;
            AccommodationStyle = accommodationStyle;
            StartOfDayTime = startOfDayTime;
            HiddenGemsVsTourist = hiddenGemsVsTourist;
            KidFriendly = kidFriendly;
            FreeTextNotes = freeTextNotes;
        }






    }
}
