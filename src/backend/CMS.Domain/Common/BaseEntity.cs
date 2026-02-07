namespace CMS.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    // Constructor for creating NEW entities or loading EXISTING ones
    protected BaseEntity(Guid id)
    {
        // If the ID is empty (default), generate a new one
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }

    // Required for EF Core to materialize objects from the database
    protected BaseEntity() 
    { 
        Id = Guid.NewGuid();
    }
}