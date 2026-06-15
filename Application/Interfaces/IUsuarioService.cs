using Application.DTOs;

namespace Application.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioResponse>> ObtenerTodosAsync();
    Task<UsuarioResponse?> ObtenerPorIdAsync(int id);
    Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request);
    Task<UsuarioResponse?> LoginAsync(LoginRequest request);
    Task<bool> EliminarAsync(int id);
}
