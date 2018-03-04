using System;
using System.Collections.Generic;
using System.Linq;
using Parking.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Parking.BL;
using Parking.Engine;

namespace Parking.Test
{
	[TestClass]
	public class ParkingCalculatorTests
	{
		private readonly ParkingCalculator _calculator;
		private readonly Mock<IModelBuilder<string, IEnumerable<RefData>>> _mockRefDataBuilder;
		private const string StandardRate = "Standard Rate";
		private const string EarlyBirdRate = "Early Bird Rate";
		private const string NightRate = "Night Rate";
		private const string WeekendRate = "Weekend Rate";
		private List<RefData> _refDatas;

		public ParkingCalculatorTests()
		{
			_mockRefDataBuilder = new Mock<IModelBuilder<string, IEnumerable<RefData>>>();
			var refDataBuilder = _mockRefDataBuilder.Object;
			var earlyBirdCalculator = new EarlyBirdCalculator(refDataBuilder);
			var standardRateCalculator = new StandardRateCalculator(refDataBuilder);
			var nightRateCalculator = new NightRateCalculator(refDataBuilder);
			var weekendRateCalculator = new WeekendRateCalculator(refDataBuilder);
			var mockParkingCalculator = new Mock<ParkingCalculator>(earlyBirdCalculator, standardRateCalculator, nightRateCalculator, weekendRateCalculator);
			_calculator = mockParkingCalculator.Object;
		}

		[TestInitialize]
		public void Setup()
		{
			_refDatas = new List<RefData>
			{
				new RefData {Amount = 5, RefDataTypeId = 4, Name = "Flat Rate", Duration = 1, RefDataType = new RefDataType { Name = StandardRate, Id = 4 }},
				new RefData {Amount = 10, RefDataTypeId = 4, Name = "Flat Rate", Duration = 2, RefDataType = new RefDataType { Name = StandardRate, Id = 4 }},
				new RefData {Amount = 15, RefDataTypeId = 4, Name = "Flat Rate", Duration = 3, RefDataType = new RefDataType { Name = StandardRate, Id = 4 }},
				new RefData {Amount = 20, RefDataTypeId = 4, Name = "Flat Rate", Duration = 3.1, RefDataType = new RefDataType { Name = StandardRate, Id = 4 }},
				new RefData {Amount = 13, RefDataTypeId = 1, Name = "Flat Rate", RefDataType = new RefDataType
				{
					Name = EarlyBirdRate, Id = 1,
					DaysAllowed = DaysAllowed.Weekdays,
					EntryStartHours = 6, EntryStartMinutes = 0, EntryEndHours = 9,  EntryEndMinutes = 0,
					ExitStartHours = 15, ExitStartMinutes = 30, ExitEndHours = 23, ExitEndMinutes = 30

				}},
				new RefData {Amount = 6.50m, RefDataTypeId = 2, Name = "Flat Rate", RefDataType = new RefDataType
				{
					Name = NightRate, Id = 2,
					DaysAllowed = DaysAllowed.Weekdays,
					EntryStartHours = 18, EntryStartMinutes = 0, EntryEndHours = 23,  EntryEndMinutes = 59,
					ExitStartHours = 0, ExitStartMinutes = 1, ExitEndHours = 6, ExitEndMinutes = 0
				}},
				new RefData {Amount = 10, RefDataTypeId = 3, Name = "Flat Rate", RefDataType = new RefDataType
				{
					Name = WeekendRate, Id = 3,
					DaysAllowed = DaysAllowed.Weekends,
					EntryStartHours = 0, EntryStartMinutes = 1, EntryEndHours = 23,  EntryEndMinutes = 59,
					ExitStartHours = 0, ExitStartMinutes = 0, ExitEndHours = 23, ExitEndMinutes = 59
				}}
			};

		}

		[TestMethod]
		public void WhenEntryIsBetween6AmAnd9AmAndExitIsBetween330PmAnd11Pm()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 6, 0, 0),
				ExitTime = new DateTime(2018, 1, 5, 15, 30, 0)
			};
			_mockRefDataBuilder.Setup(x => x.Build(EarlyBirdRate))
				.Returns(_refDatas.Where(x => x.RefDataType.Name == EarlyBirdRate));
			var expectedParkingRate = new ParkingRate { Amount = 13, Name = EarlyBirdRate };

			//Act
			var actualRate = _calculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}

		[TestMethod]
		public void WhenEntryIsBetween6PmAnd12AmAndExitNextDayBefore6Am()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 6, 0, 0),
				ExitTime = new DateTime(2018, 1, 5, 15, 30, 0)
			};
			_mockRefDataBuilder.Setup(x => x.Build(EarlyBirdRate))
				.Returns(_refDatas.Where(x => x.RefDataType.Name == EarlyBirdRate));
			var expectedParkingRate = new ParkingRate { Amount = 13, Name = EarlyBirdRate };

			//Act
			var actualRate = _calculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate20WhenDurationIsMoreThan3Hours()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 5, 15, 0, 0)
			};
			_mockRefDataBuilder.Setup(x => x.Build(StandardRate))
				.Returns(_refDatas.Where(x => x.RefDataType.Name == StandardRate));
			var expectedParkingRate = new ParkingRate { Amount = 20, Name = StandardRate };

			//Act
			var actualRate = _calculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);
		}


		[TestMethod]
		public void GivenEntryIsBetween6PmAndMidnightAndExitIsBefore6AmFollowingDayParkingRateAmountShouldBe650()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 3, 18, 0, 0),
				ExitTime = new DateTime(1981, 12, 4, 5, 59, 0)
			};
			_mockRefDataBuilder.Setup(x => x.Build(NightRate))
				.Returns(_refDatas.Where(x => x.RefDataType.Name == NightRate));
			var expectedParkingRate = new ParkingRate { Amount = 6.50m, Name = NightRate };

			//Act
			var actualRate = _calculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

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
			_mockRefDataBuilder.Setup(x => x.Build(WeekendRate))
				.Returns(_refDatas.Where(x => x.RefDataType.Name == WeekendRate));
			var expectedParkingRate = new ParkingRate { Amount = 10, Name = WeekendRate };

			//Act
			var actualRate = _calculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}

		
	}
}
