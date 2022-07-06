using NSE.Core.DomainObjects;

namespace NSE.Core.Data;

public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
{ }
