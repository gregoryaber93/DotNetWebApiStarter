using System;

namespace Domain.Common.Ids
{
    public class EntityId : ValueObject
    {
        public Guid Value { get; private set; }

        // Parameterless constructor for EF Core
        private EntityId() { }

        private EntityId(Guid value)
        {
            Value = value;
        }

        public static EntityId Create()
        {
            return new EntityId(Guid.NewGuid());
        }

        public static EntityId Create(Guid value)
        {
            return new EntityId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(EntityId id) => id.Value;
        public static implicit operator EntityId(Guid id) => Create(id);
    }
} 