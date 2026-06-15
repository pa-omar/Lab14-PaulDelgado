using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<UsuarioResponse>> ObtenerTodosAsync()
    {
        var usuarios = await _unitOfWork.Usuarios.ObtenerTodosAsync();
        return usuarios.Select(Mapear).ToList();
    }

    public async Task<UsuarioResponse?> ObtenerPorIdAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);
        return usuario is null ? null : Mapear(usuario);
    }

    public async Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request)
    {
        var existe = await _unitOfWork.Usuarios.ObtenerPorCorreoAsync(request.Correo);

        if (existe is not null)
            throw new Exception("El correo ya está registrado.");

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Correo = request.Correo,
            Password = request.Password,
            Rol = request.Rol,
            CreadoEn = DateTime.UtcNow
        };

        await _unitOfWork.Usuarios.AgregarAsync(usuario);
        await _unitOfWork.GuardarCambiosAsync();

        return Mapear(usuario);
    }

    public async Task<UsuarioResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorCorreoAsync(request.Correo);

        if (usuario is null)
            return null;

        if (usuario.Password != request.Password)
            return null;

        return Mapear(usuario);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);

        if (usuario is null)
            return false;

        _unitOfWork.Usuarios.Eliminar(usuario);
        await _unitOfWork.GuardarCambiosAsync();

        return true;
    }

    private static UsuarioResponse Mapear(Usuario usuario)
    {
        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol,
            CreadoEn = usuario.CreadoEn
        };
    }
}
