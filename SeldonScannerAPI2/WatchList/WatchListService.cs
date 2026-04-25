using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Models;
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
            return await _context.WatchList.ToListAsync();
        }

        public async Task<WatchListEntity?> GetByIdAsync(int id)
        {
            return await _context.WatchList.FindAsync(id);
        }

        public async Task<WatchListEntity> CreateAsync(WatchListEntity product)
        {
            _context.WatchList.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<WatchListEntity?> UpdateAsync(int id, WatchListEntity product)
        {
            var existing = await _context.WatchList.FindAsync(id);
            if (existing == null)
                return null;

            existing.Company = product.Company;

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


        //public void AddWatchItem(WatchListEntity watchItem)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
