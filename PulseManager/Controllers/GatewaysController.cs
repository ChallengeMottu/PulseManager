using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseManager.Infraestruture.Context;
using PulseManager.Domain.Entities;

namespace PulseManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewaysController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public GatewaysController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways()
        {
            return await _context.Gateways.Include(g => g.Zona).ThenInclude(z => z.Patio).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gateway>> GetGateway(int id)
        {
            var gateway = await _context.Gateways.Include(g => g.Zona).ThenInclude(z => z.Patio).FirstOrDefaultAsync(g => g.Id == id);

            if (gateway == null)
            {
                return NotFound();
            }

            return gateway;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGateway(int id, Gateway gateway)
        {
            if (id != gateway.Id)
            {
                return BadRequest();
            }

            _context.Entry(gateway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatewayExists(id))
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
        public async Task<ActionResult<Gateway>> PostGateway(Gateway gateway)
        {
            _context.Gateways.Add(gateway);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGateway", new { id = gateway.Id }, gateway);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGateway(int id)
        {
            var gateway = await _context.Gateways.FindAsync(id);
            if (gateway == null)
            {
                return NotFound();
            }

            _context.Gateways.Remove(gateway);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GatewayExists(int id)
        {
            return _context.Gateways.Any(e => e.Id == id);
        }
    }
}