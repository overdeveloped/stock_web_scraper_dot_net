using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.models;

namespace SeldonStockScannerAPI.FinvizScan
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinvizController : ControllerBase
    {
        //private readonly DataContext dataContext;
        private readonly IFinvizService finvizService;
        //private readonly FinvizService _finvizFilter = new FinvizService();

        public FinvizController(IFinvizService finvizService)
        {
            this.finvizService = finvizService;
        }

        [HttpGet("dummy")]
        public WatchList GetWatchListDummy()
        {
            WatchList list = new WatchList();

            FinvizCompanyEntity company = new FinvizCompanyEntity();

            company.Ticker = "ticker";
            company.Company = "company";
            company.Sector = "sector";
            company.Industry = "industry";
            company.Country = "country";
            company.MarketCap = "marketCap";
            company.PE = "pe";
            company.Price = "price";
            company.Change = "change";
            company.Volume = "volume";

            List<FinvizCompanyEntity> companies = new List<FinvizCompanyEntity>()
            {
                company
            };

            list.companies = companies;
            return list;
        }


        /// <summary>
        /// </summary>
        /// <param name="filterNames"></param>
        /// <returns></returns>

        [HttpPut("scan")]
        public async Task<ActionResult<string>> GetWatchList(Dictionary<string, string> filterNames)
        {
            //List<Plus500Symbol> plus500Symbols = await dataContext.plus500_symbols.ToListAsync();
            FinvizService filter = new FinvizService(filterNames);



            //fillFilters(filter, filterNames);
            //filter.BuildUrl();
            //WebScraper scraper = new WebScraper();

            //List<FinvizCompany> results = scraper.GetCustomWatchList(filter);
            //List<FinvizCompany> filteredTechWatchList = Helpers.filterByPlus500Stocks(results);
            //List<FinvizCompany> filteredTechWatchList = filterByPlus500Stocks(results, plus500Symbols);

            //WatchList list = new WatchList();
            //list.companies = filteredTechWatchList;

            return filter.GetFullUrl();
        }


        [HttpGet("plus500list")]
        public async Task<ActionResult<List<string>>> GetPlus500List()
        {
            return finvizService.GetPlus500List();
        }


        [HttpGet("megacompanies")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetMegaCompanies()
        {
            return finvizService.GetMegaCompanies();
        }


        [HttpGet("longholds")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetLongHolds()
        {
            return finvizService.GetLongHolds();
        }

        [HttpGet("oversoldbounce")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetOversoldBounce()
        {
            return finvizService.GetOversoldBounce();
        }

        [HttpGet("breakout")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetBreakout()
        {
            return finvizService.GetBreakout();
        }

        [HttpGet("breakoutv2")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetBreakoutV2()
        {
            return finvizService.GetBreakoutV2();
        }

        [HttpGet("fortedaytrading")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetForteDayTrading()
        {
            return finvizService.ForteCapitalDayTrading();
        }

        [HttpGet("shorts")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetShorts()
        {
            return finvizService.GetShorts();
        }

        [HttpGet("bouncema")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetBounceOffMa()
        {
            return finvizService.GetBounceOffMa();
        }







        [HttpGet("tech")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetTech()
        {
            return finvizService.GetTech();
        }

        [HttpGet("fortedaytrading")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetForteDayTrading()
        {
            return _finvizFilter.ForteCapitalDayTrading();
        }

        [HttpGet("overnightgapup")]
        public async Task<ActionResult<List<FinvizCompanyEntity>>> GetOvernightGapUp()
        {
            return _finvizFilter.ForteCapitalDayTrading();
        }



        // Example input:

        //{
        //  "Exchange": "",
        //  "MarketCap": "Mega ($200bln and more)",
        //  "EarningsDate": "",
        //  "TargetPrice": "",

        //  "Index": "",
        //  "DividendYield": "Over 1%",
        //  "AverageVolume": "",
        //  "IPODate": "",

        //  "Sector": "Sector",
        //  "FloatShort": "FloatShort",
        //  "RelativeVolume": "RelativeVolume",
        //  "SharesOutstanding": "SharesOutstanding",

        //  "Industry": "Industry",
        //  "AnalystRecom": "AnalystRecom",
        //  "CurrentVolume": "CurrentVolume",
        //  "Float": "Float",

        //  "Country": "Country",
        //  "OptionShort": "OptionShort",
        //  "Price": "Price"
        //}

        // https://finviz.com/screener.ashx

        private void fillFilters(FinvizService filter, Dictionary<string, string> input)
        {
            // Column 1
            //filter.Exchange = input["Exchange"];
            //filter.MarketCap = input["MarketCap"];
            //filter.EarningsDate = input["EarningsDate"];
            //filter.TargetPrice = input["TargetPrice"];
            //// Column 2
            //filter.Index = input["Index"];
            //filter.DividendYield = input["DividendYield"];
            //filter.AverageVolume = input["AverageVolume"];
            //filter.IPODate = input["IPODate"];
            //// Column 3
            //filter.Sector = input["Sector"];
            //filter.FloatShort = input["FloatShort"];
            //filter.RelativeVolume = input["RelativeVolume"];
            //filter.SharesOutstanding = input["SharesOutstanding"];
            //// Column 4
            //filter.Industry = input["Industry"];
            //filter.AnalystRecom = input["AnalystRecom"];
            //filter.CurrentVolume = input["CurrentVolume"];
            //filter.Float = input["Float"];
            //// Column 5
            //filter.Country = input["Country"];
            //filter.OptionShort = input["OptionShort"];
            //filter.Price = input["Price"];
        }

        //private static List<FinvizCompanyEntity> filterByPlus500Stocks(List<FinvizCompanyEntity> rawResults, List<Plus500Symbol> plus500symbols)
        //{
        //    DataAccess db = new DataAccess();
        //    List<FinvizCompanyEntity> filteredWatchList = new List<FinvizCompanyEntity>();

        //    foreach (FinvizCompanyEntity company in rawResults)
        //    {
        //        foreach (Plus500Symbol plus500Company in plus500symbols)
        //        {
        //            if (company.Ticker.Equals(plus500Company.Symbol))
        //            {
        //                filteredWatchList.Add(company.Duplicate());
        //            }
        //        }
        //    }

        //    return filteredWatchList;
        //}

        //private ActionResult<List<Plus500Symbol>> getAllPluss500Symbols()
        //{
        //    GetCompletePlus500();
        //    return dataContext.plus500_symbols.ToList();
        //}

    }
}
