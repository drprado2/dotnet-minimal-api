namespace MinimalApi.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = null;
    // Used for sort desc/asc 
    public long InternalId { get; set; } = 0;
    public byte[]? DataVersion { get; set; }

    // Throws exception in case of invalid state
    // Only sync validation, I/O validation or validation that involves other entities must be applied by the use case.
    public abstract void Validate();
    
    public override bool Equals(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        
        return Equals(obj as Entity);
    }

    protected bool Equals(Entity other)
    {
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}