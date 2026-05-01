using System.ComponentModel.DataAnnotations;

namespace SeldonStockScannerView.Models
{
    public class WatchListModel
    {
        public int WatchListId { get; set; }
        public string WatchListName { get; set; } = string.Empty;

    }
}
