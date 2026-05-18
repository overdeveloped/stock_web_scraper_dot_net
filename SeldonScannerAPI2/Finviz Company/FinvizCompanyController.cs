using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.Finviz_Company
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinvizCompanyController : Controller
    {
        private readonly IFinvizCompanyService _finvizCompanyService;

        public FinvizCompanyController(IFinvizCompanyService finvizCompanyService)
        {
            this._finvizCompanyService = finvizCompanyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _finvizCompanyService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FinvizCompanyEntity>> GetById(int id)
        {
            var result = await this._finvizCompanyService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-ticker")]
        public async Task<ActionResult<FinvizCompanyDTO>> GetByTicker(string ticker)
        {
            var result = await this._finvizCompanyService.GetByTickerAsync(ticker.ToUpper());

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<FinvizCompanyEntity>> Create(FinvizCompanyEntity company)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await this._finvizCompanyService.CreateAsync(company);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<FinvizCompanyEntity>> Update(string ticker, FinvizCompanyEntity updated)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 
            var result = await _finvizCompanyService.UpdateAsync(ticker, updated);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _finvizCompanyService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
