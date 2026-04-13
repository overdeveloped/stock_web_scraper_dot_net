using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.Models;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.FinvizScan
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        //private readonly DataContext dataContext;
        private readonly IWatchListService _watchListService;
        //private readonly FinvizService _finvizFilter = new FinvizService();

        public WatchListController(IWatchListService WatchListService)
        {
            this._watchListService = WatchListService;
        }

        [HttpGet]
        public List<WatchListEntity> GetWatchList()
        {
            return this._watchListService.GetWatchList();




            //WatchListEntity company = new WatchListEntity();

            //company.Ticker = "ticker";
            //company.Company = "company";

            //List<WatchListEntity> companies = new List<WatchListEntity>()
            //{
            //    company
            //};

            //return companies;
        }

        [HttpPut]
        public void AddWatch(WatchListEntity watchItem)
        {
            this._watchListService.AddWatchItem(watchItem);
        }




    }
}
