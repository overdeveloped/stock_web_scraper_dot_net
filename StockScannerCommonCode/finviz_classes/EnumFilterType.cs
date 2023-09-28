using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode.finviz_classes
{
    public enum EnumFilterType
    {
        Exchange = 0,
        MarketCap = 1,
        PB = 2,
        EpsGrowthPast5Years = 3,
        DividendYield = 4,
        QuickRatio = 5,
        NetProfitMargin = 6,
        InstitutionalTransactions = 7,
        Performance = 8,
        MA20 = 9,
        HighLow20Day = 10,
        Beta = 11,
        Price = 12,
        AfterHoursClose = 13,

        Index,
        PE,
        PriceCash,
        EpsGrowthNext5Years,
        ReturnOnAssets,
        LTDebtEquity,
        PayoutRatio,
        FloatShort,
        Performance2,
        MA50,
        HighLow50Day,
        AverageTrueRange,
        TargetPrice,
        AfterHoursChange,

        Sector,
        ForwardPE,
        PriceFreeCashFlow,
        SalesGrowthPast5Years,
        ReturnOnEquity,
        DebtEquity,
        InsiderOwnership,
        AnalystRecom,
        Volatility,
        MA200,
        HighLow52Week,
        AverageVolume,
        IPODate,

        Industry,
        PEG,
        EPSGrowthThisYear,
        EPSGrowthQuarterOverQuarter,
        ReturnOnInvestment,
        GrossMargin,
        InsiderTransactions,
        OptionShort,
        RSI14,
        Change,
        Pattern,
        RelativeVolume,
        SharesOutstanding,

        Country,
        PS,
        EPSGrowthNextYear,
        SalesGrowthQuarterOverQuarter,
        CurrentRatio,
        OperatingMargin,
        InstitutionalOwnership,
        EarningsDate,
        Gap,
        ChangeFromOpen,
        Candlestick,
        CurrentVolume,
        Float

    }
}
