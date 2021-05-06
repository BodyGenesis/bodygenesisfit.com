using System;

namespace BodyGenesis.Core.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool Deleted { get; set; }

        public void PrepareForSave()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
                DateCreated = DateTime.Now;
            }
        }
    }
}
