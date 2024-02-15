using SeldonStockScannerAPI.models;

namespace SeldonStockScannerAPI.FinvizScan
{
    public interface IFinvizService
    {
        List<string> GetPlus500List() { return new List<string>(); }
        List<FinvizCompanyEntity> GetMegaCompanies() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetLongHolds() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetOversoldBounce() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetBreakout() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetBreakoutV2() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetBreakoutV3() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> ForteCapitalDayTrading() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetShorts() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetShortSqueezes() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetBounceOffMa() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetShorts2() { return new List<FinvizCompanyEntity>(); }
        List<FinvizCompanyEntity> GetTech() { return new List<FinvizCompanyEntity>(); }

    }
}
