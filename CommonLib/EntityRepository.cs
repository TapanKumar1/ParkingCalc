using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Migrations;

namespace CommonLib
{
   public abstract class EntityRepository<TObjectContext, TEntity> : Repository<TObjectContext, TEntity>, IRepository<TEntity>, ICriteriaQuery<TEntity>
      where TObjectContext : DbContext
      where TEntity : class
   {

      protected EntityRepository(TObjectContext objectContext)
         : base(objectContext)
      {
      }

      public TEntity Get(ICriteria<TEntity> criteria)
      {
         return Matches(criteria).SingleOrDefault();
      }

      public IQueryable<TEntity> Matches(ICriteria<TEntity> criteria)
      {
         return criteria.Build(this);
      }

      public void Delete(TEntity entity)
      {
         Entity.Remove(entity);
      }

      public void Delete(ICriteria<TEntity> criteria)
      {
         foreach (var entity in Matches(criteria))
         {
            Delete(entity);
         }
      }

      public void Save(TEntity entity)
      {
         Entity.AddOrUpdate(entity);
      }

      public void SaveChanges()
      {
         DataEntities.SaveChanges();
      }

      public int Count(ICriteria<TEntity> criteria)
      {
         return Matches(criteria).Count();
      }
    }
}