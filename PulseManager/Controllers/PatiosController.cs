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
    [SwaggerTag("Gerenciamento de Pátios - CRUD completo para operações com pátios")]
    public class PatiosController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public PatiosController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Obter todos os pátios",
            Description = "Retorna uma lista paginada de todos os pátios cadastrados no sistema",
            OperationId = "GetPatios"
        )]
        [SwaggerResponse(200, "Lista de pátios retornada com sucesso", typeof(IEnumerable<Patio>))]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios(
            [FromQuery, SwaggerParameter("Número da página", Required = false)] int page = 1, 
            [FromQuery, SwaggerParameter("Tamanho da página (máximo 100)", Required = false)] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var totalCount = await _context.Patios.CountAsync();
            var patios = await _context.Patios
                .Include(p => p.Zonas)
                .ThenInclude(z => z.Gateways)
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

            return Ok(patios);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obter pátio por ID",
            Description = "Retorna um pátio específico com base no ID fornecido",
            OperationId = "GetPatio"
        )]
        [SwaggerResponse(200, "Pátio encontrado com sucesso", typeof(ApiResponse<Patio>))]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<ActionResult<ApiResponse<Patio>>> GetPatio(
            [SwaggerParameter("ID do pátio", Required = true)] int id)
        {
            var patio = await _context.Patios
                .Include(p => p.Zonas)
                .ThenInclude(z => z.Gateways)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patio == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<Patio>
            {
                Data = patio,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetPatio", new { id }) ?? $"/api/patios/{id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutPatio", new { id }) ?? $"/api/patios/{id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeletePatio", new { id }) ?? $"/api/patios/{id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetPatios") ?? "/api/patios", Method = "GET" },
                    new Link { Rel = "zonas", Href = Url.Action("GetZonasByPatio", "Zonas", new { patioId = id }) ?? $"/api/zonas?patioId={id}", Method = "GET" }
                }
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualizar pátio",
            Description = "Atualiza os dados de um pátio existente",
            OperationId = "UpdatePatio"
        )]
        [SwaggerResponse(204, "Pátio atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<IActionResult> PutPatio(
            [SwaggerParameter("ID do pátio", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados do pátio para atualização")] Patio patio)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Criar novo pátio",
            Description = "Cria um novo pátio com os dados fornecidos",
            OperationId = "CreatePatio"
        )]
        [SwaggerResponse(201, "Pátio criado com sucesso", typeof(ApiResponse<Patio>))]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        public async Task<ActionResult<ApiResponse<Patio>>> PostPatio(
            [FromBody, SwaggerRequestBody("Dados do novo pátio")] Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            var response = new ApiResponse<Patio>
            {
                Data = patio,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetPatio", new { id = patio.Id }) ?? $"/api/patios/{patio.Id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutPatio", new { id = patio.Id }) ?? $"/api/patios/{patio.Id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeletePatio", new { id = patio.Id }) ?? $"/api/patios/{patio.Id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetPatios") ?? "/api/patios", Method = "GET" }
                }
            };

            return CreatedAtAction("GetPatio", new { id = patio.Id }, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Excluir pátio",
            Description = "Exclui um pátio existente com base no ID fornecido",
            OperationId = "DeletePatio"
        )]
        [SwaggerResponse(204, "Pátio excluído com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<IActionResult> DeletePatio(
            [SwaggerParameter("ID do pátio a ser excluído", Required = true)] int id)
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