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

        [HttpPost]
        public void CreateWatchlist(WatchListEntity watchItem)
        {
            //this._watchListService.AddWatchItem(watchItem);

            throw new NotImplementedException();
        }


        [HttpGet]
        public ActionResult GetAllWatchLists()
        {
            var products = _watchListService.GetAll()
                .Select(p => new WatchListEntity
                {
                    WatchListId = p.WatchListId,
                    WatchListName = p.WatchListName
                })
                .ToList();

            return View(products);
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


        [HttpPut]
        public void AddWatch(WatchListEntity watchItem)
        {
            //this._watchListService.AddWatchItem(watchItem);

            throw new NotImplementedException();
        }




    }
}
