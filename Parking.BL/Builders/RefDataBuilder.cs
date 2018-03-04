using System.Collections.Generic;
using CommonLib;
using Parking.Model;

namespace Parking.BL.Builders
{
	public class RefDataBuilder : IModelBuilder<string, IEnumerable<RefData>>
	{
		private readonly IRepository<RefData> _repository;

		public RefDataBuilder(IRepository<RefData> repository)
		{
			_repository = repository;
		}

		public IEnumerable<RefData> Build(string name)
		{
			return _repository.Matches(new Criteria<RefData>(x => x.RefDataType.Name == name));
		}
	}
}