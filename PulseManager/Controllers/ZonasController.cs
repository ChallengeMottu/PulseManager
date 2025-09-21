using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseManager.Infraestruture.Context;
using PulseManager.Domain.Entities;
using PulseManager.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PulseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Gerenciamento de Zonas - CRUD completo para operações com zonas")]
    public class ZonasController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public ZonasController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Obter todas as zonas",
            Description = "Retorna uma lista paginada de todas as zonas cadastradas no sistema",
            OperationId = "GetZonas"
        )]
        [SwaggerResponse(200, "Lista de zonas retornada com sucesso", typeof(IEnumerable<Zona>))]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonas(
            [FromQuery, SwaggerParameter("Número da página", Required = false)] int page = 1, 
            [FromQuery, SwaggerParameter("Tamanho da página (máximo 100)", Required = false)] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var totalCount = await _context.Zonas.CountAsync();
            var zonas = await _context.Zonas
                .Include(z => z.Patio)
                .Include(z => z.Gateways)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Headers de paginação
            Response.Headers.Append("X-Pagination-TotalCount", totalCount.ToString());
            Response.Headers.Append("X-Pagination-Page", page.ToString());
            Response.Headers.Append("X-Pagination-PageSize", pageSize.ToString());
            Response.Headers.Append("X-Pagination-TotalPages", Math.Ceiling(totalCount / (double)pageSize).ToString());
            Response.Headers.Append("X-Pagination-HasNext", (page < Math.Ceiling(totalCount / (double)pageSize)).ToString());
            Response.Headers.Append("X-Pagination-HasPrevious", (page > 1).ToString());

            return Ok(zonas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obter zona por ID",
            Description = "Retorna uma zona específica com base no ID fornecido",
            OperationId = "GetZona"
        )]
        [SwaggerResponse(200, "Zona encontrada com sucesso", typeof(ApiResponse<Zona>))]
        [SwaggerResponse(404, "Zona não encontrada")]
        public async Task<ActionResult<ApiResponse<Zona>>> GetZona(
            [SwaggerParameter("ID da zona", Required = true)] int id)
        {
            var zona = await _context.Zonas
                .Include(z => z.Patio)
                .Include(z => z.Gateways)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zona == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<Zona>
            {
                Data = zona,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetZona", new { id }) ?? $"/api/zonas/{id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutZona", new { id }) ?? $"/api/zonas/{id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeleteZona", new { id }) ?? $"/api/zonas/{id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetZonas") ?? "/api/zonas", Method = "GET" },
                    new Link { Rel = "patio", Href = Url.Action("GetPatio", "Patios", new { id = zona.PatioId }) ?? $"/api/patios/{zona.PatioId}", Method = "GET" },
                    new Link { Rel = "gateways", Href = Url.Action("GetGatewaysByZona", "Gateways", new { zonaId = id }) ?? $"/api/gateways?zonaId={id}", Method = "GET" }
                }
            };

            return Ok(response);
        }

        [HttpGet("by-patios/{patioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obter zonas por pátio",
            Description = "Retorna todas as zonas pertencentes a um pátio específico",
            OperationId = "GetZonasByPatio"
        )]
        [SwaggerResponse(200, "Zonas do pátio retornadas com sucesso", typeof(IEnumerable<Zona>))]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonasByPatio(
            [SwaggerParameter("ID do pátio", Required = true)] int patioId)
        {
            var zonas = await _context.Zonas
                .Where(z => z.PatioId == patioId)
                .Include(z => z.Gateways)
                .ToListAsync();

            return Ok(zonas);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualizar zona",
            Description = "Atualiza os dados de uma zona existente",
            OperationId = "UpdateZona"
        )]
        [SwaggerResponse(204, "Zona atualizada com sucesso")]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        [SwaggerResponse(404, "Zona não encontrada")]
        public async Task<IActionResult> PutZona(
            [SwaggerParameter("ID da zona", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados da zona para atualização")] Zona zona)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Criar nova zona",
            Description = "Cria uma nova zona com os dados fornecidos",
            OperationId = "CreateZona"
        )]
        [SwaggerResponse(201, "Zona criada com sucesso", typeof(ApiResponse<Zona>))]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        public async Task<ActionResult<ApiResponse<Zona>>> PostZona(
            [FromBody, SwaggerRequestBody("Dados da nova zona")] Zona zona)
        {
            _context.Zonas.Add(zona);
            await _context.SaveChangesAsync();

            var response = new ApiResponse<Zona>
            {
                Data = zona,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetZona", new { id = zona.Id }) ?? $"/api/zonas/{zona.Id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutZona", new { id = zona.Id }) ?? $"/api/zonas/{zona.Id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeleteZona", new { id = zona.Id }) ?? $"/api/zonas/{zona.Id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetZonas") ?? "/api/zonas", Method = "GET" },
                    new Link { Rel = "patio", Href = Url.Action("GetPatio", "Patios", new { id = zona.PatioId }) ?? $"/api/patios/{zona.PatioId}", Method = "GET" }
                }
            };

            return CreatedAtAction("GetZona", new { id = zona.Id }, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Excluir zona",
            Description = "Exclui uma zona existente com base no ID fornecido",
            OperationId = "DeleteZona"
        )]
        [SwaggerResponse(204, "Zona excluída com sucesso")]
        [SwaggerResponse(404, "Zona não encontrada")]
        public async Task<IActionResult> DeleteZona(
            [SwaggerParameter("ID da zona a ser excluída", Required = true)] int id)
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