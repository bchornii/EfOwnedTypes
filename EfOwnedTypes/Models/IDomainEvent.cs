using System;

namespace EfOwnedTypes.Models
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
