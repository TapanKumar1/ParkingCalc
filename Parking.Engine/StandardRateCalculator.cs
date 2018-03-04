using System;
using System.Collections.Generic;
using System.Linq;
using Parking.BL;
using Parking.Model;

namespace Parking.Engine
{
	public class StandardRateCalculator : ICalculator<StandardRateCalculator>
	{
		private readonly IModelBuilder<string, IEnumerable<RefData>> _refDataBuilder;

		public StandardRateCalculator(IModelBuilder<string, IEnumerable<RefData>> refDataBuilder )
		{
			_refDataBuilder = refDataBuilder;
		}

		public IRate Calculate(TimeSlot timeSlot)
		{
			var duration = CalculateDuration(timeSlot);
			var refDatas = _refDataBuilder.Build("Standard Rate").ToList();
			if (!refDatas.Any()) return GetRate(timeSlot, 0);
			var refData = refDatas.Where(x=> x.Duration == duration || duration > x.Duration).OrderByDescending(x=>x.Duration).FirstOrDefault();
			var durationInDays = Math.Ceiling(Convert.ToDecimal((timeSlot.ExitTime - timeSlot.EntryTime).TotalDays));
			return GetRate(timeSlot, (refData?.Amount * durationInDays) ?? 0);
		}

		private static int CalculateDuration(TimeSlot timeSlot)
		{
			var durationInMinutes = (timeSlot.ExitTime - timeSlot.EntryTime).TotalMinutes;
			var durationInHours = (int) Math.Ceiling((decimal)durationInMinutes / 60);
			return durationInHours;
		}

		public IRate GetRate(TimeSlot timeSlot, decimal amount)
		{
			return new Rate { TimeSlot = timeSlot, ParkingRate = new ParkingRate { Amount = amount, Name = "Standard Rate" } };
		}
	}
}