using System.Data.Entity;

namespace CommonLib
{
    public abstract class Repository<TObjectContext, TEntity>
        where TObjectContext : DbContext
        where TEntity : class
    {
        protected Repository(TObjectContext tobjectContext)
        {
            DataEntities = tobjectContext;
            Entity = DataEntities.Set<TEntity>();
        }

        public DbSet<TEntity> Entity { get; }

        public TObjectContext DataEntities { get; }
    }
}