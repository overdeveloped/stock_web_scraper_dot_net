using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;
using System.Threading.Tasks;

public class EditModel : PageModel
{
    private readonly WatchListService _api;

    public EditModel(WatchListService api)
    {
        _api = api;
    }

    [BindProperty]
    public WatchListModel WatchList { get; set; }

    public async Task OnGet(int id)
    {
        WatchList = await _api.Get(id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        await _api.Update(WatchList);
        return RedirectToPage("Index");
    }
}
