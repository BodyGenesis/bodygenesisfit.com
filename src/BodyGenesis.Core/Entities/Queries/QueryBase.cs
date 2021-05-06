using System;
using System.Linq.Expressions;

namespace BodyGenesis.Core.Entities.Queries
{
    public abstract class QueryBase<TEntity>
        where TEntity : EntityBase
    {
        public Expression<Func<TEntity, bool>> FilterExpression { get; protected set; } = e => true;
        public Expression<Func<TEntity, object>> SortExpression { get; protected set; }
        public QuerySortDirection SortDirection { get; protected set; }
    }
} 
