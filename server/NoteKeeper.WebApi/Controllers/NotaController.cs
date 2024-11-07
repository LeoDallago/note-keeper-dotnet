using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.ModuloNota;

namespace NoteKeeper.WebApi.Controllers
{
    [Route("api/notas")]
    [ApiController]
    public class NotaController(ServicoNota servicoNota, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult>Get()
        {
            var notasResult  = await servicoNota.SelecionarTodosAsync();

            if (notasResult.IsFailed)
                return StatusCode(500);
            
            var viewModel = mapper.Map<ListarNotaViewModel[]>(notasResult.Value);
            
            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(Guid id)
        {
            var notaResult =  await servicoNota.SelecionarPorIdAsync(id);
            
            if (notaResult.IsFailed)
                return StatusCode(500);
          
            else if (notaResult.IsSuccess && notaResult.Value is null)
                return NotFound(notaResult.Errors);
            
            var viewModel = mapper.Map<VisualizarNotaViewModel>(notaResult.Value);
            
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InserirNotaViewModel inserirNotaVm)
        {
            var nota = mapper.Map<Nota>(inserirNotaVm);

            var notaResult = await servicoNota.InserirAsync(nota);

            if (notaResult.IsFailed)
                return BadRequest(notaResult.Errors);
            
            return Ok(inserirNotaVm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, EditarNotaViewModel editarNotaVm)
        {
            var notaResult = await servicoNota.SelecionarPorIdAsync(id);
            
            if (notaResult.IsFailed)
                return StatusCode(500);
          
            else if (notaResult.IsSuccess && notaResult.Value is null)
                return NotFound(notaResult.Errors);

            var notaEditada = mapper.Map(editarNotaVm, notaResult.Value);
            
            var edicaoResult = await servicoNota.EditarAsync(notaEditada);

            if (edicaoResult.IsFailed)
                BadRequest(notaResult.Errors);
            
            return Ok(editarNotaVm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var resultado = await servicoNota.ExcluirAsync(id);
            
            if(resultado.IsFailed)
                return BadRequest(resultado.Errors);
            
            return Ok();
        }
        
    }
}
