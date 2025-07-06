using EstudoJules.Domain.ValueObjects;

namespace EstudoJules.Domain.Entities;

public class Pessoa
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }

    public Pessoa(string nome, DateTime dataNascimento, Email email, Cpf cpf)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        DataNascimento = dataNascimento;
        Email = email;
        Cpf = cpf;
    }

    // Construtor para EF Core ou outros ORMs que necessitem de um construtor sem parâmetros
    private Pessoa() { }

    public void AtualizarDados(string nome, DateTime dataNascimento, Email email, Cpf cpf)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        Email = email;
        Cpf = cpf;
    }
}
