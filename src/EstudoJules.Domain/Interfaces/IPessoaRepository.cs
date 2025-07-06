using EstudoJules.Domain.Entities;

namespace EstudoJules.Domain.Interfaces;

public interface IPessoaRepository
{
    Task AdicionarAsync(Pessoa pessoa);
    Task<Pessoa?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Pessoa>> ObterTodosAsync();
    Task AtualizarAsync(Pessoa pessoa);
    Task RemoverAsync(Guid id);
}
