using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.ModuloCategoria;
using Serilog;

namespace NoteKeeper.WebApi.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    [Authorize]
    public class CategoriaController(ServicoCategoria servicoCategoria, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultado = await servicoCategoria.SelecionarTodosAsync();
            

            var viewModel = mapper.Map<ListarCategoriaViewModel[]>(resultado.Value);

            Log.Information("Foram Selecionados {QuantidadeRegistros} Registros", viewModel.Count());
            
            return Ok(viewModel);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
          var categoriaResult = await servicoCategoria.SelecionarPorIdAsync(id);

          if (categoriaResult.IsFailed)
              return StatusCode(500);
          
          else if (categoriaResult.IsSuccess && categoriaResult.Value is null)
              return NotFound(categoriaResult.Errors);

          var viewModel = mapper.Map<VisualizarCategoriaViewModel>(categoriaResult.Value);

          return Ok(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(InserirCategoriaViewModel categoriaVm)
        { 
            var categoria = mapper.Map<Categoria>(categoriaVm);
         var resultado = await servicoCategoria.InserirAsync(categoria);
         
         if(resultado.IsFailed)
             return BadRequest(resultado.Errors);
         
         return Ok(categoriaVm);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id,EditarCategoriaViewModel categoriaEditadaVm)
        {
            var categoriaSecionada = await servicoCategoria.SelecionarPorIdAsync(id);
            if (categoriaSecionada.IsFailed)
                return NotFound(categoriaSecionada.Errors);
            
            var categoria = mapper.Map(categoriaEditadaVm, categoriaSecionada.Value);
            var resultado = await servicoCategoria.EditarAsync(categoria);

            if (resultado.IsFailed)
                return BadRequest(resultado.Errors);
            
            return Ok(resultado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
           var resultado = await servicoCategoria.ExcluirAsync(id);
            
            if(resultado.IsFailed)
                return BadRequest(resultado.Errors);
            
            return Ok();
        }
    }
}
