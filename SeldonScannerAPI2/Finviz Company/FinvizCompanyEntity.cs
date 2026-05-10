using SeldonStockScannerAPI.WatchList;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeldonStockScannerAPI.Finviz_Company
{
    // The property names reflect the column titles on the Finviz results page
    public class FinvizCompanyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Ticker { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Company { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Sector { get; set; }

        [MaxLength(50)]
        public string? Industry { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(50)]
        public string? MarketCap { get; set; }

        [MaxLength(50)]
        public string? PE { get; set; }

        [MaxLength(50)]
        public string? Price { get; set; }

        [MaxLength(50)]
        public string? Change { get; set; }

        [MaxLength(50)]
        public string? Volume { get; set; }

        public ICollection<WatchListEntity> Watchlists { get; set; } = new List<WatchListEntity>();

    }
}
