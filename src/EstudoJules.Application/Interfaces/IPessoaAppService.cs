using EstudoJules.Domain.Entities; // Precisamos referenciar Entidades para Pessoa
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstudoJules.Application.Interfaces;

// Adicionando DTOs para desacoplar a camada de aplicação da camada de domínio diretamente nos contratos.
// No entanto, para simplificar esta primeira versão e seguir o requisito de usar entidades diretamente
// nos serviços de aplicação, vamos usar a entidade Pessoa diretamente aqui.
// Em uma evolução, DTOs (Data Transfer Objects) seriam recomendados.

public class PessoaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
}

public class CriarPessoaDto
{
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
}

public class AtualizarPessoaDto
{
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
}

public interface IPessoaAppService
{
    Task<PessoaDto?> AdicionarAsync(CriarPessoaDto criarPessoaDto);
    Task<PessoaDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<PessoaDto>> ObterTodosAsync();
    Task<PessoaDto?> AtualizarAsync(Guid id, AtualizarPessoaDto atualizarPessoaDto);
    Task<bool> RemoverAsync(Guid id);
}
