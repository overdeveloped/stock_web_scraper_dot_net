using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScannerCommonCode.model;

namespace StockScannerCommonCode.Strategies
{
    interface IStrategy
    {
        List<TradeSignal> RunStrategy();
    }
}
