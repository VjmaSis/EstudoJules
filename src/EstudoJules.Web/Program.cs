using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using EstudoJules.Web.Services; // Adicionado para IPessoaApiService e PessoaApiService

namespace EstudoJules.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        // HeadOutlet e outras configurações de root component podem ser adicionadas aqui se necessário.
        // builder.RootComponents.Add<HeadOutlet>("head::after");

        // Configura o HttpClient para apontar para a API.
        // O endereço base deve corresponder ao endereço onde sua API está rodando.
        // Ajuste a URL base da API conforme necessário (ex: http://localhost:5000 ou https://localhost:7001)
        // Esta URL deve corresponder à URL em que sua API EstudoJules.Api está sendo executada.
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") }); // Ajustado para o endereço da API

        // Registra o serviço de API para ser injetado nos componentes
        builder.Services.AddScoped<IPessoaApiService, PessoaApiService>();

        builder.Services.AddMudServices();

        await builder.Build().RunAsync();
    }
}
