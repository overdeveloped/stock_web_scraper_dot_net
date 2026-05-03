using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.Finviz_Company;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.WatchList
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinvizWatchList : Controller
    {
        //private readonly DataContext dataContext;
        private readonly IWatchListService _watchListService;
        //private readonly FinvizService _finvizFilter = new FinvizService();

        public FinvizWatchList(IWatchListService WatchListService)
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
        public async Task<ActionResult<WatchListEntity>> Create(WatchListEntity watchList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await this._watchListService.CreateAsync(watchList);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("create-with-companies")]
        public async Task<ActionResult<WatchListEntity>> CreateWithCompanies(AttachCompaniesDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await this._watchListService.CreateWithCompaniesAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
