using Newtonsoft.Json;

namespace AutoDealersHelper.Parsers
{
    public class AutoRiaResponse
    {
        [JsonProperty("result")]
        public Result Result { get; set; } 
    }

    public class Result
    { 
        [JsonProperty("search_result")]
        public AutoRiaSearchResult SearchResult { get; set; }
    }

    public class Advertisement
    { 
        [JsonProperty("USD")]
        public int Price { get; set; }

        [JsonProperty("markId")]
        public int Brand { get; set; }

        [JsonProperty("modelId")]
        public int Model { get; set; }

        [JsonProperty("linkToView")]
        public string Link { get; set; }

        public AutoData AutoData { get; set; }

        public StateData StateData { get; set; }
    }

    public class AutoData
    { 
        [JsonProperty("year")]
        public int Year { get; set; }
        
        [JsonProperty("raceInt")]
        public int Mileage { get; set; }

        [JsonProperty("fuelId")]
        public int Fuel { get; set; }

        [JsonProperty("gearBoxId")]
        public int GearBox { get; set; }
    }

    public class StateData
    { 
        [JsonProperty("stateId")]
        public int State { get; set; }

        [JsonProperty("cityId")]
        public int City { get; set; }
    }
}
