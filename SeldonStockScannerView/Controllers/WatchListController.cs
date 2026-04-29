using Microsoft.AspNetCore.Mvc;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;

namespace SeldonStockScannerView.Controllers
{
    public class WatchListController : Controller
    {
        private readonly WatchListService _api = new WatchListService();
        public async Task<ActionResult> Index()
        {
            var watchLists = await _api.GetAll();
            return View(watchLists);
        }

        public async Task<ActionResult> Details(int id)
        {
            var watchList = await _api.Get(id);
            return View(watchList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(WatchList watchList)
        {
            if (!ModelState.IsValid)
                return View(watchList);

            await _api.Create(watchList);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var watchList = await _api.Get(id);
            return View(watchList);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(WatchList watchList)
        {
            if (!ModelState.IsValid)
                return View(watchList);

            await _api.Update(watchList);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var watchList = await _api.Get(id);
            return View(watchList);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _api.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
