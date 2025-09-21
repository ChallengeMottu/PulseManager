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
    [SwaggerTag("Gerenciamento de Gateways - CRUD completo para operações com gateways")]
    public class GatewaysController : ControllerBase
    {
        private readonly ManagerDbContext _context;

        public GatewaysController(ManagerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Obter todos os gateways",
            Description = "Retorna uma lista paginada de todos os gateways cadastrados no sistema",
            OperationId = "GetGateways"
        )]
        [SwaggerResponse(200, "Lista de gateways retornada com sucesso", typeof(IEnumerable<Gateway>))]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGateways(
            [FromQuery, SwaggerParameter("Número da página", Required = false)] int page = 1, 
            [FromQuery, SwaggerParameter("Tamanho da página (máximo 100)", Required = false)] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var totalCount = await _context.Gateways.CountAsync();
            var gateways = await _context.Gateways
                .Include(g => g.Zona)
                .ThenInclude(z => z.Patio)
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

            return Ok(gateways);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obter gateway por ID",
            Description = "Retorna um gateway específico com base no ID fornecido",
            OperationId = "GetGateway"
        )]
        [SwaggerResponse(200, "Gateway encontrado com sucesso", typeof(ApiResponse<Gateway>))]
        [SwaggerResponse(404, "Gateway não encontrado")]
        public async Task<ActionResult<ApiResponse<Gateway>>> GetGateway(
            [SwaggerParameter("ID do gateway", Required = true)] int id)
        {
            var gateway = await _context.Gateways
                .Include(g => g.Zona)
                .ThenInclude(z => z.Patio)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gateway == null)
            {
                return NotFound();
            }

            var response = new ApiResponse<Gateway>
            {
                Data = gateway,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetGateway", new { id }) ?? $"/api/gateways/{id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutGateway", new { id }) ?? $"/api/gateways/{id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeleteGateway", new { id }) ?? $"/api/gateways/{id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetGateways") ?? "/api/gateways", Method = "GET" },
                    new Link { Rel = "zona", Href = Url.Action("GetZona", "Zonas", new { id = gateway.ZonaId }) ?? $"/api/zonas/{gateway.ZonaId}", Method = "GET" }
                }
            };

            return Ok(response);
        }

        [HttpGet("by-zonas/{zonaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obter gateways por zona",
            Description = "Retorna todos os gateways pertencentes a uma zona específica",
            OperationId = "GetGatewaysByZona"
        )]
        [SwaggerResponse(200, "Gateways da zona retornados com sucesso", typeof(IEnumerable<Gateway>))]
        [SwaggerResponse(404, "Zona não encontrada")]
        public async Task<ActionResult<IEnumerable<Gateway>>> GetGatewaysByZona(
            [SwaggerParameter("ID da zona", Required = true)] int zonaId)
        {
            var gateways = await _context.Gateways
                .Where(g => g.ZonaId == zonaId)
                .Include(g => g.Zona)
                .ToListAsync();

            return Ok(gateways);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualizar gateway",
            Description = "Atualiza os dados de um gateway existente",
            OperationId = "UpdateGateway"
        )]
        [SwaggerResponse(204, "Gateway atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        [SwaggerResponse(404, "Gateway não encontrado")]
        public async Task<IActionResult> PutGateway(
            [SwaggerParameter("ID do gateway", Required = true)] int id,
            [FromBody, SwaggerRequestBody("Dados do gateway para atualização")] Gateway gateway)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Criar novo gateway",
            Description = "Cria um novo gateway com os dados fornecidos",
            OperationId = "CreateGateway"
        )]
        [SwaggerResponse(201, "Gateway criado com sucesso", typeof(ApiResponse<Gateway>))]
        [SwaggerResponse(400, "Dados inválidos fornecidos")]
        public async Task<ActionResult<ApiResponse<Gateway>>> PostGateway(
            [FromBody, SwaggerRequestBody("Dados do novo gateway")] Gateway gateway)
        {
            _context.Gateways.Add(gateway);
            await _context.SaveChangesAsync();

            var response = new ApiResponse<Gateway>
            {
                Data = gateway,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Action("GetGateway", new { id = gateway.Id }) ?? $"/api/gateways/{gateway.Id}", Method = "GET" },
                    new Link { Rel = "update", Href = Url.Action("PutGateway", new { id = gateway.Id }) ?? $"/api/gateways/{gateway.Id}", Method = "PUT" },
                    new Link { Rel = "delete", Href = Url.Action("DeleteGateway", new { id = gateway.Id }) ?? $"/api/gateways/{gateway.Id}", Method = "DELETE" },
                    new Link { Rel = "all", Href = Url.Action("GetGateways") ?? "/api/gateways", Method = "GET" },
                    new Link { Rel = "zona", Href = Url.Action("GetZona", "Zonas", new { id = gateway.ZonaId }) ?? $"/api/zonas/{gateway.ZonaId}", Method = "GET" }
                }
            };

            return CreatedAtAction("GetGateway", new { id = gateway.Id }, response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Excluir gateway",
            Description = "Exclui um gateway existente com base no ID fornecido",
            OperationId = "DeleteGateway"
        )]
        [SwaggerResponse(204, "Gateway excluído com sucesso")]
        [SwaggerResponse(404, "Gateway não encontrado")]
        public async Task<IActionResult> DeleteGateway(
            [SwaggerParameter("ID do gateway a ser excluído", Required = true)] int id)
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