using SeldonStockScannerAPI.Finviz_Company;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeldonStockScannerAPI.WatchList
{
    public class WatchListEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string WatchListName { get; set; } = string.Empty;

        public ICollection<FinvizCompanyEntity> Companies { get; set; } = new List<FinvizCompanyEntity>();
    }
}
