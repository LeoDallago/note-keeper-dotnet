using FluentValidation;

namespace NoteKeeper.Dominio.ModuloNota;

public class ValidarNota : AbstractValidator<Nota>
{
    public ValidarNota()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("O título é obrigatório")
            .MinimumLength(3).WithMessage("O titulo deve conter no minimo 3 caracteres")
            .MaximumLength(30).WithMessage("O titulo deve conter no maximo 30 caracteres");
        
        RuleFor(x => x.Conteudo)
            .MaximumLength(100).WithMessage("o conteudo deve conter no maximo 100 caracteres");
    }
}