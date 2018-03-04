using System.Linq;

namespace CommonLib
{
    public interface ICriteria<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Build(ICriteriaQuery<TEntity> entity);
    }
}