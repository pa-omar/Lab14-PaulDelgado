using Application.Interfaces;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IUsuarioRepository usuarios)
    {
        Usuarios = usuarios;
    }

    public IUsuarioRepository Usuarios { get; }

    public Task<int> GuardarCambiosAsync()
    {
        return Task.FromResult(1);
    }
}
