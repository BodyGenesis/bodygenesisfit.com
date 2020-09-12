using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using BodyGenesis.Core.Entities;
using BodyGenesis.Core.Entities.Queries;
using BodyGenesis.Shared;

namespace BodyGenesis.Infrastructure.MongoDB
{
    public class MongoDBRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDBRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task Delete(Guid id, bool permanent = false)
        {
            if (permanent)
            {
                await _collection.DeleteOneAsync(e => e.Id == id);
            }

            else
            {
                await _collection.UpdateOneAsync(e => e.Id == id, Builders<TEntity>.Update.Set(e => e.Deleted, true));
            }
        }

        public async Task<Maybe<TEntity>> Get(Guid id)
        {
            var result = await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();

            return Maybe<TEntity>.From(result);
        }

        public async Task<IReadOnlyCollection<TEntity>> Query(QueryBase<TEntity> query, int skip = 0, int take = int.MaxValue)
        {
            SortDefinition<TEntity> sort = null;

            if (query.SortExpression != null)
            {
                if (query.SortDirection == QuerySortDirection.Ascending)
                {
                    sort = Builders<TEntity>.Sort.Ascending(query.SortExpression);
                }

                else
                {
                    sort = Builders<TEntity>.Sort.Descending(query.SortExpression);
                }
            }

            return await _collection.Find(query.FilterExpression).Sort(sort).Skip(skip).Limit(take).ToListAsync();
        }

        public async Task Save(TEntity entity)
        {
            entity.PrepareForSave();

            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
        }
    }
}
