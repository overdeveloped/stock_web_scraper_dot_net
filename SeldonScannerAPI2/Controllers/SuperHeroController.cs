using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


// https://www.youtube.com/watch?v=Fbf_ua2t6v4

namespace SeldonStockScannerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuperHero>>> GetHeroes()
        {
            return Ok(getAllHeroes());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            SuperHero hero = await dataContext.SuperHeroes.FindAsync(id);

            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            else
            {
                return Ok(hero);
            }
        }

        [HttpPost()]
        public async Task<ActionResult<IEnumerable<SuperHero>>> CreateHero(SuperHero hero)
        {
            dataContext.SuperHeroes.Add(hero);
            await dataContext.SaveChangesAsync();
            //heroes.Add(hero);
            return Ok(getAllHeroes());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<SuperHero>>> PutHero(SuperHero update)
        {
            SuperHero hero = await dataContext.SuperHeroes.FindAsync(update.Id);
            //SuperHero hero = heroes.Find(x => x.Id == update.Id);

            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            else
            {
                hero.Name = update.Name;
                hero.FirstName = update.FirstName;
                hero.LastName = update.LastName;
                hero.Place = update.Place;

                await dataContext.SaveChangesAsync();

                return Ok(getAllHeroes());
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<SuperHero>>> DeleteHero(int id)
        {
            SuperHero hero = await dataContext.SuperHeroes.FindAsync(id);
            //SuperHero hero = heroes.Find(x => x.Id == id);

            if (hero == null)
            {
                return BadRequest("Hero not found");
            }
            else
            {
                dataContext.SuperHeroes.Remove(hero);
                await dataContext.SaveChangesAsync();

                //heroes.Remove(hero);
                return Ok(getAllHeroes());
            }
        }

        private ActionResult<IEnumerable<SuperHero>> getAllHeroes()
        {
            return dataContext.SuperHeroes.ToList();
        }
    }
}
