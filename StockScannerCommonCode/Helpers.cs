using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScannerCommonCode.model;
using StockScannerCommonCode.PolygonClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StockScannerCommonCode
{
    public static class Helpers
    {
        private const string dataPath = @"C:\Users\exper\output\data\";
        private const string errorPath = @"C:\Users\exper\logs\errors\";
        private const string logPath = @"C:\Users\exper\logs\";
        public static string connectionValue(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        #region -JSON PROCESSING-
        // https://stackoverflow.com/questions/66243883/parsing-json-data-with-a-period-from-alpha-vantage-vb-net
        //public static Dictionary<DateTime, TimeSeriesData> deserialiseJSON(string json)
        //{
        //    try
        //    {
        //        JObject obj = JObject.Parse(json);
        //        MetaData meta = JsonConvert.DeserializeObject<MetaData>(obj.First.First.ToString());
        //        Dictionary<DateTime, TimeSeriesData> dailyTimeSeries = JsonConvert.DeserializeObject<Dictionary<DateTime, TimeSeriesData>>(obj.Last.First.ToString());
        //        Dictionary<DateTime, TimeSeriesData> dailyTimeSeriesReversed = new Dictionary<DateTime, TimeSeriesData>();

        //        for (int index = dailyTimeSeries.Count - 1; index >= 0; index--)
        //        {
        //            dailyTimeSeriesReversed.Add(dailyTimeSeries.ElementAt(index).Key, dailyTimeSeries.ElementAt(index).Value);
        //        }

        //        if (dailyTimeSeries.Count() != dailyTimeSeriesReversed.Count())
        //        {
        //            throw new Exception("These should be equal");
        //        }

        //        DataAccess db = new DataAccess();
        //        db.UpdatePriceAction(meta, dailyTimeSeries);

        //        //var jPerson = JsonConvert.DeserializeObject<dynamic>(json);
        //        debugOutput("Here's our JSON object: " + Environment.NewLine);


        //        return dailyTimeSeriesReversed;
        //    }
        //    catch (Exception ex)
        //    {
        //        debugOutput("Error deserialising JSON: " + ex.Message.ToString());
        //    }

        //    return null;
        //}

        public static Tuple<MetaData, Dictionary<DateTime, TimeSeriesData>> deserialiseAlphaVantageJSON(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                MetaData meta = JsonConvert.DeserializeObject<MetaData>(obj.First.First.ToString());
                Dictionary<DateTime, TimeSeriesData> timeSeries = JsonConvert.DeserializeObject<Dictionary<DateTime, TimeSeriesData>>(obj.Last.First.ToString());
                Dictionary<DateTime, TimeSeriesData> dailyTimeSeriesReversed = new Dictionary<DateTime, TimeSeriesData>();

                for (int index = timeSeries.Count - 1; index >= 0; index--)
                {
                    KeyValuePair<DateTime, TimeSeriesData> candle = timeSeries.ElementAt(index);
                    //if (candle.Key.TimeOfDay > marketOpen && candle.Key.TimeOfDay < marketClose)
                    //{

                    dailyTimeSeriesReversed.Add(timeSeries.ElementAt(index).Key, timeSeries.ElementAt(index).Value);
                }
                return new Tuple<MetaData, Dictionary<DateTime, TimeSeriesData>>(meta, dailyTimeSeriesReversed);
            }
            catch (Exception ex)
            {
                error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Error deserialising JSON into Tuple: " + ex.Message.ToString());
            }

            return null;
        }

        public static PolygonPriceAction deserialisePolygonJSON(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);

                //var meta = (JArray)obj["results"];
                PolygonPriceAction priceAction = JsonConvert.DeserializeObject<PolygonPriceAction>(obj.ToString());


                //MetaData meta = JsonConvert.DeserializeObject<MetaData>(obj.First.First.ToString());
                //Dictionary<DateTime, TimeSeriesData> timeSeries = JsonConvert.DeserializeObject<Dictionary<DateTime, TimeSeriesData>>(obj.Last.First.ToString());
                //Dictionary<DateTime, TimeSeriesData> dailyTimeSeriesReversed = new Dictionary<DateTime, TimeSeriesData>();

                //for (int index = timeSeries.Count - 1; index >= 0; index--)
                //{
                //    KeyValuePair<DateTime, TimeSeriesData> candle = timeSeries.ElementAt(index);
                //    //if (candle.Key.TimeOfDay > marketOpen && candle.Key.TimeOfDay < marketClose)
                //    //{

                //    dailyTimeSeriesReversed.Add(timeSeries.ElementAt(index).Key, timeSeries.ElementAt(index).Value);
                //}

                return priceAction;
            }
            catch (Exception ex)
            {
                error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Error deserialising JSON into Tuple: " + ex.Message.ToString());
            }

            return null;
        }

        public static DataTable deserialiseJSONtoDataTable(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                MetaData meta = JsonConvert.DeserializeObject<MetaData>(obj.First.First.ToString());
                Dictionary<DateTime, TimeSeriesData> timeSeries = JsonConvert.DeserializeObject<Dictionary<DateTime, TimeSeriesData>>(obj.Last.First.ToString());
                Dictionary<DateTime, TimeSeriesData> dailyTimeSeriesReversed = new Dictionary<DateTime, TimeSeriesData>();
                DataTable dataTableMarket = new DataTable();
                dataTableMarket.Columns.Add("TheTime", typeof(DateTime));
                dataTableMarket.Columns.Add("High", typeof(double));
                dataTableMarket.Columns.Add("Low", typeof(double));
                dataTableMarket.Columns.Add("Open", typeof(double));
                dataTableMarket.Columns.Add("Close", typeof(double));
                dataTableMarket.Columns.Add("Volume", typeof(int));
                dataTableMarket.Columns.Add("Symbol", typeof(string));
                dataTableMarket.Columns.Add("Entry", typeof(double));
                dataTableMarket.Columns.Add("StopLoss", typeof(double));
                dataTableMarket.Columns.Add("Profit", typeof(double));
                dataTableMarket.Columns.Add("MACD", typeof(double));
                dataTableMarket.Columns.Add("Signal", typeof(double));
                dataTableMarket.Columns.Add("HIST", typeof(double));
                dataTableMarket.Columns.Add("Trend", typeof(double));

                TimeSpan marketOpen = new TimeSpan(9, 29, 0);
                TimeSpan marketClose = new TimeSpan(16, 0, 0);

                for (int index = timeSeries.Count - 1; index >= 0; index--)
                {
                    KeyValuePair<DateTime, TimeSeriesData> candle = timeSeries.ElementAt(index);
                    //if (candle.Key.TimeOfDay > marketOpen && candle.Key.TimeOfDay < marketClose)
                    //{

                    DataRow dr = dataTableMarket.NewRow();

                    // Convert to double
                    double High = Convert.ToDouble(candle.Value.High);
                    double Low = Convert.ToDouble(candle.Value.Low);
                    double Open = Convert.ToDouble(candle.Value.Open);
                    double Close = Convert.ToDouble(candle.Value.Close);
                    int Volume = Convert.ToInt32(candle.Value.Volume);
                    dr["TheTime"] = candle.Key;
                    dr["High"] = High;
                    dr["Low"] = Low;
                    dr["Open"] = Open;
                    dr["Close"] = Close;
                    dr["Volume"] = Volume;
                    dr["Symbol"] = meta.Symbol;
                    dataTableMarket.Rows.Add(dr);
                    //}
                }

                //if (dailyTimeSeries.Count() != dataTableMarket.Rows.Count)
                //{
                //    throw new Exception("These should be equal");
                //}

                //DataAccess db = new DataAccess();
                //db.UpdatePriceAction(meta, dailyTimeSeries);

                return dataTableMarket;
            }
            catch (Exception ex)
            {
                error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Error deserialising JSON: " + ex.Message.ToString());
            }

            return null;
        }

        #endregion
        #region -FILE IO-
        public static void outputToFile(string fileName, string output)
        {
            try
            {
                // Empty target file
                File.WriteAllText($@"{dataPath}{fileName}.txt", "");
                // Replace contents
                File.AppendAllText($@"{dataPath}{fileName}.txt", output);

            }
            catch (Exception ex)
            {
                error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Error outputting to file: " + ex.Message.ToString());
            }
        }

        public static string getFromFile(string fileName)
        {
            string text;
            using (var streamReader = new StreamReader($@"{dataPath}{fileName}.txt", Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            return text;
        }
        #endregion

        public static void error(string source, string text)
        {
            try
            {
                File.AppendAllText($@"{errorPath}{DateTime.Now.ToString("yyyy-MM-dd")}_{source}.txt", $"\r\n{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")} - ERROR - {text}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message.ToString() + Environment.NewLine);
            }
        }
        public static void log(string text)
        {
            try
            {
                File.AppendAllText($@"{logPath}{DateTime.Now.ToString("yyyy-MM-dd")}.txt", $"\r\n{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")} - {text}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message.ToString() + Environment.NewLine);
            }
        }

        public static List<FinvizCompany> filterByPlus500Stocks(List<FinvizCompany> rawResults)
        {
            DataAccess db = new DataAccess();
            List<FinvizCompany> filteredWatchList = new List<FinvizCompany>();
            List<Plus500Symbol> plus500symbols = db.GetPlus500SymbolList();
            
            foreach (FinvizCompany company in rawResults)
            {
                foreach (Plus500Symbol plus500Company in plus500symbols)
                {
                    if (company.Ticker.Equals(plus500Company.Symbol))
                    {
                        filteredWatchList.Add(company.Duplicate());
                    }
                }
            }

            return filteredWatchList;
        }
    }
}
