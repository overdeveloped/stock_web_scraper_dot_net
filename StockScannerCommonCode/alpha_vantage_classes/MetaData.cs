using System;
using Newtonsoft.Json;

namespace StockScannerCommonCode
{
    public class MetaData
    {
        [JsonProperty("1. Information")]
        public string Information;

        [JsonProperty("2. Symbol")]
        public string Symbol;

        [JsonProperty("3. Last Refreshed")]
        public string LastRefreshed;

        [JsonProperty("4. Output Size")]
        public string OutputSize;

        [JsonProperty("5. Time Zone")]
        public string TimeZone;

        public override string ToString()
        {
            return $"{this.Symbol}{Environment.NewLine}" +
            $"{this.LastRefreshed}{Environment.NewLine}" +
            $"{this.OutputSize}{Environment.NewLine}" +
            $"{this.TimeZone}";
        }
    }
}
