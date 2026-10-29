using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RedSocial.Application.Interfaces.Services;

namespace RedSocial.API.Services;

public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public int? UsuarioId
    {
        get
        {
            var valor = User?.FindFirst("sub")?.Value ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(valor, out var id) ? id : null;
        }
    }

    public Guid? TokenId
    {
        get
        {
            var valor = User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? User?.FindFirst("jti")?.Value;
            return Guid.TryParse(valor, out var guid) ? guid : null;
        }
    }

    public string? NombreUsuario => User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value ?? User?.FindFirst(ClaimTypes.Name)?.Value;

    public bool EstaAutenticado => User?.Identity?.IsAuthenticated == true;
}
