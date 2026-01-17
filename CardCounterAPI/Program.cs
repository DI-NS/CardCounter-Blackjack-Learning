using CardCounterAPI.Services;
using CardCounterAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CONFIGURAÇÃO DOS SERVIÇOS
// ==========================================

// Registra os controllers
builder.Services.AddControllers();

// Registra o DeckService como Singleton
// Singleton = Uma única instância para toda a aplicação
builder.Services.AddSingleton<DeckService>();

// Registra o DatabaseService como Singleton
builder.Services.AddSingleton<DatabaseService>();

// Registra o GameRepository como Scoped (uma instância por requisição)
builder.Services.AddScoped<GameRepository>();

// Configura o Swagger (documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o CORS (permite o site acessar a API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Qualquer origem
              .AllowAnyMethod()    // Qualquer método (GET, POST, etc)
              .AllowAnyHeader();   // Qualquer cabeçalho
    });
});

var app = builder.Build();

// ==========================================
// INICIALIZAÇÃO DO BANCO DE DADOS
// ==========================================

// Inicializa as tabelas do banco ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var databaseService = scope.ServiceProvider.GetRequiredService<DatabaseService>();

    try
    {
        await databaseService.InitializeDatabaseAsync();
        Console.WriteLine("✅ Banco de dados inicializado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao inicializar banco: {ex.Message}");
    }
}

// ==========================================
// CONFIGURAÇÃO DO PIPELINE
// ==========================================

// Ativa o Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativa o CORS
app.UseCors("AllowAll");

// Mapeia os controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
