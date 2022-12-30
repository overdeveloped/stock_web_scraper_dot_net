using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode
{
    public interface IHistoricPriceActionData
    {
        // Fetches price action history between dates for given symbol and time scale.
        // Returns JSON string results
        // NOTE: If the Alpha Vantage service is used the time range will be ignored and the history will start from now and go 
        // back an arbitrary amount
        string getPriceActionHistory(string symbol, EnumTimeScale timeScale, DateTime start, DateTime end);
    }
}
