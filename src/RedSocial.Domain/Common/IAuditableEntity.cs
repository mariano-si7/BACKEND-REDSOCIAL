namespace RedSocial.Domain.Common;

/// <summary>
/// Interfaz para entidades que rastrean fecha de modificacion y trazabilidad por token/usuario.
/// </summary>
public interface IAuditableEntity
{
    DateTime? FechaActualizacion { get; set; }
    int? ModificadoPorUsuarioId { get; set; }
    Guid? ModificadoPorTokenId { get; set; }
}
