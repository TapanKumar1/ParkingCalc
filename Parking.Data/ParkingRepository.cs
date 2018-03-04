using CommonLib;

namespace Parking.Data
{
	public class ParkingRepository<TEntity>: EntityRepository<DbConnection, TEntity> where TEntity : class
	{
		public ParkingRepository(DbConnection objectContext) : base(objectContext)
		{
		}
	}
}