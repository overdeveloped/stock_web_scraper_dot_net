using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeldonStockScannerView.Models;

namespace SeldonStockScannerView.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        static readonly HttpClient client = new HttpClient();

        [BindProperty]
        public string scanType { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPostScan()
        {
            Console.WriteLine("BUTTON CLICKED");
            Console.WriteLine(scanType.ToString());
        }

        public void OnGet()
        {
            string thing = "";
        }

        public void GetShorts()
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

        public async Task<IActionResult> OnGetScanAsync(string endpoint)
        {
            Console.WriteLine("HANDLER SELECTION: " + this.scanType);

            HttpResponseMessage result = await client.GetAsync($"https://localhost:7059/api/Finviz/{endpoint}");
            List<FinvizCompany> companies = new List<FinvizCompany>();

            if (result.IsSuccessStatusCode)
            {
                Task<string> readString = result.Content.ReadAsStringAsync();
                string jsonString = readString.Result;
                companies = FinvizCompany.FromJson(jsonString);
            }

            ViewData["companies"] = companies;

            return Partial("_ScanResultPartial", companies);
        }

    }
}
