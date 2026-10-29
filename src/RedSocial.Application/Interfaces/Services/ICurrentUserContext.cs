namespace RedSocial.Application.Interfaces.Services;

/// <summary>
/// Provee acceso a la identidad y token de sesion del usuario autenticado en la peticion actual.
/// </summary>
public interface ICurrentUserContext
{
    int? UsuarioId { get; }
    Guid? TokenId { get; }
    string? NombreUsuario { get; }
    bool EstaAutenticado { get; }
}
