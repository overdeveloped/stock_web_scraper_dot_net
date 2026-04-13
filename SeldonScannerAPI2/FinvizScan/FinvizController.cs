using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.Models;

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
        public List<WatchListEntity> GetWatchListDummy()
        {
            WatchListEntity company = new WatchListEntity();

            company.Ticker = "ticker";
            company.Company = "company";

            List<WatchListEntity> companies = new List<WatchListEntity>()
            {
                company
            };

            return companies;
        }

        [HttpGet("scan")]
        public ActionResult<List<string>> GetScan()
        {
            return finvizService.GetPlus500List();
        }



        // For filtering by what's available on the Plus500 platform
        [HttpGet("plus500list")]
        public ActionResult<List<string>> GetPlus500List()
        {
            return finvizService.GetPlus500List();
        }

        // Basic Scans
        [HttpGet("megacompanies")]
        public ActionResult<List<FinvizCompanyEntity>> GetMegaCompanies()
        {
            return finvizService.GetMegaCompanies();
        }

        [HttpGet("tech")]
        public ActionResult<List<FinvizCompanyEntity>> GetTech()
        {
            return finvizService.GetTech();
        }

        // Other
        [HttpGet("longholds")]
        public ActionResult<List<FinvizCompanyEntity>> GetLongHolds()
        {
            return finvizService.GetLongHolds();
        }

        [HttpGet("oversoldbounce")]
        public ActionResult<List<FinvizCompanyEntity>> GetOversoldBounce()
        {
            return finvizService.GetOversoldBounce();
        }

        [HttpGet("breakout")]
        public ActionResult<List<FinvizCompanyEntity>> GetBreakout()
        {
            return finvizService.GetBreakout();
        }

        [HttpGet("breakoutv2")]
        public ActionResult<List<FinvizCompanyEntity>> GetBreakoutV2()
        {
            return finvizService.GetBreakoutV2();
        }

        [HttpGet("fortedaytrading")]
        public ActionResult<List<FinvizCompanyEntity>> GetForteDayTrading()
        {
            return finvizService.ForteCapitalDayTrading();
        }

        [HttpGet("shorts")]
        public ActionResult<List<FinvizCompanyEntity>> GetShorts()
        {
            return finvizService.GetShorts();
        }

        [HttpGet("shortsqueeze")]
        public ActionResult<List<FinvizCompanyEntity>> GetShortSqueeze()
        {
            // TODO: NEEDS SHORT SQUEEZE IN SERVICE
            return finvizService.GetShorts();
        }

        [HttpGet("bouncema")]
        public ActionResult<List<FinvizCompanyEntity>> GetBounceOffMa()
        {
            return finvizService.GetBounceOffMa();
        }

        [HttpGet("overnightgapup")]
        public ActionResult<List<FinvizCompanyEntity>> GetOvernightGapUp()
        {
            return finvizService.GetBounceOffMa();
        }

    }
}
