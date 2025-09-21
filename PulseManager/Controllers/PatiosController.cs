using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseManager.Infraestruture.Context;
using PulseManager.Domain.Entities;

namespace PulseManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatiosController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public PatiosController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            return await _context.Patios.Include(p => p.Zonas).ThenInclude(z => z.Gateways).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patio>> GetPatio(int id)
        {
            var patio = await _context.Patios.Include(p => p.Zonas).ThenInclude(z => z.Gateways).FirstOrDefaultAsync(p => p.Id == id);

            if (patio == null)
            {
                return NotFound();
            }

            return patio;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatio(int id, Patio patio)
        {
            if (id != patio.Id)
            {
                return BadRequest();
            }

            _context.Entry(patio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatioExists(id))
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
        public async Task<ActionResult<Patio>> PostPatio(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatio", new { id = patio.Id }, patio);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(int id)
        {
            return _context.Patios.Any(e => e.Id == id);
        }
    }
}