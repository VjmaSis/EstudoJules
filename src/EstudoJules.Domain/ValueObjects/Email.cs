using System.Text.RegularExpressions;

namespace EstudoJules.Domain.ValueObjects;

public record Email
{
    public string Endereco { get; }

    public Email(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new ArgumentException("O endereço de e-mail não pode ser vazio.", nameof(endereco));

        if (!IsValid(endereco))
            throw new ArgumentException("Formato de e-mail inválido.", nameof(endereco));

        Endereco = endereco;
    }

    public static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Expressão regular simples para validação de e-mail.
        // Para uma validação mais robusta, considere bibliotecas especializadas.
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    public override string ToString() => Endereco;
}
