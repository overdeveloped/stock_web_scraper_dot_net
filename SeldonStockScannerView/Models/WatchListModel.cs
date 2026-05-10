using System.ComponentModel.DataAnnotations;

namespace SeldonStockScannerView.Models
{
    public class WatchListModel
    {
        public int Id { get; set; }
        public string WatchListName { get; set; } = string.Empty;
        public List<FinvizCompanyModel> Companies { get; set; } = new();

    }
}
