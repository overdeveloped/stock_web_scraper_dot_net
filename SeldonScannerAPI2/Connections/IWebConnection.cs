using HtmlAgilityPack;

namespace SeldonStockScannerAPI.Connections
{
    public interface IWebConnection
    {
        HtmlDocument GetWebsiteByUrl(string url);

    }
}
