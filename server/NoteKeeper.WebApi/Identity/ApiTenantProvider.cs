using System.Security.Claims;
using NoteKeeper.Dominio.ModuloAutenticacao;

namespace NoteKeeper.WebApi.Identity;

public class ApiTenantProvider : ITenantProvider
{
    public IHttpContextAccessor ContextAccessor;
    
    public ApiTenantProvider(IHttpContextAccessor contextAccessor)
    {
        ContextAccessor = contextAccessor;
    }
    public Guid? UsuarioId
    {
        get
        {
            var claimId = ContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claimId == null)
                return null;
            
            return Guid.Parse(claimId.Value);
        }
    }
}