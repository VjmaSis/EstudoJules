using EstudoJules.Application.Interfaces;
using EstudoJules.Domain.Entities;
using EstudoJules.Domain.Interfaces;
using EstudoJules.Domain.ValueObjects; // Necessário para Email e Cpf

namespace EstudoJules.Application.Services;

public class PessoaAppService : IPessoaAppService
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoaAppService(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository ?? throw new ArgumentNullException(nameof(pessoaRepository));
    }

    public async Task<PessoaDto?> AdicionarAsync(CriarPessoaDto criarPessoaDto)
    {
        try
        {
            var email = new Email(criarPessoaDto.Email);
            var cpf = new Cpf(criarPessoaDto.Cpf);
            var pessoa = new Pessoa(criarPessoaDto.Nome, criarPessoaDto.DataNascimento, email, cpf);

            await _pessoaRepository.AdicionarAsync(pessoa);

            return MapToPessoaDto(pessoa);
        }
        catch (ArgumentException ex)
        {
            // Logar a exceção (ex.ToString()) ou retornar um resultado específico indicando o erro de validação
            // Por enquanto, vamos deixar a exceção subir ou retornar null para indicar falha
            // Em um cenário real, trataríamos isso de forma mais granular.
            Console.WriteLine($"Erro de validação ao adicionar pessoa: {ex.Message}");
            return null;
        }
    }

    public async Task<PessoaDto?> ObterPorIdAsync(Guid id)
    {
        var pessoa = await _pessoaRepository.ObterPorIdAsync(id);
        return pessoa != null ? MapToPessoaDto(pessoa) : null;
    }

    public async Task<IEnumerable<PessoaDto>> ObterTodosAsync()
    {
        var pessoas = await _pessoaRepository.ObterTodosAsync();
        return pessoas.Select(MapToPessoaDto);
    }

    public async Task<PessoaDto?> AtualizarAsync(Guid id, AtualizarPessoaDto atualizarPessoaDto)
    {
        var pessoaExistente = await _pessoaRepository.ObterPorIdAsync(id);
        if (pessoaExistente == null)
        {
            return null; // Ou lançar uma exceção de "não encontrado"
        }

        try
        {
            var email = new Email(atualizarPessoaDto.Email);
            var cpf = new Cpf(atualizarPessoaDto.Cpf);

            // Atualiza a entidade existente. A entidade Pessoa tem um método para isso.
            pessoaExistente.AtualizarDados(
                atualizarPessoaDto.Nome,
                atualizarPessoaDto.DataNascimento,
                email,
                cpf
            );

            await _pessoaRepository.AtualizarAsync(pessoaExistente);
            return MapToPessoaDto(pessoaExistente);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Erro de validação ao atualizar pessoa: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        var pessoaExistente = await _pessoaRepository.ObterPorIdAsync(id);
        if (pessoaExistente == null)
        {
            return false; // Pessoa não encontrada
        }

        await _pessoaRepository.RemoverAsync(id);
        return true;
    }

    private static PessoaDto MapToPessoaDto(Pessoa pessoa)
    {
        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            DataNascimento = pessoa.DataNascimento,
            Email = pessoa.Email.Endereco,
            Cpf = pessoa.Cpf.Numero
        };
    }
}
