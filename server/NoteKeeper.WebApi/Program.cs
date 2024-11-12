using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config;
using NoteKeeper.WebApi.Filters;
using Serilog;

const string politicaCors = "_minhaPoliticaCors";

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(connectionString, dbOptions =>
    {
        dbOptions.EnableRetryOnFailure();
    });
});

builder.Services.AddScoped<IRepositorioCategoria,RepositorioCategoriaOrm>();
builder.Services.AddScoped<ServicoCategoria>();

builder.Services.AddScoped<IRepositorioNota,RepositorioNotaOrm>();
builder.Services.AddScoped<ServicoNota>();

builder.Services.AddScoped<ConfigurarCategoriaMappingAction>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<NotaProfile>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: politicaCors, policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseWrapperFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSerilog(builder.Logging);

var app = builder.Build();

app.UseGlobalExceptionHandler();

    app.UseSwagger();
    app.UseSwaggerUI();

var migracaoConcluida = app.AutoMigrateDataBase();

if (migracaoConcluida) Log.Information("Migracao do banco de dados concluida");
else Log.Information("Nenhuma migracao de banco de dados pendente");

app.UseHttpsRedirection();

app.UseCors(politicaCors);

app.UseAuthorization();

app.MapControllers();

app.Run();