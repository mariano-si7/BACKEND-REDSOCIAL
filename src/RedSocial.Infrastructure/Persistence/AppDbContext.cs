using Microsoft.EntityFrameworkCore;
using RedSocial.Application.Interfaces.Services;
using RedSocial.Domain.Common;
using RedSocial.Domain.Entities;

namespace RedSocial.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserContext? _currentUserContext;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserContext? currentUserContext = null) : base(options)
    {
        _currentUserContext = currentUserContext;
    }

    public DbSet<LogAcceso> LogsAcceso => Set<LogAcceso>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
    public DbSet<PerfilUsuario> PerfilesUsuario => Set<PerfilUsuario>();
    public DbSet<Seguidor> Seguidores => Set<Seguidor>();
    public DbSet<Momento> Momentos => Set<Momento>();
    public DbSet<Reporte> Reportes => Set<Reporte>();
    public DbSet<ModeracionAccion> ModeracionAcciones => Set<ModeracionAccion>();
    public DbSet<AdvertenciaUsuario> AdvertenciasUsuario => Set<AdvertenciaUsuario>();
    public DbSet<MomentoMeGusta> MeGustaMomentos => Set<MomentoMeGusta>();
    public DbSet<HistorialCambioPerfil> HistorialCambiosPerfil => Set<HistorialCambioPerfil>();
    public DbSet<VerificacionEmail> VerificacionesEmail => Set<VerificacionEmail>();
    public DbSet<RecuperacionPassword> RecuperacionesPassword => Set<RecuperacionPassword>();
    public DbSet<Conversacion> Conversaciones => Set<Conversacion>();
    public DbSet<MensajeChat> MensajesChat => Set<MensajeChat>();
    public DbSet<ComentarioMomento> ComentariosMomentos => Set<ComentarioMomento>();
    public DbSet<ComentarioMomentoMeGusta> MeGustaComentariosMomentos => Set<ComentarioMomentoMeGusta>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AplicarTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        AplicarTimestamps();
        return base.SaveChanges();
    }

    private void AplicarTimestamps()
    {
        var ahora = DateTime.UtcNow;
        var usuarioId = _currentUserContext?.UsuarioId;
        var tokenId = _currentUserContext?.TokenId;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                var propCreacion = entry.Properties.FirstOrDefault(p =>
                    p.Metadata.Name is "FechaCreacion" or "FechaRegistro" or "FechaEnvio" or "FechaSeguimiento");

                if (propCreacion != null && (propCreacion.CurrentValue is null || (DateTime)propCreacion.CurrentValue == default))
                {
                    propCreacion.CurrentValue = ahora;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IAuditableEntity auditable)
                {
                    auditable.FechaActualizacion = ahora;
                    if (usuarioId.HasValue) auditable.ModificadoPorUsuarioId = usuarioId.Value;
                    if (tokenId.HasValue) auditable.ModificadoPorTokenId = tokenId.Value;
                }
                else
                {
                    var propActualizacion = entry.Properties.FirstOrDefault(p =>
                        p.Metadata.Name == "FechaActualizacion");

                    if (propActualizacion != null)
                    {
                        propActualizacion.CurrentValue = ahora;
                    }
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
