namespace RedSocial.Domain.Common;

/// <summary>
/// Interfaz para entidades que rastrean fecha de modificacion/auditoria.
/// </summary>
public interface IAuditableEntity
{
    DateTime? FechaActualizacion { get; set; }
}
