using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseManager.Infraestruture.Context;
using PulseManager.Domain.Entities;

namespace PulseManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonasController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public ZonasController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
        {
            return await _context.Zonas.Include(z => z.Patio).Include(z => z.Gateways).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Zona>> GetZona(int id)
        {
            var zona = await _context.Zonas.Include(z => z.Patio).Include(z => z.Gateways).FirstOrDefaultAsync(z => z.Id == id);

            if (zona == null)
            {
                return NotFound();
            }

            return zona;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutZona(int id, Zona zona)
        {
            if (id != zona.Id)
            {
                return BadRequest();
            }

            _context.Entry(zona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZonaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Zona>> PostZona(Zona zona)
        {
            _context.Zonas.Add(zona);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZona", new { id = zona.Id }, zona);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZona(int id)
        {
            var zona = await _context.Zonas.FindAsync(id);
            if (zona == null)
            {
                return NotFound();
            }

            _context.Zonas.Remove(zona);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZonaExists(int id)
        {
            return _context.Zonas.Any(e => e.Id == id);
        }
    }
}