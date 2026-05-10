using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;
using SeldonStockScannerView.Services;
using System.Threading.Tasks;

public class DeleteModel : PageModel
{
    private readonly IWatchListClient _api;

    public DeleteModel(IWatchListClient api)
    {
        _api = api;
    }

    [BindProperty]
    public WatchListModel WatchList { get; set; }

    public async Task OnGet(int id)
    {
        WatchList = await _api.GetByIdAsync(id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        await _api.DeleteAsync(WatchList.Id);
        return RedirectToPage("Index");
    }
}
