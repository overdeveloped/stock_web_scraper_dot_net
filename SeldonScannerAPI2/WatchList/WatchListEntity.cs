using SeldonStockScannerAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeldonStockScannerAPI.WatchList
{
    public class WatchListEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WatchListId { get; set; }

        [Required]
        [MaxLength(50)]
        public string WatchListName { get; set; } = string.Empty;

        public ICollection<FinvizCompanyEntity> Companies { get; set; }
    }
}
