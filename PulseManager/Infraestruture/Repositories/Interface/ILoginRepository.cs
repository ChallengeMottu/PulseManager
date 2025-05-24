namespace PulseManager.Infraestruture.Repositories.Interface
{
    public interface ILoginRepository : IMethodsRepository<Login>
    {
        Task<Login?> GetByCpfAsync(string cpf);
        
    }
}
