using HtmlAgilityPack;

namespace SeldonStockScannerAPI.Connections
{
    public class WebConnection : IWebConnection
    {
        private readonly HtmlWeb htmlWeb;

        public WebConnection(HtmlWeb htmlWeb)
        {
            this.htmlWeb = htmlWeb;
        }

        public HtmlDocument GetWebsiteByUrl(string url)
        {
            return this.htmlWeb.Load(url);
        }
    }
}
