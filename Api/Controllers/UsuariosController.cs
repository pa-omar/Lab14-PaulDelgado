using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario is null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(usuario);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(CrearUsuarioRequest request)
    {
        try
        {
            var usuario = await _usuarioService.CrearAsync(request);
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var usuario = await _usuarioService.LoginAsync(request);

        if (usuario is null)
            return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });

        return Ok(new
        {
            mensaje = "Inicio de sesión correcto.",
            usuario
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _usuarioService.EliminarAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(new { mensaje = "Usuario eliminado correctamente." });
    }
}
