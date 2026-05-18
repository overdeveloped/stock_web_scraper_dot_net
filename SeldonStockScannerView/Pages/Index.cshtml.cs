using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SeldonStockScannerView.Models;
using System.Data;

namespace SeldonStockScannerView.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IFinvizScanClient _api_scan;
        private readonly IWatchListClient _api_watchList;

        [BindProperty]
        public string scanType { get; set; }

        public List<FinvizCompanyModel> Companies { get; set; } = new();

        [BindProperty]
        public int SelectedWatchListId { get; set; }
        public List<SelectListItem> WatchLists { get; set; } = new();

        [BindProperty]
        public int RowId { get; set; }

        [BindProperty]
        public int Ticker { get; set; }



        public IndexModel(ILogger<IndexModel> logger, IFinvizScanClient api_scan, IWatchListClient api_watchList)
        {
            _logger = logger;
            _api_scan = api_scan;
            _api_watchList = api_watchList;
        }

        public void OnPostScan()
        {
            Console.WriteLine("BUTTON CLICKED");
            Console.WriteLine(scanType.ToString());
        }

        public async Task OnGetAsync()
        {
            var results = await _api_watchList.GetAllAsync();

            WatchLists = results.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.WatchListName
            }).ToList();

            string thing = "";

        }

        //public void GetShorts()
        //{
        //    Task<HttpResponseMessage> task = _api.GetAllAsync("https://localhost:7059/api/Finviz/shorts");
        //    HttpResponseMessage result = task.Result;
        //    List<FinvizCompany> companies = new List<FinvizCompany>();

        //    if (result.IsSuccessStatusCode)
        //    {
        //        Task<string> readString = result.Content.ReadAsStringAsync();
        //        string jsonString = readString.Result;
        //        companies = FinvizCompany.FromJson(jsonString);
        //    }

        //    ViewData["companies"] = companies;

        //}

        public async Task<IActionResult> OnGetScanAsync(string endpoint)
        {

            Console.WriteLine("HANDLER SELECTION: " + this.scanType);

            Companies = await _api_scan.GetAllAsync(endpoint);

            ViewData["companies"] = Companies;

            return Partial("_ScanResultPartial", Companies);
        }




        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var payload = new
        //    {
        //        ticker = Ticker,
        //        watchListId = SelectedWatchListId
        //    };

        //    var response = await _api_watchList.
        //}










        //public async Task<IActionResult> OnPostWatchListAsync(string id, string value)
        //{
        //    HttpResponseMessage result = await client.post($"https://localhost:7059/api/Finviz/{endpoint}");

        //}

        //public async Task<IActionResult> OnGetWatchListsAsync()
        //{
        //    Console.WriteLine("GET WATCH LISTS");

        //    HttpResponseMessage result = await client.GetAsync($"https://localhost:7059/api/WatchList");

        //}



        // HELPERS
        public static Dictionary<string, object> RowToDictionary(DataRow row)
        {
            return row.Table.Columns
                .Cast<DataColumn>()
                .ToDictionary(col => col.ColumnName, col => row[col]);
        }



    }
}
