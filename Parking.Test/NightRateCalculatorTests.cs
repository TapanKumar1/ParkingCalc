using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Parking.BL.Builders;
using Parking.Engine;
using Parking.Model;

namespace Parking.Test
{
	[TestClass]
	public class NightRateCalculatorTests
	{
		private readonly ICalculator<NightRateCalculator> _nightRateCalculator;
		private readonly Mock<IRepository<RefData>> _mockRepository;
		private const string NightRate = "Night Rate";
		private List<RefData> _refDatas;

		public NightRateCalculatorTests()
		{
			_mockRepository = new Mock<IRepository<RefData>>();
			var refDataBuilder = new RefDataBuilder(_mockRepository.Object);
			var mockNightRateCalculator = new Mock<NightRateCalculator>(refDataBuilder);
			_nightRateCalculator = mockNightRateCalculator.Object;
		}

		[TestInitialize]
		public void Setup()
		{
			_refDatas = new List<RefData>
			{
				new RefData {
							Amount = 6.50m, RefDataTypeId = 2, Name = NightRate, RefDataType = new RefDataType { Name = NightRate,
							DaysAllowed = DaysAllowed.Weekdays,
							EntryStartHours = 18, EntryStartMinutes = 0, EntryEndHours = 23,  EntryEndMinutes = 59,
							ExitStartHours = 0, ExitStartMinutes = 1, ExitEndHours = 6, ExitEndMinutes = 0}
							}
			};
			_mockRepository.Setup(x => x.Matches(It.IsAny<ICriteria<RefData>>())).Returns(_refDatas.AsQueryable());
		}

		[TestMethod]
		public void WhenEntryIsBetween6PmAndMidnightAndExitIsBefore6AmFollowingDayParkingRateAmountShouldBe650()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 3, 18, 0, 0),
				ExitTime = new DateTime(1981, 12, 4, 5, 59, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 6.50m, Name = NightRate };

			//Act
			var actualRate = _nightRateCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}


		[TestMethod]
		public void WhenEntryIsOnWeekendTheAmountShouldBeZero()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 5, 18, 0, 0),
				ExitTime = new DateTime(1981, 12, 6, 5, 59, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 0.0m, Name = NightRate };

			//Act
			var actualRate = _nightRateCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}


		[TestMethod]
		public void WhenEntryIsOutSideNightRateOnAWeekdayTheAmountShouldBeZero()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 4, 9, 0, 0),
				ExitTime = new DateTime(1981, 12, 4, 18, 30, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 0.0m, Name = NightRate };

			//Act
			var actualRate = _nightRateCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}
	}
}