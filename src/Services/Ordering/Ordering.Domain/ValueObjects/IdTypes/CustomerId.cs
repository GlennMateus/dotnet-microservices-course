namespace Ordering.Domain.ValueObjects.IdTypes;

public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;
    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException($"{nameof(value)} cannot be empty.");
        }

        return new CustomerId(value);
    }
}
