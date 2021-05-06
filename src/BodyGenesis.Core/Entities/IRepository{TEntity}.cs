using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        Task<IReadOnlyCollection<TEntity>> Query(Expression<Func<TEntity, bool>> filterExpression, int skip = 0, int take = int.MaxValue, Expression<Func<TEntity, object>> sortExpression = null, QuerySortDirection sortDirection = QuerySortDirection.Ascending);
        Task Save(TEntity entity);
    }
}
