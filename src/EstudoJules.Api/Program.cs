using EstudoJules.Application.Interfaces;
using EstudoJules.Application.Services;
using EstudoJules.Domain.Interfaces;
using EstudoJules.Infra.Persistence.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();

// Configuração da Injeção de Dependência
builder.Services.AddSingleton<IPessoaRepository, PessoaRepositoryInMemory>(); // Singleton para repositório em memória
builder.Services.AddScoped<IPessoaAppService, PessoaAppService>();

// Adicionar suporte para Swagger/OpenAPI (opcional, mas útil para APIs)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "EstudoJules API", Version = "v1" });
});

// Configurar CORS para permitir chamadas do Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5001", "https://localhost:5001") // Endereço do app Blazor (ajuste se necessário)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configurar o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EstudoJules API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorApp"); // Aplicar a política CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
