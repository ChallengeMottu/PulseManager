using PulseManager.Application.Dto;
using PulseManager.Domain.Entities;

public class Login
{
    public Guid Id { get; private set; }
    public string NumeroCpf { get; private set; }
    public string SenhaHash { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; }
    public int TentativasLogin { get; private set; }

    public Login() { }

    public Login(UsuarioRequestDto cadastroRequest, Usuario usuario)
    {
        Id = Guid.NewGuid();
        NumeroCpf = cadastroRequest.Cpf;
        Usuario = usuario;
        UsuarioId = usuario.Id;
        DefinirSenha(cadastroRequest.Senha);
        TentativasLogin = 0;
    }

    public bool EstaBloqueado()
    {
        return TentativasLogin >= 5;
    }

    public void DefinirSenha(string senha)
    {
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool VerificarSenha(string senha)
    {
        if (EstaBloqueado())
            return false; 

        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, SenhaHash);

        if (senhaCorreta)
            ResetarTentativas();
        else
            IncrementarTentativas();

        return senhaCorreta;
    }

    public void IncrementarTentativas()
    {
        TentativasLogin++;
    }

    public void ResetarTentativas()
    {
        TentativasLogin = 0;
    }

    public void Desbloquear()
    {
        ResetarTentativas();
    }
}
