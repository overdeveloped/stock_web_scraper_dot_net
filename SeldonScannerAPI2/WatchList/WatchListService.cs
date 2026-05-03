using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace SeldonStockScannerAPI.WatchList
{
    public class WatchListService : IWatchListService
    {
        private readonly ApplicationDbContext _context;

        public WatchListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WatchListEntity>> GetAllAsync()
        {
            return await _context.WatchList
                .Include(w => w.Companies)
                .ToListAsync();
        }

        public async Task<WatchListEntity> GetByIdAsync(int id)
        {
            return await _context.WatchList
                .AsNoTracking()
                .Include(w => w.Companies)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WatchListEntity> CreateAsync(WatchListEntity watchList)
        {
            _context.WatchList.Add(watchList);
            await _context.SaveChangesAsync();
            return watchList;
        }

        public async Task<WatchListEntity?> UpdateAsync(int id, WatchListEntity watchList)
        {
            var existing = await _context.WatchList
                .Include(w => w.Companies)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (existing == null)
                return null;

            existing.WatchListName = watchList.WatchListName;

            // Update companies
            if (watchList.Companies != null && watchList.Companies.Any())
            {
                watchList.Companies.Clear();

                foreach (var comp in watchList.Companies)
                {
                    var company = await _context.FinvizCompany.FindAsync(comp.Id);

                    if (company != null)
                        watchList.Companies.Add(company);
                }
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.WatchList.FindAsync(id);
            if (existing == null)
                return false;

            _context.WatchList.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
