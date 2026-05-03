using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Models;

namespace SeldonStockScannerAPI.Finviz_Company
{
    public class FinvizCompanyService : IFinvizCompanyService
    {
        private readonly ApplicationDbContext _context;

        public FinvizCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FinvizCompanyEntity>> GetAllAsync()
        {
            return  await _context.FinvizCompany
                .Include(c => c.Watchlists)
                .ToListAsync();
        }

        public async Task<FinvizCompanyEntity?> GetByIdAsync(int id)
        {
            return await _context.FinvizCompany
                .AsNoTracking()
                .Include(c => c.Watchlists)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity company)
        {
            _context.FinvizCompany.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<FinvizCompanyEntity?> UpdateAsync(int id, FinvizCompanyEntity company)
        {
            var existing = await _context.FinvizCompany
                .Include(c => c.Watchlists)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existing == null)
                return null;

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
                company.Watchlists.Clear();

                foreach (var comp in company.Watchlists)
                {
                    var watchlist = await _context.WatchList.FindAsync(comp.Id);

                    if (watchlist != null)
                        company.Watchlists.Add(watchlist);
                }
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
    }
}
