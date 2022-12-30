using Dapper;
using StockScannerCommonCode.charting;
using StockScannerCommonCode.model;
using StockScannerCommonCode.PolygonClasses;
using StockScannerCommonCode.Strategies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using TimeZoneConverter;

namespace StockScannerCommonCode
{
    public class DataAccess
    {
        // SCANNER
        public void InsertFilterSave(FinvizFilterSave save)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<Candle> results = new List<Candle>();
                try
                {
                    connection.Execute($"INSERT INTO [dbo].[scanner_filter_save] VALUES(" +
                        $"'{save.GetSaveName()}'," +

                        $"'{save.GetExchange()}'," +
                        $"'{save.GetMarketCap()}'," +
                        $"'{save.GetEarningsDate()}'," +
                        $"'{save.GetTargetPrice()}'," +

                        $"'{save.GetIndex()}'," +
                        $"'{save.GetDividendYield()}'," +
                        $"'{save.GetAverageVolume()}'," +
                        $"'{save.GetIPODate()}'," +

                        $"'{save.GetSecter()}'," +
                        $"'{save.GetFloatShare()}'," +
                        $"'{save.GetRelativeVolume()}'," +
                        $"'{save.GetSharesOutstanding()}'," +

                        $"'{save.GetIndustry()}'," +
                        $"'{save.GetAnalystRecommends()}'," +
                        $"'{save.GetCurrentValue()}'," +
                        $"'{save.GetFloat()}'," +

                        $"'{save.GetCountry()}'," +
                        $"'{save.GetOptionShort()}'," +
                        $"'{save.GetPrice()}');");

                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting filter save: {ex.Message}");
                }
            }

        }

        public List<FinvizFilterSave> LoadFilterSaves()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<FinvizFilterSave> results = new List<FinvizFilterSave>();
                try
                {
                    results = connection.Query<FinvizFilterSave>($"SELECT * FROM [dbo].[scanner_filter_save]").ToList();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem fetching filter saves: {ex.Message}");
                }

                return results;
            }
        }

        public FinvizFilterSave LoadFilterSave(string saveName)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                FinvizFilterSave filterSave = new FinvizFilterSave();
                try
                {
                    filterSave = connection.Query<FinvizFilterSave>($"SELECT * FROM [dbo].[scanner_filter_save] WHERE saveName = '{saveName}';").ToList().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem fetching filter saves: {ex.Message}");
                }

                return filterSave;
            }
        }


        public List<Candle> GetPrice(int id)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<Candle> results = new List<Candle>();
                try
                {
                    results = connection.Query<Candle>("dbo.PriceAction_GetById @Id", new { Id = id }).ToList();

                }
                catch (Exception)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, "There was a problem getting price_action");
                }

                return results;
            }
        }
        public List<Plus500Symbol> GetPlus500SymbolList()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<Plus500Symbol> results = connection.Query<Plus500Symbol>("SELECT * FROM plus500_symbols;").ToList();
                return results;
            }
        }

        public void InsertCompany(string symbol, string name)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                Company company = new Company { symbol = symbol, name = name };
                List<Company> companies = new List<Company>();
                companies.Add(company);

                try
                {
                    connection.Execute("dbo.Company_Insert @Symbol, @Name", companies);
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting a company: {ex.Message}");
                }
            }
        }

        public void InsertPlus500SymbolList(List<string> symbolList)
        {
            List<List<string>> partitionedData = PartitionList(symbolList, 1000);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Query("DELETE FROM plus500_symbols");
                    StringBuilder query = new StringBuilder();

                    foreach (List<string> partition in partitionedData)
                    {
                        query.AppendLine("INSERT INTO [dbo].[plus500_symbols] VALUES ");
                        bool firstLoop = true;
                        foreach (string symbol in partition)
                        {
                            if (!firstLoop)
                            {
                                query.Append(",");
                            }

                            //sb.Append($"{thing.Key},{thing.Value.FormatDataForBulkUpload()};");
                            query.AppendLine($"('{symbol}')");

                            firstLoop = false;
                        }

                        query.Append(";");
                        Helpers.outputToFile("query", query.ToString());

                        try
                        {
                            connection.Query(query.ToString());
                        }
                        catch (System.Exception ex)
                        {
                            Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting Plus500 symbols: {ex.Message}");
                        }

                        query.Clear();
                    }

                    // TODO: WHY DOES THIS QUERY GET RUN AGAIN? / WHY ARE THERE SO MANY TRY BLOCKS
                    query.Append(";");
                    Helpers.outputToFile("query", query.ToString());

                    try
                    {
                        connection.Query(query.ToString());
                    }
                    catch (System.Exception ex)
                    {
                        Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting Plus500 symbols: {ex.Message}");
                    }

                    query.Clear();
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting Plus500 symbols: {ex.Message}");
                }
            }
        }

        public void BulkInsertPriceAction(MetaData metaData, Dictionary<DateTime, TimeSeriesData> data)
        {
            // https://stackoverflow.com/questions/9240288/how-to-insert-a-multiple-rows-in-sql-using-stored-procedures

            // Check for above 1000 rows and split into smaller chunks. SQL only inserts 1000 at a time
            List<Dictionary<DateTime, TimeSeriesData>> partitionedData = PartitionDictionary(data, 1000);
            DateTime mostRecent = checkMostRecentInsertion(metaData.Symbol);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                StringBuilder query = new StringBuilder();

                foreach (Dictionary<DateTime, TimeSeriesData> partition in partitionedData)
                {
                    query.AppendLine("INSERT INTO [dbo].[price_action] VALUES ");
                    bool firstLoop = true;
                    foreach (KeyValuePair<DateTime, TimeSeriesData> priceRow in partition)
                    {
                        int recent = priceRow.Key.CompareTo(mostRecent);
                        if (priceRow.Key.CompareTo(mostRecent) > 0)
                        {
                            if (!firstLoop)
                            {
                                query.Append(",");
                            }

                            //sb.Append($"{thing.Key},{thing.Value.FormatDataForBulkUpload()};");
                            query.AppendLine($"('{priceRow.Key.ToString("yyyy-MM-dd HH:mm:ss")}','{priceRow.Value.Open}','{priceRow.Value.Close}','{priceRow.Value.High}','{priceRow.Value.Low}','{metaData.Symbol}')");

                            firstLoop = false;
                        }
                    }

                    query.Append(";");
                    Helpers.outputToFile("query", query.ToString());

                    try
                    {
                        connection.Query(query.ToString());
                    }
                    catch (System.Exception ex)
                    {
                        Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem bulk inserting price action: {ex.Message}");
                    }

                    query.Clear();
                }
            }
        }

        public void ClearPriceActionTable()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Execute("DELETE FROM price_action");
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem clearing the price_action table: {ex.Message}");
                }
            }
        }

        // WITH POLYGON DATA:
        public void InsertCandle(Candle candle)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                bool thereIsDataToCommit = true;
                StringBuilder query_priceAction = new StringBuilder();

                query_priceAction.AppendLine("INSERT INTO [dbo].[price_action] VALUES ");

                double Open = candle.price_open;
                double Close = candle.price_close;
                double High = candle.price_high;
                double Low = candle.price_low;
                string Symbol = candle.company_id;

                DateTime theTime = candle.date_time;

                query_priceAction.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Open}','{Close}','{High}','{Low}','{Symbol}')");

                query_priceAction.Append(";");
                //Helpers.outputToFile("query", query_priceAction.ToString());

                if (thereIsDataToCommit)
                {
                    try
                    {
                        //Helpers.outputToFile("db_price_action_query", query_priceAction.ToString());
                        connection.Query(query_priceAction.ToString());
                    }
                    catch (Exception ex)
                    {
                        Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Database query problem: {ex.Message}");
                    }
                }

                query_priceAction.Clear();
            }
        }

        public void InsertCandleMACD(CandleMACD candleMACD)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                bool thereIsDataToCommit = true;
                StringBuilder query_priceAction = new StringBuilder();
                StringBuilder query_macd = new StringBuilder();
                StringBuilder query_sma200 = new StringBuilder();

                query_priceAction.AppendLine("INSERT INTO [dbo].[price_action] VALUES ");
                query_macd.AppendLine("INSERT INTO [dbo].[indicator_macd] VALUES ");
                query_sma200.AppendLine("INSERT INTO [dbo].[indicator_sma200] VALUES ");

                double Open = candleMACD.price_open;
                double Close = candleMACD.price_close;
                double High = candleMACD.price_high;
                double Low = candleMACD.price_low;

                double macd = candleMACD.macd;
                double signal = candleMACD.signal;
                double hist = candleMACD.hist;

                double sma200 = candleMACD.sma200;

                DateTime theTime = candleMACD.date_time;
                string Symbol = candleMACD.company_id;

                query_priceAction.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Open}','{Close}','{High}','{Low}','{Symbol}')");
                query_macd.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{macd}','{signal}','{hist}')");
                query_sma200.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{sma200}')");

                query_priceAction.Append(";");
                query_macd.Append(";");
                query_sma200.Append(";");
                //Helpers.outputToFile("query", query_priceAction.ToString());

                if (thereIsDataToCommit)
                {
                    try
                    {
                        //Helpers.outputToFile("db_price_action_query", query_priceAction.ToString());
                        connection.Query(query_priceAction.ToString());
                        connection.Query(query_macd.ToString());
                        connection.Query(query_sma200.ToString());
                    }
                    catch (Exception ex)
                    {
                        Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Database query problem: {ex.Message}");
                    }
                }
            }
        }

        public void InsertPriceActionAndIndicators(PolygonPriceAction data)
        {
            PolygonCandle[] candles = data.candles;

            // Check the database for the most recent record
            DateTime mostRecent = checkMostRecentInsertion(data.symbol);
            // Remove dates that are already in the database
            if (mostRecent != null)
            {
                candles = filterOutExistingDates(candles, mostRecent);
            }

            List<List<PolygonCandle>> candlesPartitioned = PartitionPolygonCandles(candles, 500);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                foreach (List<PolygonCandle> partition in candlesPartitioned)
                {
                    bool thereIsDataToCommit = false;
                    StringBuilder query_priceAction = new StringBuilder();
                    StringBuilder query_macd = new StringBuilder();
                    StringBuilder query_sma200 = new StringBuilder();
                    // MACD
                    RollingAverage ema9 = new RollingAverage(9, true);
                    RollingAverage ema12 = new RollingAverage(12, true);
                    RollingAverage ema26 = new RollingAverage(26, true);
                    RollingAverage signal = new RollingAverage(9, true);
                    // TREND
                    RollingAverage sma200 = new RollingAverage(200, false);

                    query_priceAction.AppendLine("INSERT INTO [dbo].[price_action] VALUES ");
                    query_macd.AppendLine("INSERT INTO [dbo].[indicator_macd] VALUES ");
                    query_sma200.AppendLine("INSERT INTO [dbo].[indicator_sma200] VALUES ");
                    bool firstLoop = true;

                    foreach (PolygonCandle candle in partition)
                    {
                        double Open = candle.open;
                        double Close = candle.close;
                        double High = candle.high;
                        double Low = candle.low;
                        long Volume = candle.volume;
                        string Symbol = data.symbol;

                        // Time Zone strings:
                        // https://himasagar.com/list-of-timezone-ids-for-use-with-findtimezonebyid-in-charp
                        // Conversion code:
                        // https://stackoverflow.com/questions/16834169/change-the-timezone-of-datetimeoffset
                        // More:
                        // https://www.c-sharpcorner.com/article/c-sharp-datetime-conversion-with-specific-timezone/

                        DateTime UtcTime = DateTimeOffset.FromUnixTimeMilliseconds(candle.time).UtcDateTime;
                        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        DateTime theTime = TimeZoneInfo.ConvertTimeFromUtc(UtcTime, timeZoneInfo);

                        // TREND
                        sma200.advance(Convert.ToDouble(Close));
                        // MACD
                        ema9.advance(Convert.ToDouble(Close));
                        ema12.advance(Convert.ToDouble(Close));
                        ema26.advance(Convert.ToDouble(Close));
                        double MACD = ema12.getAverage() - ema26.getAverage();
                        signal.advance(MACD);
                        double Hist = MACD - signal.getAverage();

                        if (!firstLoop)
                        {
                            query_priceAction.Append(",");
                            query_macd.Append(",");
                            query_sma200.Append(",");
                        }

                        query_priceAction.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Open}','{Close}','{High}','{Low}','{Volume}','{Symbol}')");
                        query_macd.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{MACD}','{signal.getAverage()}','{Hist}')");
                        query_sma200.AppendLine($"('{theTime.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{sma200.getAverage()}')");

                        firstLoop = false;
                        thereIsDataToCommit = true;
                    }

                    query_priceAction.Append(";");
                    query_macd.Append(";");
                    query_sma200.Append(";");
                    //Helpers.outputToFile("query", query_priceAction.ToString());

                    if (thereIsDataToCommit)
                    {
                        try
                        {
                            //Helpers.outputToFile("db_price_action_query", query_priceAction.ToString());
                            //Helpers.outputToFile("db_macd_query", query_macd.ToString());
                            connection.Query(query_priceAction.ToString());
                            connection.Query(query_macd.ToString());
                            connection.Query(query_sma200.ToString());
                        }
                        catch (Exception ex)
                        {
                            Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Database query problem: {ex.Message}");
                        }
                    }

                    query_priceAction.Clear();
                }
            }
        }

        public void UpsertPriceActionAlphaVantage(MetaData metaData, Dictionary<DateTime, TimeSeriesData> data)
        {
            // https://stackoverflow.com/questions/9240288/how-to-insert-a-multiple-rows-in-sql-using-stored-procedures

            // Check the database for the most recent record
            DateTime mostRecent = checkMostRecentInsertion(metaData.Symbol);
            if (mostRecent != null)
            {
                data = filterOutExistingDates(data, mostRecent);
            }

            // Check for above 1000 rows and split into smaller chunks. SQL only inserts 1000 at a time
            List<Dictionary<DateTime, TimeSeriesData>> partitionedData = PartitionDictionary(data, 1000);

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                bool thereIsDataToCommit = false;
                // TODO: INDICATOR CALCS DOESN'T WORK WITH PARTITIONING
                foreach (Dictionary<DateTime, TimeSeriesData> partition in partitionedData)
                {
                    StringBuilder query_priceAction = new StringBuilder();
                    StringBuilder query_macd = new StringBuilder();
                    StringBuilder query_sma200 = new StringBuilder();
                    // MACD
                    RollingAverage ema9 = new RollingAverage(9, true);
                    RollingAverage ema12 = new RollingAverage(12, true);
                    RollingAverage ema26 = new RollingAverage(26, true);
                    RollingAverage signal = new RollingAverage(9, true);
                    // TREND
                    RollingAverage sma200 = new RollingAverage(200, false);

                    query_priceAction.AppendLine("INSERT INTO [dbo].[price_action] VALUES ");
                    query_macd.AppendLine("INSERT INTO [dbo].[indicator_macd] VALUES ");
                    query_sma200.AppendLine("INSERT INTO [dbo].[indicator_sma200] VALUES ");
                    bool firstLoop = true;

                    foreach (KeyValuePair<DateTime, TimeSeriesData> priceRow in partition)
                    {
                        string Open = priceRow.Value.Open;
                        string Close = priceRow.Value.Close;
                        string High = priceRow.Value.High;
                        string Low = priceRow.Value.Low;
                        string Volume = priceRow.Value.Volume;
                        string Symbol = metaData.Symbol;

                        DateTime theTime = priceRow.Key;
                        // TREND
                        sma200.advance(Convert.ToDouble(Close));
                        // MACD
                        ema9.advance(Convert.ToDouble(Close));
                        ema12.advance(Convert.ToDouble(Close));
                        ema26.advance(Convert.ToDouble(Close));
                        double MACD = ema12.getAverage() - ema26.getAverage();
                        signal.advance(MACD);
                        double Hist = MACD - signal.getAverage();

                        if (!firstLoop)
                        {
                            query_priceAction.Append(",");
                            query_macd.Append(",");
                            query_sma200.Append(",");
                        }

                        query_priceAction.AppendLine($"('{priceRow.Key.ToString("yyyy-MM-dd HH:mm:ss")}','{Open}','{Close}','{High}','{Low}','{Symbol}')");
                        query_macd.AppendLine($"('{priceRow.Key.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{MACD}','{signal.getAverage()}','{Hist}')");
                        query_sma200.AppendLine($"('{priceRow.Key.ToString("yyyy-MM-dd HH:mm:ss")}','{Symbol}','{sma200.getAverage()}')");

                        firstLoop = false;
                        thereIsDataToCommit = true;
                    }

                    query_priceAction.Append(";");
                    query_macd.Append(";");
                    query_sma200.Append(";");
                    //Helpers.outputToFile("query", query_priceAction.ToString());

                    if (thereIsDataToCommit)
                    {
                        try
                        {
                            //Helpers.outputToFile("db_price_action_query", query_priceAction.ToString());
                            //Helpers.outputToFile("db_macd_query", query_macd.ToString());
                            connection.Query(query_priceAction.ToString());
                            connection.Query(query_macd.ToString());
                            connection.Query(query_sma200.ToString());
                        }
                        catch (Exception ex)
                        {
                            Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting price action and symbols: {ex.Message}");
                        }
                    }

                    query_priceAction.Clear();
                }
            }
        }


        // INDICATORS
        public void AddMovingAveToPriceAction(string symbol)
        {
            // Add MA to model? Use Hibernate?
        }

        // GET SIMULATION PRICE ACTION BETWEEN DATES
        public List<Candle> GetPriceActionBetweenDates(DateTime startDate, DateTime endDate)
        {
            List<Candle> results = new List<Candle>();
            string query = "SELECT * FROM price_action "
                            + "WHERE company_id = 'IBM' "
                            + $"AND date_time > '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}' "
                            + $"AND date_time < '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}' "
                            + "ORDER BY date_time DESC";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                //connection.Query(query);
                results = connection.Query<Candle>(query).ToList();
            }

            return results;
        }

        // GET SIMULATION PRICE ACTION FOR COMPANY
        public List<Candle> GetPriceAction(string symbol)
        {
            List<Candle> results = new List<Candle>();
            string query = "SELECT * FROM price_action "
                            + $"WHERE company_id = '{symbol}' "
                            + "ORDER BY date_time";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    results = connection.Query<Candle>(query).ToList();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Problem fetching Price Action with MACD data: {ex.Message}");
                }
            }

            return results;
        }

        // GET BACKTEST DATA
        public List<CandleMACD> GetMacdData(string symbol)
        {
            List<CandleMACD> results = new List<CandleMACD>();
            string query = "SELECT price.*, macd.macd, macd.signal, macd.hist, sma.sma200 FROM price_action price "
                            + "JOIN indicator_macd macd ON macd.the_time = price.date_time AND macd.symbol = price.company_id "
                            + "JOIN indicator_sma200 sma ON sma.the_time = price.date_time AND sma.symbol = price.company_id "
                            + $"WHERE price.company_id = '{symbol}' "
                            + "ORDER BY date_time";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    results = connection.Query<CandleMACD>(query).ToList();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Problem fetching Price Action with MACD data: {ex.Message}");
                }
            }

            return results;
        }

        public List<CandleMACD> GetMacdDataByDate(string symbol, DateTime start, DateTime end)
        {
            List<CandleMACD> results = new List<CandleMACD>();
            string query = "SELECT price.*, macd.macd, macd.signal, macd.hist, sma.sma200 FROM price_action price " +
                            "JOIN indicator_macd macd ON macd.the_time = price.date_time AND macd.symbol = price.company_id " +
                            "JOIN indicator_sma200 sma ON sma.the_time = price.date_time AND sma.symbol = price.company_id " +
                            $"WHERE price.company_id = '{symbol}' " +
                            $"AND " +
                            $"( " +
                            $"	price.date_time >= CONVERT(DATETIME,'2022-05-10', 110) " +
                            $"	AND " +
                            $"	price.date_time <= CONVERT(DATETIME,'2022-05-11', 110) " +
                            $") "
                            + "ORDER BY date_time";

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    results = connection.Query<CandleMACD>(query).ToList();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Problem fetching Price Action with MACD data: {ex.Message}");
                }
            }

            return results;
        }

        // TRADE SIGNALS
        public void ClearSignalTable()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Execute("DELETE FROM trade_signal");
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem clearing the signal table: {ex.Message}");
                }
            }
        }

        // DELETE PRICE ACTIONS, INDICATORS
        public void ClearPriceAndIndicatorTables()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Execute("DELETE FROM price_action");
                    connection.Execute("DELETE FROM indicator_macd");
                    connection.Execute("DELETE FROM indicator_sma200");
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem clearing price and indicator tables: {ex.Message}");
                }
            }
        }


        public void InsertTradeSignal(TradeSignal tradeSignal)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Execute($"INSERT INTO [dbo].[trade_signal] VALUES ('{tradeSignal.getOpenDate().ToString("yyyy-MM-dd HH:mm:ss")}'," +
                        $"'{tradeSignal.getSymbol()}'," +
                        $"{tradeSignal.getEntry()}," +
                        $"{tradeSignal.getStopLoss()}," +
                        $"{tradeSignal.getProfitLevel()}," +
                        $"'{tradeSignal.getSignalType()}');");
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting trade signal: {ex.Message}");
                }
            }
        }

        public void InsertTradeSignals(List<TradeSignal> tradeSignals)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                foreach (var tradeSignal in tradeSignals)
                {
                    try
                    {
                        connection.Execute($"INSERT INTO [dbo].[trade_signal] VALUES ('{tradeSignal.getOpenDate().ToString("yyyy-MM-dd HH:mm:ss")}'," +
                            $"'{tradeSignal.getSymbol()}'," +
                            $"{tradeSignal.getEntry()}," +
                            $"{tradeSignal.getStopLoss()}," +
                            $"{tradeSignal.getProfitLevel()}," +
                            $"'{tradeSignal.getSignalType()}');");
                    }
                    catch (System.Exception ex)
                    {
                        Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting trade signals: {ex.Message}");
                    }
                }
            }
        }

        public List<model.TradeSignal> GetTradeSignals()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<model.TradeSignal> results = new List<model.TradeSignal>();
                try
                {
                    results = connection.Query<TradeSignal>("SELECT * FROM trade_signal;").ToList();
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem fetching trade signals: {ex.Message}");
                }

                return results;
            }
        }

        // WATCH LIST
        public void ClearWatchListTable()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    connection.Execute("DELETE FROM watch_list");
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem clearing the watch table: {ex.Message}");
                }
            }
        }

        public void InsertWatchList(EnumWatchType type, List<FinvizCompany> companies)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                StringBuilder query = new StringBuilder();

                query.AppendLine("INSERT INTO [dbo].[watch_list] VALUES ");
                bool firstLoop = true;
                foreach (FinvizCompany company in companies)
                {
                    if (!firstLoop)
                    {
                        query.Append(",");
                    }

                    //sb.Append($"{thing.Key},{thing.Value.FormatDataForBulkUpload()};");
                    query.AppendLine($"('{company.Ticker}', '{company.Company}', '{type.ToString()}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                    firstLoop = false;
                }

                query.Append(";");
                //Helpers.outputToFile("query", query.ToString());
                //Helpers.log($"query: {query.ToString()}");
                try
                {
                    connection.Query(query.ToString());
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting watch list: {ex.Message}");
                }

                query.Clear();
            }
        }

        public void InsertWatchListFTSE100(EnumWatchType type, Dictionary<string, string> companies)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                StringBuilder query = new StringBuilder();

                query.AppendLine("INSERT INTO [dbo].[watch_list] VALUES ");
                bool firstLoop = true;
                foreach (var company in companies)
                {
                    if (!firstLoop)
                    {
                        query.Append(",");
                    }

                    query.AppendLine($"('{company.Key}', '{company.Value}', '{type.ToString()}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')");

                    firstLoop = false;
                }

                query.Append(";");
                Helpers.outputToFile("ftse100_insert_query", query.ToString());

                try
                {
                    connection.Query(query.ToString());
                }
                catch (System.Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem inserting watch list: {ex.Message}");
                }

                query.Clear();
            }
        }

        public List<WatchListItem> GetWatchList()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                List<WatchListItem> results = new List<WatchListItem>();
                try
                {
                    results = connection.Query<WatchListItem>("SELECT * FROM watch_list;").ToList();
                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"There was a problem fetching watch list: {ex.Message}");
                }

                return results;
            }
        }



        // HELPERS

        // Splits a larger dictionary into a list of smaller dictionaries

        private DateTime checkMostRecentInsertion(string symbol)
        {
            DateTime lastDate = new DateTime();
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helpers.connectionValue("seldonscanner")))
            {
                try
                {
                    lastDate = connection.Query<DateTime>($"SELECT TOP 1 date_time FROM price_action WHERE company_id = '{symbol}' ORDER BY date_time DESC;").ToList().FirstOrDefault();

                }
                catch (Exception ex)
                {
                    Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, "Error getting data in checkMostRecentInsertion(): " + ex.Message.ToString());
                }
            }

            return lastDate;
        }

        public List<List<PolygonCandle>> PartitionPolygonCandles(PolygonCandle[] source, int limit)
        {
            List<List<PolygonCandle>> results = new List<List<PolygonCandle>>();
            List<PolygonCandle> partition = new List<PolygonCandle>();
            var counter = 0;
            var total = 0;

            foreach (var company in source)
            {
                if (counter == limit)
                {
                    results.Add(partition);
                    partition = new List<PolygonCandle>();
                    counter = 0;
                }

                partition.Add(company);
                counter++;
                total++;

                // For the last partition that will probably be smaller than the limit:
                if (total == source.Count())
                {
                    results.Add(partition);
                }
            }

            return results;
        }

        public List<Dictionary<DateTime, TimeSeriesData>> PartitionDictionary(Dictionary<DateTime, TimeSeriesData> source, int size)
        {
            List<Dictionary<DateTime, TimeSeriesData>> results = new List<Dictionary<DateTime, TimeSeriesData>>();
            var partition = new Dictionary<DateTime, TimeSeriesData>(size);
            var counter = 0;

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    partition.Add(enumerator.Current.Key, enumerator.Current.Value);
                    counter++;
                    if (counter % size == 0)
                    {
                        results.Add(partition.ToDictionary(original => original.Key,
                                               original => original.Value));
                        partition.Clear();
                        counter = 0;
                    }
                }

                if (counter != 0)
                    results.Add(partition.ToDictionary(original => original.Key,
                                               original => original.Value));
            }

            return results;
        }

        // Returns the provided dictionary minus the rows before the provided date
        public Dictionary<DateTime, TimeSeriesData> filterOutExistingDates(Dictionary<DateTime, TimeSeriesData> source, DateTime mostRecent)
        {
            var filtered = new Dictionary<DateTime, TimeSeriesData>();

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Key > mostRecent)
                    {
                        filtered.Add(enumerator.Current.Key, enumerator.Current.Value);
                    }
                }
            }

            return filtered;
        }

        public PolygonCandle[] filterOutExistingDates(PolygonCandle[] source, DateTime mostRecent)
        {
            var filtered = new List<PolygonCandle>();

            var dateWithOffset = new DateTimeOffset(mostRecent).ToUniversalTime();
            long timestamp = dateWithOffset.ToUnixTimeMilliseconds();

            foreach (PolygonCandle candle in source)
            {
                if (candle.time > timestamp)
                {
                    filtered.Add(candle);
                }
            }

            return filtered.ToArray();
        }

        public List<List<string>> PartitionList(List<string> source, int size)
        {
            List<List<string>> results = new List<List<string>>();
            var partition = new List<string>(size);
            var counter = 0;

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    partition.Add(enumerator.Current);
                    counter++;
                    if (counter % size == 0)
                    {
                        results.Add(partition.ToList());
                        partition.Clear();
                        counter = 0;
                    }
                }

                if (counter != 0)
                    results.Add(partition.ToList());
            }

            return results;
        }
    }
}

