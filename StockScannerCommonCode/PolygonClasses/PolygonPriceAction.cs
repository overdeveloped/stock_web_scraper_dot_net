using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockScannerCommonCode.PolygonClasses
{
    public class PolygonPriceAction
    {
        [JsonProperty("ticker")]
        public string symbol;

        [JsonProperty("queryCount")]
        public int queryCount;

        [JsonProperty("resultsCount")]
        public int resultsCount;

        [JsonProperty("adjusted")]
        public bool adjusted;

        [JsonProperty("results")]
        public PolygonCandle[] candles;

        [JsonProperty("status")]
        public string status;

        [JsonProperty("request_id")]
        public string requestId;

        [JsonProperty("count")]
        public int count;



    }
}
