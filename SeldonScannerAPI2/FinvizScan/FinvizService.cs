using SeldonStockScannerAPI.FinvizUrlTranslator;
using SeldonStockScannerAPI.models;
using SeldonStockScannerAPI.WebScraper;

namespace SeldonStockScannerAPI.FinvizScan
{
    // TODO: THE FINVIZ UI NOW INCLUDES A $ SIGN. FIX TRANSLATIONS

    public class FinvizService : IFinvizService
    {
        //private readonly AppDbContext _dbContext;
        private readonly IWebScraper webScraper;
        private readonly IFinvizUrlTranslator finvizUrlTranslator;

        private List<string> plus500tickers = new List<string>();

        public FinvizService(IWebScraper webScraper, IFinvizUrlTranslator finvizUrlTranslator)
        {
            this.webScraper = webScraper;
            this.finvizUrlTranslator = finvizUrlTranslator;
        }

        private List<FinvizCompanyEntity> filterByPluss500(List<FinvizCompanyEntity> finvizResults)
        {
            List<FinvizCompanyEntity> result = new List<FinvizCompanyEntity>();

            //using (var context = new DataContext())
            //{
            //    List<Plus500Symbol> symbols = context.plus500Symbols.ToList();

            //    if (symbols.Count > 0 && this.webScraper.GetMegaCompanies().Count() > 0)
            //    {
            //        foreach (Plus500Symbol symbol in symbols)
            //        {
            //            foreach (FinvizCompanyEntity finvizResult in finvizResults)
            //            {
            //                if (symbol.Symbol.Contains('.'))
            //                {
            //                    string[] split = symbol.Symbol.Split('.');
            //                    string firstPart = split[0];
            //                    if (firstPart != null && firstPart.Equals(finvizResult.Ticker))
            //                    {
            //                        result.Add(finvizResult);
            //                    }
            //                }
            //                else if (symbol.Symbol.Equals(finvizResult.Ticker))
            //                {
            //                    result.Add(finvizResult);
            //                }
            //            }
            //        }
            //    }
            //}


            //return result;



            return finvizResults;
        }

        private List<FinvizCompanyEntity> matchWithPlus500()
        {
            List<FinvizCompanyEntity> result = new List<FinvizCompanyEntity>();


            return result;
        }

        public List<string> GetPlus500List()
        {
            this.plus500tickers = this.webScraper.GetCompletePlus500();

            //using (var context = new DataContext())
            //{
            //    foreach (var plus500ticker in plus500tickers)
            //    {
            //        Plus500Symbol symbol = new Plus500Symbol();
            //        symbol.Symbol = plus500ticker;

            //        //context.plus500Symbols.Add(symbol);

            //    }

            //    context.SaveChanges();
            //}


            return this.plus500tickers;
        }

