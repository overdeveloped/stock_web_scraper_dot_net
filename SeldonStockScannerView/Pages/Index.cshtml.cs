using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;

namespace SeldonStockScannerView.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        static readonly HttpClient client = new HttpClient();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Task<HttpResponseMessage> task = client.GetAsync("https://localhost:7059/api/Finviz/shorts");
            HttpResponseMessage result = task.Result;
            List<FinvizCompany> companies = new List<FinvizCompany>();

            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string jsonString = readString.Result;
                companies = FinvizCompany.FromJson(jsonString);
            }

            ViewData["companies"] = companies;

        }
    }
}
