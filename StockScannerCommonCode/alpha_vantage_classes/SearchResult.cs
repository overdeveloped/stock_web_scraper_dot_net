using Newtonsoft.Json;

namespace StockScannerCommonCode
{
    public class SearchResult
    {
        [JsonProperty("1. symbol")]
        public string Symbol;

        [JsonProperty("2. name")]
        public string Name;

        [JsonProperty("3. type")]
        public string Type;

        [JsonProperty("4. region")]
        public string Region;

        [JsonProperty("5. marketOpen")]
        public string MarketOpen;

        [JsonProperty("6. marketClose")]
        public string MarketClose;

        [JsonProperty("7. timezone")]
        public string TimeZone;

        [JsonProperty("8. currency")]
        public string Currency;

        [JsonProperty("9. matchScore")]
        public string MatchScore;

        //public override string ToString()
        //{
        //    return $"OPEN: {this.Open}, CLOSE: {this.Close}, HIGH: {this.High}, LOW: {this.Close}";
        //}

        //public string FormatDataForBulkUpload()
        //{
        //    return $"{this.Open},{this.Close},{this.High},{this.Close}";
        //}

    }
}
