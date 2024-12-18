using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi;
using NoteKeeper.WebApi.Config;
using NoteKeeper.WebApi.Filters;
using NoteKeeper.WebApi.Identity;
using Serilog;

const string politicaCors = "_minhaPoliticaCors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDbContext(builder.Configuration);

builder.Services.ConfigureCoreServices();

builder.Services.ConfigureAutoMapper();

builder.Services.ConfigureCors(politicaCors);

builder.Services.ConfigureControllersWithFilters();

builder.Services.ConfigureSwaggerAuthorization();

builder.Services.ConfigureSerilog(builder.Logging, builder.Configuration);

builder.Services.ConfigureIdentity();

builder.Services.ConfigureJwt(builder.Configuration);

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

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal("Ocorreu um erro que ocasionou o fechamendo da aplicacao", ex);
    return;
}