using AutoMapper;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;

public class CategoriaProfile : Profile
{
   public CategoriaProfile()
   {
      CreateMap<Categoria, ListarCategoriaViewModel>();
      CreateMap<Categoria, VisualizarCategoriaViewModel>();
      
      CreateMap<InserirCategoriaViewModel, Categoria>();
      CreateMap<EditarCategoriaViewModel, Categoria>();

      
   }
}