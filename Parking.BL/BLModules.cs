using System.Collections.Generic;
using CommonLib;
using Parking.BL.Builders;
using Parking.Data;
using Parking.Model;
using SimpleInjector;

namespace Parking.BL
{
	public class BLModules
	{
		public static void Register(Container container)
		{
			container.Register(typeof(IRepository<>), typeof(ParkingRepository<>));
			container.Register(typeof(ICriteria<>), typeof(Criteria<>));

			#region Builders
			container.Register<IModelBuilder<string, IEnumerable<RefData>>, RefDataBuilder>(Lifestyle.Transient);
			#endregion

		}
	}
}