using AutoMapper;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;

public class ConfigurarCategoriaMappingAction(IRepositorioCategoria repositorioCategoria)
    : IMappingAction<FormsNotaViewModel, Nota>
{
    public void Process(FormsNotaViewModel source, Nota destination, ResolutionContext context)
    {
        var idCategoria = source.CategoriaId;
        
        destination.Categoria = repositorioCategoria.SelecionarPorId(idCategoria);
    }
}