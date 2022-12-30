using StockScannerCommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode
{
    public class AlphaVantageAPI : IHistoricPriceActionData
    {
        private const string API_KEY = "XYOSAC07KZ38BYQL";

        // Interface functions
        public string getPriceActionHistory(string symbol, EnumTimeScale timeScale, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }





        public static string getFiveMinuteBySymbolCompact(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&outputsize=compact&apikey={API_KEY}";
            string result = "";

            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);
                Helpers.outputToFile($"alpha_vantage_5min_compact_{symbol}", result);
            }

            return result;
        }

        public static string getFiveMinuteBySymbolFull(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=5min&outputsize=full&apikey={API_KEY}";
            string result = "";

            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);
                Helpers.outputToFile($"alpha_vantage_5min_full_{symbol}", result);
            }

            return result;
        }

        public static string getHourlyBySymbol(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=60min&outputsize=full&apikey={API_KEY}";
            string result = "";

            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);
                Helpers.outputToFile($"alpha_vantage_60min_full_{symbol}", result);
            }

            return result;
        }

        public static string getDailyBySymbolCompact(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&datatype=json&apikey={API_KEY}";
            string result = "";
            Uri queryUri = new Uri(QUERY_URL);
            //MessageBox.Show(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);


                Helpers.outputToFile($"alpha_vantage_daily_compact_{symbol}", result);
            }

            return result;
        }

        public static string getDailyBySymbolFull(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&datatype=json&apikey={API_KEY}";
            string result = "";
            Uri queryUri = new Uri(QUERY_URL);
            //MessageBox.Show(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);


                Helpers.outputToFile($"alpha_vantage_daily_full_{symbol}", result);
            }

            return result;
        }

        public static string search(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords={symbol}&apikey={API_KEY}";
            string result = "";
            Uri queryUri = new Uri(QUERY_URL);
            //MessageBox.Show(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);


                Helpers.outputToFile("alpha_vantage_symbol_call", result);
            }

            return result;

        }

        // THIS REQUIRES PREMIUM
        public static string getDailyMacdBySymbolCompact(string symbol)
        {
            string QUERY_URL = $"https://www.alphavantage.co/query?function=MACD&series_type=close&symbol={symbol}&outputsize=full&datatype=json&apikey={API_KEY}";
            string result = "";
            Uri queryUri = new Uri(QUERY_URL);
            //MessageBox.Show(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                // -------------------------------------------------------------------------
                // if using .NET Framework (System.Web.Script.Serialization)

                result = client.DownloadString(queryUri);


                Helpers.outputToFile("alpha_vantage_daily_compact", result);
            }

            return result;
        }

    }
}
