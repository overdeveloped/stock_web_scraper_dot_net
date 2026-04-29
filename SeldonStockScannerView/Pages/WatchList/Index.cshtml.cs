using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WatchList
{
    public class IndexModel : PageModel
    {
        private readonly WatchListService _api;

        public IndexModel(WatchListService api)
        {
            _api = api;
        }

        public List<WatchList> WatchLists { get; set; }

        public async Task OnGet()
        {
            WatchLists = await _api.GetAll();
        }
    }

}