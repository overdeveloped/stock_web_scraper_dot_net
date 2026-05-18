using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.Finviz_Company
{
    public class FinvizCompanyService : IFinvizCompanyService
    {
        private readonly ApplicationDbContext _context;

        public FinvizCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FinvizCompanyDTO>> GetAllAsync()
        {
            var entities = await _context.FinvizCompany
                .Include(c => c.Watchlists)
                .ToListAsync();

            var entityDTOs = new List<FinvizCompanyDTO>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    entityDTOs.Add(new FinvizCompanyDTO()
                    {
                        Id = entity.Id,
                        Ticker = entity.Ticker,
                        Company = entity.Company,
                        Sector = entity.Sector,
                        Industry = entity.Industry,
                        Country = entity.Country,
                        MarketCap = entity.MarketCap,
                        PE = entity.PE,
                        Price = entity.Price,
                        Change = entity.Change,
                        Volume = entity.Volume,
                        Watchlists = entity.Watchlists
                    .Select(w => new WatchListDTO { Id = w.Id, WatchListName = w.WatchListName })
                    .ToList()
                    });
                }
            }
            else
            {
                return null;
            }

            return entityDTOs;
        }

        public async Task<FinvizCompanyDTO?> GetByIdAsync(int id)
        {
            var entity = await _context.FinvizCompany
                .AsNoTracking()
                .Include(c => c.Watchlists)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
                return null;

            return new FinvizCompanyDTO()
            {
                Id = entity.Id,
                Ticker = entity.Ticker,
                Company = entity.Company,
                Sector = entity.Sector,
                Industry = entity.Industry,
                Country = entity.Country,
                MarketCap = entity.MarketCap,
                PE = entity.PE,
                Price = entity.Price,
                Change = entity.Change,
                Volume = entity.Volume,
                Watchlists = entity.Watchlists
                    .Select(w => new WatchListDTO { Id = w.Id, WatchListName = w.WatchListName })
                    .ToList()
            };
        }

        public async Task<FinvizCompanyDTO?> GetByTickerAsync(string ticker)
        {
            var entity = await _context.FinvizCompany
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Ticker == ticker);

            if (entity == null)
                return null;

            return new FinvizCompanyDTO()
            {
                Id = entity.Id,
                Ticker = entity.Ticker,
                Company = entity.Company,
                Sector = entity.Sector,
                Industry = entity.Industry,
                Country = entity.Country,
                MarketCap = entity.MarketCap,
                PE = entity.PE,
                Price = entity.Price,
                Change = entity.Change,
                Volume = entity.Volume,
            };

        }

        //public async Task<FinvizCompanyEntity> CheckOrAdd(string ticker)
        //{
        //    // CHECK IF ALREADY EXISTS
        //    var comp = await _context.FinvizCompany.FirstOrDefaultAsync(c => c.Ticker == ticker);

        //    if (comp == null)
        //    {
        //        comp = new FinvizCompanyEntity()
        //    }



        //}

        public async Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity company)
        {
            _context.FinvizCompany.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<FinvizCompanyEntity> CreateCompanyAsync(FinvizCompanyEntity company, List<int> watchListIds)
        {
            var watchlists = await _context.WatchList
                .Where(w => watchListIds.Contains(w.Id))
                .ToListAsync();

            company.Watchlists = watchlists;

            _context.FinvizCompany.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<FinvizCompanyEntity?> UpdateAsync(string ticker, FinvizCompanyEntity company)
        {
            var existing = await _context.FinvizCompany
                .Include(c => c.Watchlists)
                .FirstOrDefaultAsync(c => c.Ticker == ticker);

            if (existing == null)
            {
                await CreateAsync(company);
                return company;
            }

            existing.Ticker = company.Ticker;
            existing.Company = company.Company;
            existing.Sector = company.Sector;
            existing.Industry = company.Industry;
            existing.Country = company.Country;
            existing.MarketCap = company.MarketCap;
            existing.PE = company.PE;
            existing.Price = company.Price;
            existing.Change = company.Change;
            existing.Volume = company.Volume;

            // Update watchlists
            if (company.Watchlists != null && company.Watchlists.Any())
            {
                var newWatchlists = new List<WatchListEntity>();

                foreach (var comp in company.Watchlists)
                {
                    var watchlist = await _context.WatchList.FindAsync(comp.Id);

                    if (watchlist != null)
                        newWatchlists.Add(watchlist);
                }

                existing.Watchlists = newWatchlists;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.FinvizCompany.FindAsync(id);

            if (existing == null)
                return false;

            _context.FinvizCompany.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddWatchListToCompany(int companyId, int watchListId)
        {
            var company = await _context.FinvizCompany
                .Include(c => c.Watchlists)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            var watchList = await _context.WatchList.FindAsync(watchListId);

            if (company == null || watchList == null)
                return;

            company.Watchlists.Add(watchList);

            await _context.SaveChangesAsync();
        }

    }
}
