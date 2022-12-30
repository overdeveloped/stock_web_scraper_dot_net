
using StockScannerCommonCode.Strategies;

namespace StockScannerCommonCode.model
{
    public class WatchListItem
    {
        public int id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public EnumWatchType type { get; set; }
    }
}
