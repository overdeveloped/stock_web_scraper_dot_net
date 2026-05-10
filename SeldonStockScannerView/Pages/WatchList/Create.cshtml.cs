using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;
using System.Threading.Tasks;

namespace SeldonStockScannerView.Pages.WatchList
{
    public class CreateModel : PageModel
    {
        private readonly IWatchListClient _api;

        public CreateModel(IWatchListClient api)
        {
            _api = api;
        }

        [BindProperty]
        public WatchListModel WatchList { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _api.CreateAsync(WatchList);
            return RedirectToPage("Index");
        }
    }
}