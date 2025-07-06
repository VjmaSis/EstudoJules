using EstudoJules.Application.Interfaces; // Para DTOs
using System.Net.Http;
using System.Net.Http.Json;

namespace EstudoJules.Web.Services;

public interface IPessoaApiService
{
    Task<IEnumerable<PessoaDto>?> ObterTodosAsync();
    Task<PessoaDto?> ObterPorIdAsync(Guid id);
    Task<PessoaDto?> AdicionarAsync(CriarPessoaDto pessoa);
    Task<PessoaDto?> AtualizarAsync(Guid id, AtualizarPessoaDto pessoa);
    Task<bool> RemoverAsync(Guid id);
}

public class PessoaApiService : IPessoaApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseApiUrl = "api/pessoas"; // Define a URL base da API

    public PessoaApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<PessoaDto>?> ObterTodosAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<PessoaDto>>(BaseApiUrl);
        }
        catch (HttpRequestException ex)
        {
            // Logar ou tratar o erro especificamente
            Console.WriteLine($"Erro ao obter todas as pessoas: {ex.Message}");
            return null; // Ou lançar uma exceção customizada
        }
    }

    public async Task<PessoaDto?> ObterPorIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PessoaDto>($"{BaseApiUrl}/{id}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro ao obter pessoa por ID {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<PessoaDto?> AdicionarAsync(CriarPessoaDto pessoa)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseApiUrl, pessoa);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PessoaDto>();
            }
            // Logar o erro vindo da API
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro ao adicionar pessoa: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro HTTP ao adicionar pessoa: {ex.Message}");
            return null;
        }
    }

    public async Task<PessoaDto?> AtualizarAsync(Guid id, AtualizarPessoaDto pessoa)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseApiUrl}/{id}", pessoa);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PessoaDto>();
            }
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro ao atualizar pessoa {id}: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro HTTP ao atualizar pessoa {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseApiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro HTTP ao remover pessoa {id}: {ex.Message}");
            return false;
        }
    }
}
