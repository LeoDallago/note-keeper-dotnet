using FluentResults;
using Microsoft.AspNetCore.Identity;
using NoteKeeper.Dominio.ModuloAutenticacao;

namespace NoteKeeper.Aplicacao.ModuloAutenticacao;

public class ServicoAutenticacao
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public ServicoAutenticacao(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<Usuario>> RegistrarAsync(Usuario usuario, string senha)
    {
      var usuarioResult = await _userManager.CreateAsync(usuario, senha);

      if (!usuarioResult.Succeeded)
      {
          return Result
              .Fail(usuarioResult
                  .Errors
                  .Select(failure => new Error(failure.Description))
              );
      }
      
      return Result.Ok(usuario);
    }
    
    public async Task<Result<Usuario>> Autenticar(string login, string senha)
    {
        var loginResult = await _signInManager.PasswordSignInAsync(login, senha, false, true);
        var erros = new List<IError>();
        if (loginResult.IsLockedOut)
            erros.Add(new Error("O acesso para este usuário foi bloqueado"));
        if (loginResult.IsNotAllowed)
            erros.Add(new Error("O acesso para este usuário é permitido"));
        if (!loginResult.Succeeded)
            erros.Add(new Error("O login ou a senha estão incorretas"));
        if (erros.Count > 0)
            return Result.Fail(erros);
        var usuario = await _userManager.FindByNameAsync(login);
        if (usuario == null)
            return Result.Fail("Não foi possível encontrar o usuário");
        return Result.Ok(usuario);
    }
    
    public async Task<Result<Usuario>> Sair()
    {
        await _signInManager.SignOutAsync();
        return Result.Ok();
    }
    
}
