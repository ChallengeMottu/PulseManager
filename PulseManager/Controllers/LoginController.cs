using Microsoft.AspNetCore.Mvc;
using PulseManager.Application.Dto;
using PulseManager.Application.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PulseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("autenticar")]
        [SwaggerOperation(Summary = "Autentica um usuário e verifica bloqueio")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Autenticar([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var response = await _loginService.AutenticarAsync(loginRequestDto);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { mensagem = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os logins cadastrados")]
        [ProducesResponseType(typeof(IEnumerable<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LoginResponseDto>>> ListarTodos()
        {
            var lista = await _loginService.ListarTodosAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca login cadastrado por ID")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDto>> BuscarPorId(Guid id)
        {
            try
            {
                var login = await _loginService.BuscarPorIdAsync(id);
                return Ok(login);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpGet("cpf/{cpf}")]
        [SwaggerOperation(Summary = "Busca login cadastrado por CPF")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDto>> BuscarPorCpf(string cpf)
        {
            try
            {
                var login = await _loginService.BuscarPorCpfAsync(cpf);
                return Ok(login);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}/senha")]
        [SwaggerOperation(Summary = "Atualiza senha do login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarSenha(Guid id, [FromBody] string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(novaSenha))
                return BadRequest(new { mensagem = "Nova senha inválida." });

            try
            {
                await _loginService.AtualizarSenhaAsync(id, novaSenha);
                return Ok(new { mensagem = "Senha atualizada com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deleta login pelo ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _loginService.DeleteAsync(id);
                return Ok(new { mensagem = "Login deletado com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPost("{id}/desbloquear")]
        [SwaggerOperation(Summary = "Desbloqueia o usuário")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Desbloquear(Guid id)
        {
            try
            {
                await _loginService.DesbloquearUsuarioAsync(id);
                return Ok(new { mensagem = "Usuário desbloqueado com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }


    }
}
