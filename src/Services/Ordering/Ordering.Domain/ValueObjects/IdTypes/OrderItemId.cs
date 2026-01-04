namespace Ordering.Domain.ValueObjects.IdTypes;

public record OrderItemId
{
    public Guid Value { get; }
    private OrderItemId(Guid value) => Value = value;
    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException($"{nameof(value)} cannot be empty.");
        }
        return new OrderItemId(value);
    }
}
