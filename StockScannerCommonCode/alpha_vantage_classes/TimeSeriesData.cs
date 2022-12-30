using Newtonsoft.Json;

namespace StockScannerCommonCode
{
    public class TimeSeriesData
    {
        [JsonProperty("1. open")]
        public string Open;

        [JsonProperty("2. high")]
        public string High;

        [JsonProperty("3. low")]
        public string Low;

        [JsonProperty("4. close")]
        public string Close;

        [JsonProperty("5. volume")]
        public string Volume;

        public override string ToString()
        {
            return $"OPEN: {this.Open}, CLOSE: {this.Close}, HIGH: {this.High}, LOW: {this.Close}";
        }

        public string FormatDataForBulkUpload()
        {
            return $"{this.Open},{this.Close},{this.High},{this.Close}";
        }

    }
}
