﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NoteKeeper.Aplicacao.ModuloAutenticacao;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.Infra.Orm.Compartilhado;

namespace NoteKeeper.WebApi.Identity;

public static class IdentityDependecyInjection
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddScoped<ServicoAutenticacao>();
        services.AddScoped<JsonWebTokenProvider>();
        services.AddScoped<ITenantProvider, ApiTenantProvider>();
        
        services.AddIdentity<Usuario, Cargo>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<NoteKeeperDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration config)
    {
        var chaveAssinaturaJwt = config["JWT_GENERATION_KEY"];

        if (chaveAssinaturaJwt == null)
            throw new ArgumentException("Nao foi possivel obter a chave de assinatura de token");
        
        var chaveEmBytes = Encoding.ASCII.GetBytes(chaveAssinaturaJwt);
        
        var audienciaValida = config["JWT_AUDIENCE_DOMAIN"];
        
        if (audienciaValida == null)
            throw new ArgumentException("Nao foi possivel obter o dominio da audiencia");
        
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;

            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(chaveEmBytes),
                ValidAudience = audienciaValida,
                ValidIssuer = "NoteKeeper",
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true
            };
        });
    }
}