using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloAutenticacao;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.WebApi.Identity;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly ServicoAutenticacao _servicoAutenticacao;
    private readonly JsonWebTokenProvider _jsonWebTokenProvider;
    private readonly IMapper _mapper;

    public AutenticacaoController(
        ServicoAutenticacao servicoAutenticacao,
        JsonWebTokenProvider jsonWebTokenProvider,
        IMapper mapper
        )
    {
        _servicoAutenticacao = servicoAutenticacao;
        _jsonWebTokenProvider = jsonWebTokenProvider;
        _mapper = mapper;
        
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel viewModel)
    {
        var usuario = _mapper.Map<Usuario>(viewModel);
        var usuarioResult = await _servicoAutenticacao.RegistrarAsync(usuario, viewModel.Password);

        if (usuarioResult.IsFailed)
            return BadRequest(usuarioResult.Errors);
        
        var tokenViewModel = _jsonWebTokenProvider.GerarTokenAcesso(usuario);

        return Ok(tokenViewModel);
    }
    
    [HttpPost("autenticar")]
    public async Task<IActionResult> Autenticar(AutenticarUsuarioViewModel viewModel)
    {
        var usuarioResult = await _servicoAutenticacao.Autenticar(viewModel.UserName,
            viewModel.Password);
        
        if (usuarioResult.IsFailed)
            return BadRequest(usuarioResult.Errors);
        
        var usuario = usuarioResult.Value;
        
        var tokenViewModel = _jsonWebTokenProvider.GerarTokenAcesso(usuario);
        
        return Ok(tokenViewModel);
    }

    [HttpPost("sair")]
    public async Task<IActionResult> Sair()
    {
        await _servicoAutenticacao.Sair();
        return Ok();
    }
}