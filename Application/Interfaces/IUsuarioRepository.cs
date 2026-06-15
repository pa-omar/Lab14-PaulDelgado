using Domain.Entities;

namespace Application.Interfaces;

public interface IUsuarioRepository
{
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    void Actualizar(Usuario usuario);
    void Eliminar(Usuario usuario);
}
