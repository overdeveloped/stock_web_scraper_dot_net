using Newtonsoft.Json;

namespace StockScannerCommonCode.PolygonClasses
{
    public class PolygonCandle
    {
        [JsonProperty("v")]
        public long volume;

        [JsonProperty("vw")]
        public double volumeWeightedAverage;

        [JsonProperty("o")]
        public double open;

        [JsonProperty("h")]
        public double high;

        [JsonProperty("l")]
        public double low;

        [JsonProperty("c")]
        public double close;

        [JsonProperty("t")]
        public long time;

        [JsonProperty("n")]
        public int numberOfTransactions;

        public override string ToString()
        {
            return $"OPEN: {this.open}, CLOSE: {this.close}, HIGH: {this.high}, LOW: {this.low}";
        }

        public string FormatDataForBulkUpload()
        {
            return $"{this.open},{this.close},{this.high},{this.low}";
        }

    }
}
