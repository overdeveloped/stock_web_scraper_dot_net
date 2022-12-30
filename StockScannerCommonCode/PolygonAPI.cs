using StockScannerCommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode
{
    // https://polygon.io/dashboard
    public class PolygonAPI : IHistoricPriceActionData
    {
        private const string API_KEY = "u0aX8p7DIjVrvDwNWhkILlBxpcHUVMCd";

        // Interface functions
        public string getPriceActionHistory(string symbol, EnumTimeScale timeScale, DateTime start, DateTime end)
        {
            string scale = "";
            string timespan = "";

            switch (timeScale)
            {
                case EnumTimeScale.FiveMinute:
                    scale = "5";
                    timespan = "minute";
                    break;
                case EnumTimeScale.FifteenMinute:
                    scale = "15";
                    timespan = "minute";
                    break;
                case EnumTimeScale.ThirtyMinute:
                    scale = "3";
                    timespan = "minute";
                    break;
                case EnumTimeScale.Hourly:
                    scale = "1";
                    timespan = "minute";
                    break;
                case EnumTimeScale.Daily:
                    scale = "1";
                    timespan = "minute";
                    break;
                default:
                    return "Timescale not recognised";
            }

            string QUERY_URL = $"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/{scale}/{timespan}/{start.ToString("yyyy-MM-dd")}/{end.ToString("yyyy-MM-dd")}?adjusted=true&sort=asc&limit=50000&apiKey=u0aX8p7DIjVrvDwNWhkILlBxpcHUVMCd";
            string result = "";
            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                try
                {
                    result = client.DownloadString(queryUri);
                    //Helpers.outputToFile($"polygon_{timeScale.ToString()}_{start.ToString("yyyy-MM-dd")}_to_{start.ToString("yyyy-MM-dd")}_{symbol}", result);
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Problem getting data from Polygon: " + ex.Message.ToString());
                }
            }

            return result;
        }







    }
}
