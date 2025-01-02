namespace NSE.Clientes.API.Models;

public interface IClienteRepository
{
    void Adicionar(Cliente cliente);
    Task<IEnumerable<Cliente>> ObterTodos();
    Task<Cliente> ObterPorCpf(string cpf);
    void AdicionarEndereco(Endereco endereco);
    Task<Endereco> ObterEnderecoPorId(Guid id);
}
