using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.Models;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.FinvizScan
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchListController : Controller
    {
        //private readonly DataContext dataContext;
        private readonly IWatchListService _watchListService;
        //private readonly FinvizService _finvizFilter = new FinvizService();

        public WatchListController(IWatchListService WatchListService)
        {
            this._watchListService = WatchListService;
        }

        //[HttpGet]
        //public ActionResult GetAllWatchLists()
        //{
        //    var watchLists = _watchListService.GetAll();

        //    return PartialView("_WatchListSelectBox", watchLists);

        //}

        //[HttpPost]
        //public void CreateWatchlist(WatchListEntity watchItem)
        //{
        //    //this._watchListService.AddWatchItem(watchItem);

        //    throw new NotImplementedException();
        //}


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _watchListService.GetAll();
            return Ok(items);
        }



        //[HttpGet]
        //public WatchListEntity GetWatchList(int id)
        //{
        //    var result = this._watchListService.GetByIdAsync(id);

        //    if (result.IsCompletedSuccessfully)
        //    {
        //        return result.Result;
        //    }

        //    return null;

        //    //WatchListEntity company = new WatchListEntity();

        //    //company.Ticker = "ticker";
        //    //company.Company = "company";

        //    //List<WatchListEntity> companies = new List<WatchListEntity>()
        //    //{
        //    //    company
        //    //};

        //    //return companies;
        //}


        //[HttpPut]
        //public void AddWatch(WatchListEntity watchItem)
        //{
        //    //this._watchListService.AddWatchItem(watchItem);

        //    throw new NotImplementedException();
        //}




    }
}
