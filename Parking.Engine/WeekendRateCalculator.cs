using System;
using System.Collections.Generic;
using System.Linq;
using Parking.BL;
using Parking.Model;
using Parking.Utils;

namespace Parking.Engine
{
	public class WeekendRateCalculator : ICalculator<WeekendRateCalculator>
	{
		private readonly IModelBuilder<string, IEnumerable<RefData>> _refDataBuilder;
		private const string WeekendRate = "Weekend Rate";

		public WeekendRateCalculator(IModelBuilder<string, IEnumerable<RefData>> refDataBuilder)
		{
			_refDataBuilder = refDataBuilder;
		}
		
		public IRate Calculate(TimeSlot timeSlot)
		{
			var refDatas = _refDataBuilder.Build(WeekendRate).ToList();

			if (!refDatas.Any()) return GetRate(timeSlot, 0);

			var refData = refDatas.FirstOrDefault();

			if (refData.RefDataType.DaysAllowed == DaysAllowed.Weekends && timeSlot.EntryTime.IsWeekday()) return GetRate(timeSlot, 0);

			if (timeSlot.EntryTime >= new DateTime(timeSlot.EntryTime.Year, timeSlot.EntryTime.Month, timeSlot.EntryTime.Day, refData.RefDataType.EntryStartHours, refData.RefDataType.EntryStartMinutes, 0)
			&& timeSlot.EntryTime <= new DateTime(timeSlot.EntryTime.Year, timeSlot.EntryTime.Month, timeSlot.EntryTime.AddDays(1).Day, refData.RefDataType.EntryEndHours, refData.RefDataType.EntryEndMinutes, 0)
			&& timeSlot.ExitTime >= new DateTime(timeSlot.ExitTime.Year, timeSlot.ExitTime.Month, timeSlot.ExitTime.Day, refData.RefDataType.ExitStartHours, refData.RefDataType.ExitStartMinutes, 0)
			&& timeSlot.ExitTime <= new DateTime(timeSlot.ExitTime.Year, timeSlot.ExitTime.Month, timeSlot.EntryTime.AddDays(1).Day, refData.RefDataType.ExitEndHours, refData.RefDataType.ExitEndMinutes, 0))
			{

				return GetRate(timeSlot, refData?.Amount ?? 0);
			}

			return GetRate(timeSlot, 0);
		}

		public IRate GetRate(TimeSlot timeSlot, decimal amount)
		{
			return new Rate { TimeSlot = timeSlot, ParkingRate = new ParkingRate { Amount = amount, Name = WeekendRate } };
		}
	}
}