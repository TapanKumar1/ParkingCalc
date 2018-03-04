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
	public class WeekendRateCalculatorTests
	{
		private readonly ICalculator<WeekendRateCalculator> _weekendRateCalculator;
		private readonly Mock<IRepository<RefData>> _mockRepository;
		private const string WeekendRate = "Weekend Rate";
		private List<RefData> _refDatas;

		public WeekendRateCalculatorTests()
		{
			_mockRepository = new Mock<IRepository<RefData>>();
			var refDataBuilder = new RefDataBuilder(_mockRepository.Object);
			var mockWeekendRateCalculator = new Mock<WeekendRateCalculator>(refDataBuilder);
			_weekendRateCalculator = mockWeekendRateCalculator.Object;
		}

		[TestInitialize]
		public void Setup()
		{
			_refDatas = new List<RefData>
			{
				new RefData {Amount = 10, RefDataTypeId = 3, Name = "Flat Rate", RefDataType = new RefDataType
				{
					Name = WeekendRate, Id = 3,
					DaysAllowed = DaysAllowed.Weekends,
					EntryStartHours = 0, EntryStartMinutes = 1, EntryEndHours = 23,  EntryEndMinutes = 59,
					ExitStartHours = 0, ExitStartMinutes = 0, ExitEndHours = 23, ExitEndMinutes = 59
				}}
			};
			_mockRepository.Setup(x => x.Matches(It.IsAny<ICriteria<RefData>>())).Returns(_refDatas.AsQueryable());
		}

		[TestMethod]
		public void GivenEntryIsOnWeekendTheParkingRateAmountShouldBe10()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 5, 0, 1, 0),
				ExitTime = new DateTime(1981, 12, 6, 23, 59, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 10, Name = WeekendRate };

			//Act
			var actualRate = _weekendRateCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}


		[TestMethod]
		public void GivenEntryIsOnFridayBeforeMidnightAndExitOnSaturdayMorningParkingRateAmountShouldBeZero()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 4, 23, 59, 0),
				ExitTime = new DateTime(1981, 12, 5, 5, 59, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 0.0m, Name = WeekendRate };

			//Act
			var actualRate = _weekendRateCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}
	}
}