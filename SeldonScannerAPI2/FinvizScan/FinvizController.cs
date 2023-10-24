using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using StockScannerCommonCode;
using StockScannerCommonCode.model;

namespace SeldonStockScannerAPI.FinvizScan
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinvizController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly FinvizFilter _finvizFilter = new FinvizFilter();

        public FinvizController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("dummy")]
        public WatchList GetWatchListDummy()
        {
            WatchList list = new WatchList();

            FinvizCompany company = new FinvizCompany();

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

            List<FinvizCompany> companies = new List<FinvizCompany>()
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
            FinvizFilter filter = new FinvizFilter(filterNames);



            //fillFilters(filter, filterNames);
            //filter.BuildUrl();
            //WebScraper scraper = new WebScraper();

            //List<FinvizCompany> results = scraper.GetCustomWatchList(filter);
            //List<FinvizCompany> filteredTechWatchList = Helpers.filterByPlus500Stocks(results);
            //List<FinvizCompany> filteredTechWatchList = filterByPlus500Stocks(results, plus500Symbols);

            //WatchList list = new WatchList();
            //list.companies = filteredTechWatchList;

            return filter.getFullUrl();
        }




        [HttpGet("megacompanies")]
        public async Task<ActionResult<List<FinvizCompany>>> GetMegaCompanies()
        {
            return _finvizFilter.getMegaCompanies();
        }


        [HttpGet("longholds")]
        public async Task<ActionResult<List<FinvizCompany>>> GetLongHolds()
        {
            return _finvizFilter.getLongHolds();
        }

        [HttpGet("shorts")]
        public async Task<ActionResult<List<FinvizCompany>>> GetShorts()
        {
            return _finvizFilter.getShorts();
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

        private void fillFilters(FinvizFilter filter, Dictionary<string, string> input)
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

        private static List<FinvizCompany> filterByPlus500Stocks(List<FinvizCompany> rawResults, List<Plus500Symbol> plus500symbols)
        {
            DataAccess db = new DataAccess();
            List<FinvizCompany> filteredWatchList = new List<FinvizCompany>();

            foreach (FinvizCompany company in rawResults)
            {
                foreach (Plus500Symbol plus500Company in plus500symbols)
                {
                    if (company.Ticker.Equals(plus500Company.symbol))
                    {
                        filteredWatchList.Add(company.Duplicate());
                    }
                }
            }

            return filteredWatchList;
        }

        private ActionResult<IEnumerable<Plus500Symbol>> getAllPluss500Symbols()
        {
            return dataContext.plus500_symbols.ToList();
        }

    }
}
