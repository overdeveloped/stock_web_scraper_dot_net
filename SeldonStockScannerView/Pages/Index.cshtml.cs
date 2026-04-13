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

            switch (endpoint)
            {
                case "0":
                    endpoint = "GetMegaCompanies";
                    break;
                case "1":
                    endpoint = "GetLongHolds";
                    break;
                case "2":
                    endpoint = "GetOversoldBounce";
                    break;
                case "3":
                    endpoint = "GetBreakout";
                    break;
                case "4":
                    endpoint = "GetBreakoutV2";
                    break;
                case "5":
                    endpoint = "GetBreakoutV3";
                    break;
                case "6":
                    endpoint = "ForteCapitalDayTrading";
                    break;
                case "7":
                    endpoint = "GetShorts";
                    break;
                case "8":
                    endpoint = "GetShorts2";
                    break;
                case "9":
                    endpoint = "GetShortSqueezes";
                    break;
                case "10":
                    endpoint = "GetBounceOffMa";
                    break;
                case "11":
                    endpoint = "GetTech";
                    break;
            }


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
