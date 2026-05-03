using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SeldonStockScannerAPI.Finviz_Company;
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

        public async Task<IEnumerable<WatchListDTO>> GetAllAsync()
        {
            var entities = await _context.WatchList
                .Include(w => w.Companies)
                .ToListAsync();

            var entityDTOs = new List<WatchListDTO>();

            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    entityDTOs.Add(new WatchListDTO()
                    {
                        Id = entity.Id,
                        WatchListName = entity.WatchListName,
                        Companies = entity.Companies
                            .Select(w => new FinvizCompanyDTO
                            { 
                                Id = w.Id,
                                Company = w.Company,
                                Sector = w.Sector,
                                Industry = w.Industry,
                                Country = w.Country,
                                MarketCap = w.MarketCap,
                                PE = w.PE,
                                Price = w.Price,
                                Change = w.Change,
                                Volume = w.Volume
                            })
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

        public async Task<WatchListDTO> GetByIdAsync(int id)
        {
            var entity = await _context.WatchList
                .AsNoTracking()
                .Include(w => w.Companies)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
                return null;

            return new WatchListDTO()
            {
                Id = entity.Id,
                WatchListName = entity.WatchListName,
                Companies = entity.Companies
                    .Select(w => new FinvizCompanyDTO
                    {
                        Id = w.Id,
                        Company = w.Company,
                        Sector = w.Sector,
                        Industry = w.Industry,
                        Country = w.Country,
                        MarketCap = w.MarketCap,
                        PE = w.PE,
                        Price = w.Price,
                        Change = w.Change,
                        Volume = w.Volume
                    })
                    .ToList()
            };
        }

        public async Task<WatchListEntity> CreateAsync(WatchListEntity watchList)
        {
            _context.WatchList.Add(watchList);
            await _context.SaveChangesAsync();
            return watchList;
        }

        public async Task<WatchListEntity> CreateWithCompaniesAsync(AttachCompaniesDTO request)
        {
            var companies = await _context.FinvizCompany
                .Where(c => request.CompanyIds.Contains(c.Id))
                .ToListAsync();

            var watchList = new WatchListEntity
            {
                WatchListName = request.WatchListName
            };

            watchList.Companies = companies;

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

        public async Task AddCompanyToWatchList(int watchListId, int companyId)
        {
            var watchList = await _context.WatchList
                .Include(w => w.Companies)
                .FirstOrDefaultAsync(w => w.Id == watchListId);

            var company = await _context.FinvizCompany.FindAsync(companyId);

            if (watchList == null || company == null)
                return;

            watchList.Companies.Add(company);

            await _context.SaveChangesAsync();
        }
    }
}
