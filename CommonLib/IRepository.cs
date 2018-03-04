using System.Linq;

namespace CommonLib
{
   public interface IRepository<TEntity> where TEntity : class
   {
      TEntity Get(ICriteria<TEntity> criteria);
      IQueryable<TEntity> Matches(ICriteria<TEntity> criteria);
      void Delete(TEntity entity);
      void Delete(ICriteria<TEntity> criteria);
      void Save(TEntity entity);
      void SaveChanges();
      int Count(ICriteria<TEntity> criteria);
   }
}
