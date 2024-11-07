using AutoMapper;
using NoteKeeper.Dominio.ModuloNota;

public class NotaProfile : Profile
{
    public NotaProfile()
    {
        CreateMap<Nota, ListarNotaViewModel>();
        CreateMap<Nota, VisualizarNotaViewModel>();

        CreateMap<FormsNotaViewModel, Nota>()
            .AfterMap<ConfigurarCategoriaMappingAction>();
    }
}