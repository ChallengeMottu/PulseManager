using Microsoft.AspNetCore.Mvc;
using PulseManager.Application.Dto;
using PulseManager.Application.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PulseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os Usuários cadastrados")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> ListarTodos()
        {
            var usuarios = await _usuarioService.ListarTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca usuário cadastrado por ID")]
        [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioResponseDto>> BuscarPorId(Guid id)
        {
            try
            {
                var usuarioDto = await _usuarioService.BuscarPorIdAsync(id);
                return Ok(usuarioDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria cadastro do usuário")]
        [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioResponseDto>> Criar([FromBody] UsuarioRequestDto usuarioRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    mensagem = "Dados de entrada inválidos.",
                    erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var usuarioCriado = await _usuarioService.CriarAsync(usuarioRequest);
            return CreatedAtAction(nameof(BuscarPorId), new { id = usuarioCriado.Id }, usuarioCriado);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza dados de usuário cadastrado")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioRequestDto usuarioRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados de entrada inválidos.", erros = ModelState });

            try
            {
                await _usuarioService.AtualizarAsync(id, usuarioRequest);
                return Ok(new { mensagem = "Cadastro atualizado com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deleta cadastro pelo ID")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _usuarioService.DeletarAsync(id);
                return Ok(new { mensagem = "Cadastro deletado com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
