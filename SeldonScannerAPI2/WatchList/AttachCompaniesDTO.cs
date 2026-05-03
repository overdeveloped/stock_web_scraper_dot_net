namespace SeldonStockScannerAPI.WatchList
{
    public class AttachCompaniesDTO
    {
        public string WatchListName { get; set; } = string.Empty;

        // List of company IDs to attach
        public List<int> CompanyIds { get; set; } = new();
    }
}
