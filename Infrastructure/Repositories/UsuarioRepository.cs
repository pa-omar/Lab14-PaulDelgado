using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly string _key;

    public UsuarioRepository(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _url = configuration["Supabase:Url"]!;
        _key = configuration["Supabase:Key"]!;

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("apikey", _key);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _key);
        _httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
    }

    public async Task<List<Usuario>> ObtenerTodosAsync()
    {
        var response = await _httpClient.GetAsync($"{_url}/rest/v1/usuarios?select=*&order=id.asc");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Usuario>>(json, OpcionesJson()) ?? new List<Usuario>();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_url}/rest/v1/usuarios?id=eq.{id}&select=*");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Usuario>>(json, OpcionesJson())?.FirstOrDefault();
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        var response = await _httpClient.GetAsync($"{_url}/rest/v1/usuarios?correo=eq.{correo}&select=*");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Usuario>>(json, OpcionesJson())?.FirstOrDefault();
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        var payload = new
        {
            usuario.Nombre,
            usuario.Correo,
            usuario.Password,
            usuario.Rol,
            usuario.CreadoEn
        };

        var response = await _httpClient.PostAsJsonAsync($"{_url}/rest/v1/usuarios", payload, OpcionesJson());
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var creado = JsonSerializer.Deserialize<List<Usuario>>(json, OpcionesJson())?.FirstOrDefault();

        if (creado is not null)
        {
            usuario.Id = creado.Id;
            usuario.CreadoEn = creado.CreadoEn;
        }
    }

    public void Actualizar(Usuario usuario)
    {
        var response = _httpClient.PatchAsJsonAsync(
            $"{_url}/rest/v1/usuarios?id=eq.{usuario.Id}",
            usuario,
            OpcionesJson()
        ).Result;

        response.EnsureSuccessStatusCode();
    }

    public void Eliminar(Usuario usuario)
    {
        var response = _httpClient.DeleteAsync($"{_url}/rest/v1/usuarios?id=eq.{usuario.Id}").Result;
        response.EnsureSuccessStatusCode();
    }

    private static JsonSerializerOptions OpcionesJson()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
}
