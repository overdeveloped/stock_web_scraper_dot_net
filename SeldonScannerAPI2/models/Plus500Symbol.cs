//using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace SeldonStockScannerAPI.models
{
    // The property names reflect the column titles on the Finviz results page
    public class Plus500Symbol
    {
        [Key, Required]
        [MaxLength(12)]
        public string Symbol { get; set; } = string.Empty;

    }
}
