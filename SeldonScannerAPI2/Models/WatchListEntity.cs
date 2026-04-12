using System.ComponentModel.DataAnnotations;

namespace SeldonStockScannerAPI.Models
{
    public class WatchListEntity
    {
        [Key, Required]
        [MaxLength(12)]
        public string Ticker { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Company { get; set; } = string.Empty;
    }
}
