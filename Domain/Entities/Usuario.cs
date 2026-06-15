using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Usuario
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("rol")]
    public string Rol { get; set; } = "Usuario";

    [JsonPropertyName("creado_en")]
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}
