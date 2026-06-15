namespace Application.Interfaces;

public interface IUnitOfWork
{
    IUsuarioRepository Usuarios { get; }
    Task<int> GuardarCambiosAsync();
}
