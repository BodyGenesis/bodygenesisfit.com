using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BodyGenesis.Core.Entities.Queries;
using BodyGenesis.Shared;

namespace BodyGenesis.Core.Entities
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        Task Delete(Guid id, bool permanent = false);
        Task<Maybe<TEntity>> Get(Guid id);
        Task<IReadOnlyCollection<TEntity>> Query(QueryBase<TEntity> query, int skip = 0, int take = int.MaxValue);
        Task Save(TEntity entity);
    }
}
