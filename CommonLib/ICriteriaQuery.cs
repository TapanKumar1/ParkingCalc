using System.Data.Entity;

namespace CommonLib
{
    public interface ICriteriaQuery<TEntity> where TEntity : class
    {
        DbSet<TEntity> Entity { get; }
    }
}