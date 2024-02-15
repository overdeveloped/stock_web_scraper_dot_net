namespace SeldonStockScannerAPI.FinvizUrlTranslator
{
    public interface IFinvizUrlTranslator
    {
        string BuildUrl(Dictionary<string, string> filterNames);
    }
}
