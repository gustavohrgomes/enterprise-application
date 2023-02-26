using NSE.Core.Messages;

namespace NSE.Core.DomainObjects;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }

    private List<DomainEvent>? _eventos;
    public IReadOnlyCollection<DomainEvent> Eventos => _eventos?.AsReadOnly()!;

    public void AdicionarEvento(DomainEvent @evento)
    {
        _eventos ??= new List<DomainEvent>();
        _eventos.Add(@evento);
    }

    public void RemoverEvento(DomainEvent @evento) => _eventos?.Remove(evento);

    public void LimparEventos() => _eventos?.Clear();

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}
