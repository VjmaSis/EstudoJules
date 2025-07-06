using System.Text.RegularExpressions;

namespace EstudoJules.Domain.ValueObjects;

public record Cpf
{
    public string Numero { get; }

    public Cpf(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("O CPF não pode ser vazio.", nameof(numero));

        var numeroLimpo = LimparFormatacao(numero);

        if (!IsValid(numeroLimpo))
            throw new ArgumentException("Número de CPF inválido.", nameof(numero));

        Numero = Formatar(numeroLimpo);
    }

    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var numeroLimpo = LimparFormatacao(cpf);

        if (numeroLimpo.Length != 11)
            return false;

        if (numeroLimpo.Distinct().Count() == 1) // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            return false;

        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;

        tempCpf = numeroLimpo.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito += resto.ToString();

        return numeroLimpo.EndsWith(digito);
    }

    public static string LimparFormatacao(string cpf)
    {
        return Regex.Replace(cpf, @"[^\d]", ""); // Remove todos os caracteres não numéricos
    }

    private static string Formatar(string cpfLimpo)
    {
        if (string.IsNullOrWhiteSpace(cpfLimpo) || cpfLimpo.Length != 11)
            return cpfLimpo; // Retorna o valor original se não for um CPF de 11 dígitos

        return Convert.ToUInt64(cpfLimpo).ToString(@"000\.000\.000\-00");
    }

    public override string ToString() => Numero;

    public string ObterNumeroSemFormatacao() => LimparFormatacao(Numero);
}
