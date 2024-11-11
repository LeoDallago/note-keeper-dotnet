using FluentValidation;

namespace NoteKeeper.Dominio.ModuloCategoria;

public class ValidadorCategoria : AbstractValidator<Categoria>
{
    public ValidadorCategoria()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("O titulo é obrigatorio!!")
            .MinimumLength(3).WithMessage("o titulo deve conter no minimo 3 caracteres")
            .MaximumLength(30).WithMessage("o titulo deve conter ao maximo 30 caracteres");
    }
}