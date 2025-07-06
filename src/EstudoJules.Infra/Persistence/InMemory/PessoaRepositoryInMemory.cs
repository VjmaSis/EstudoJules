using EstudoJules.Domain.Entities;
using EstudoJules.Domain.Interfaces;

namespace EstudoJules.Infra.Persistence.InMemory;

public class PessoaRepositoryInMemory : IPessoaRepository
{
    private readonly List<Pessoa> _pessoas = new();
    private readonly object _lock = new();

    public Task AdicionarAsync(Pessoa pessoa)
    {
        lock (_lock)
        {
            _pessoas.Add(pessoa);
        }
        return Task.CompletedTask;
    }

    public Task<Pessoa?> ObterPorIdAsync(Guid id)
    {
        lock (_lock)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(pessoa);
        }
    }

    public Task<IEnumerable<Pessoa>> ObterTodosAsync()
    {
        lock (_lock)
        {
            // Retorna uma nova lista para evitar modificações externas da lista interna
            return Task.FromResult<IEnumerable<Pessoa>>(_pessoas.ToList());
        }
    }

    public Task AtualizarAsync(Pessoa pessoaAtualizada)
    {
        lock (_lock)
        {
            var pessoaExistente = _pessoas.FirstOrDefault(p => p.Id == pessoaAtualizada.Id);
            if (pessoaExistente != null)
            {
                // Em um cenário real com banco de dados, a atualização seria mais complexa.
                // Aqui, estamos apenas substituindo o objeto na lista.
                // Para evitar problemas com a referência original, podemos remover e adicionar,
                // ou atualizar as propriedades individualmente se a entidade Pessoa permitir.
                // Dada a implementação de Pessoa.AtualizarDados, vamos usá-la.
                pessoaExistente.AtualizarDados(
                    pessoaAtualizada.Nome,
                    pessoaAtualizada.DataNascimento,
                    pessoaAtualizada.Email,
                    pessoaAtualizada.Cpf
                );
            }
            // Poderia lançar uma exceção se a pessoa não for encontrada, dependendo da política de erro.
        }
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Guid id)
    {
        lock (_lock)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Id == id);
            if (pessoa != null)
            {
                _pessoas.Remove(pessoa);
            }
            // Poderia lançar uma exceção se a pessoa não for encontrada, dependendo da política de erro.
        }
        return Task.CompletedTask;
    }
}
