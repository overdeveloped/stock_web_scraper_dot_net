using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.Models;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.Finviz_Company
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinvizCompanyController : Controller
    {
        //private readonly DataContext dataContext;
        private readonly IWatchListService _watchListService;
        //private readonly FinvizService _finvizFilter = new FinvizService();

        public FinvizCompanyController(IWatchListService WatchListService)
        {
            this._watchListService = WatchListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _watchListService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WatchListEntity>> GetById(int id)
        {
            var result = await this._watchListService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpPost]
        public void AddWatch(WatchListEntity watchItem)
        {
            this._watchListService.CreateAsync(watchItem);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<WatchListEntity>> Update(int id, WatchListEntity updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _watchListService.UpdateAsync(id, updated);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _watchListService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
