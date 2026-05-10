using SeldonStockScannerAPI.WatchList;
using System.ComponentModel.DataAnnotations;

namespace SeldonStockScannerAPI.Finviz_Company
{
    public class FinvizCompanyDTO
    {
        public int Id { get; set; }

        public string Ticker { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string? Sector { get; set; }

        public string? Industry { get; set; }

        public string? Country { get; set; }

        public string? MarketCap { get; set; }

        public string? PE { get; set; }

        public string? Price { get; set; }

        public string? Change { get; set; }

        public string? Volume { get; set; }

        public List<WatchListDTO> Watchlists { get; set; } = new();

    }
}
