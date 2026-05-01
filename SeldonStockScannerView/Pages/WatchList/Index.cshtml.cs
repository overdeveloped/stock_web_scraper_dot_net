using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeldonStockScannerView.Pages.WatchList
{
    public class IndexModel : PageModel
    {
        private readonly WatchListService _api;
        public List<WatchListModel> WatchLists { get; set; }

        public IndexModel(WatchListService api)
        {
            _api = api;
        }


        public async Task OnGetAsync()
        {
            WatchLists = await _api.GetAllAsync();
        }
    }
}