        public List<FinvizCompanyEntity> GetMegaCompanies()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "Mega (200bln and more)" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "longHolds");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetLongHolds()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>()
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "+Micro (over 50mln)" },
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.Beta.ToString(), "Over 1.5" },
                { FinvzEnumFilterType.EpsGrowthNext5Years.ToString(), "Over 10%" },
                { FinvzEnumFilterType.ReturnOnEquity.ToString(), "Over +15%" },
                { FinvzEnumFilterType.CurrentRatio.ToString(), "Over 1.5" }
            };


            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "longHolds");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetOversoldBounce()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.Price.ToString(), "Over 5" },
                { FinvzEnumFilterType.RSI14.ToString(), "Oversold (30)" },
                { FinvzEnumFilterType.Change.ToString(), "Up" },
                { FinvzEnumFilterType.RelativeVolume.ToString(), "Over 2" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "oversoldBounce");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetBreakout()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.MA50.ToString(), "Price above SMA50" },
                { FinvzEnumFilterType.MA200.ToString(), "Price above SMA200" },
                { FinvzEnumFilterType.HighLow50Day.ToString(), "New High" },
                { FinvzEnumFilterType.ReturnOnEquity.ToString(), "Over +20%" },
                { FinvzEnumFilterType.DebtEquity.ToString(), "Under 1" },
                { FinvzEnumFilterType.AverageVolume.ToString(), "Over 100K" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "breakout");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetBreakoutV2()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "+Small (over 300mln)" },
                { FinvzEnumFilterType.RelativeVolume.ToString(), "Over 1" }, // Move this up if too many results
                { FinvzEnumFilterType.CurrentVolume.ToString(), "Over 500K" }, // Move this up if too many results
                { FinvzEnumFilterType.SalesGrowthPast5Years.ToString(), "Positive (>0%)" },
                { FinvzEnumFilterType.MA50.ToString(), "Price above SMA50" },
                { FinvzEnumFilterType.HighLow52Week.ToString(), "0-10% below High" },
                { FinvzEnumFilterType.Volatility.ToString(), "Month - Over 5%" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "breakout_v2");

            return filterByPluss500(finvizResults);
        }

        // Low float vs high volume
        public List<FinvizCompanyEntity> GetBreakoutV3()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "+Small (over 300mln)" },
                { FinvzEnumFilterType.RelativeVolume.ToString(), "Over 1" }, // Move this up if too many results
                { FinvzEnumFilterType.CurrentVolume.ToString(), "Over 500K" }, // Move this up if too many results
                { FinvzEnumFilterType.SalesGrowthPast5Years.ToString(), "Positive (>0%)" },
                { FinvzEnumFilterType.Float.ToString(), "Positive (>0%)" },
                { FinvzEnumFilterType.MA50.ToString(), "Price above SMA50" },
                { FinvzEnumFilterType.HighLow52Week.ToString(), "0-10% below High" },
                { FinvzEnumFilterType.Volatility.ToString(), "Month - Over 5%" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "breakout_v3");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> ForteCapitalDayTrading()
        {
            // From this tutorial:
            // https://www.youtube.com/watch?v=yyW8WDGjdKI
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                //urlArguments.Add(FinvzEnumFilterType.Country.ToString(), "USA");
                { FinvzEnumFilterType.SalesGrowthQuarterOverQuarter.ToString(), "Over 30%" },
                { FinvzEnumFilterType.Industry.ToString(), "Stocks only (ex-Funds)" },
                { FinvzEnumFilterType.AverageVolume.ToString(), "Over 500K" },
                { FinvzEnumFilterType.IPODate.ToString(), "In the last 3 years" }, // Make the most explosive moves but might keep the list too small
                { FinvzEnumFilterType.MA200.ToString(), "Price above SMA200" },
                { FinvzEnumFilterType.MA50.ToString(), "Price above SMA50" },
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.FloatShort.ToString(), "Over 5%" },
                { FinvzEnumFilterType.ChangeFromOpen.ToString(), "Up" }
            };


            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "forte_day_trading_v2");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetShorts()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "+Small (over 300mln)" },
                { FinvzEnumFilterType.FloatShort.ToString(), "High (>20%)" },
                { FinvzEnumFilterType.AverageVolume.ToString(), "Over 500K" },
                { FinvzEnumFilterType.RelativeVolume.ToString(), "Over 1" },
                { FinvzEnumFilterType.CurrentVolume.ToString(), "Over 2M" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "shorts");

            return filterByPluss500(finvizResults);
        }

        public List<FinvizCompanyEntity> GetShorts2()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MarketCap.ToString(), "+Micro (over 50mln)" },
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.Beta.ToString(), "Over 1.5" },
                { FinvzEnumFilterType.EpsGrowthNext5Years.ToString(), "Over 10%" },
                { FinvzEnumFilterType.ReturnOnEquity.ToString(), "Over +15%" },
                { FinvzEnumFilterType.CurrentRatio.ToString(), "Over 1.5" }
            };

            return this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "shorts2");
        }

        public List<FinvizCompanyEntity> GetShortSqueezes()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>();

            // TODO: Add short squeezes

            // $"https://finviz.com/screener.ashx?v=151&f=sh_short_high,ta_highlow50d_b0to10h&ft=3&o=ticker


            //urlArguments.Add(FinvzEnumFilterType.MarketCap.ToString(), "+Micro (over 50mln)");
            //urlArguments.Add(FinvzEnumFilterType.MA20.ToString(), "Price above SMA20");
            //urlArguments.Add(FinvzEnumFilterType.Beta.ToString(), "Over 1.5");
            //urlArguments.Add(FinvzEnumFilterType.EpsGrowthNext5Years.ToString(), "Over 10%");
            //urlArguments.Add(FinvzEnumFilterType.ReturnOnEquity.ToString(), "Over +15%");
            //urlArguments.Add(FinvzEnumFilterType.CurrentRatio.ToString(), "Over 1.5");

            return this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "short_squeeze");
        }


        public List<FinvizCompanyEntity> GetBounceOffMa()
        {
            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.MA50.ToString(), "Price below SMA50" },
                { FinvzEnumFilterType.AverageVolume.ToString(), "Over 400K" },
                { FinvzEnumFilterType.RelativeVolume.ToString(), "Over 1" },
                { FinvzEnumFilterType.CurrentVolume.ToString(), "Over 2M" }
            };

            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "bouncema");

            return filterByPluss500(finvizResults);
        }





        public List<FinvizCompanyEntity> OvernightGapUp()
        {
            // From this tutorial:
            // https://www.youtube.com/watch?v=VDN4jp4Uf1k


            //urlArguments.Add(FinvzEnumFilterType.Country.ToString(), "USA");

            // TODO: Change all these filters to this:
            //Price: 5 - 400
            //Change from close / After hours change?: 2 %
            //Float: 5M - 10M
            //Market Cap: 1B
            //Volume Today / Current Volume ?: 20000

            Dictionary<string, string> urlArguments = new Dictionary<string, string>
            {
                { FinvzEnumFilterType.Price.ToString(), "Over 30%" },
                { FinvzEnumFilterType.Industry.ToString(), "Stocks only (ex-Funds)" },
                { FinvzEnumFilterType.AverageVolume.ToString(), "Over 500K" },
                { FinvzEnumFilterType.IPODate.ToString(), "In the last 3 years" }, // Make the most explosive moves but might keep the list too small
                { FinvzEnumFilterType.MA200.ToString(), "Price above SMA200" },
                { FinvzEnumFilterType.MA50.ToString(), "Price above SMA50" },
                { FinvzEnumFilterType.MA20.ToString(), "Price above SMA20" },
                { FinvzEnumFilterType.FloatShort.ToString(), "Over 5%" },
                { FinvzEnumFilterType.ChangeFromOpen.ToString(), "Up" }
            };


            List<FinvizCompanyEntity> finvizResults = this.webScraper.GetCustomFinvizScan(this.finvizUrlTranslator.BuildUrl(urlArguments), "forte_day_trading_v2");

            return filterByPluss500(finvizResults);
        }



    }
}
