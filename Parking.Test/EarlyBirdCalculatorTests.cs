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
	public class EarlyBirdCalculatorTests
	{
		private readonly ICalculator<EarlyBirdCalculator> _earlyBirdCalculator;
		private readonly Mock<IRepository<RefData>> _mockRepository;
		private const string EarlyBirdRate = "Early Bird Rate";
		private List<RefData> _refDatas;

		public EarlyBirdCalculatorTests()
		{
			_mockRepository = new Mock<IRepository<RefData>>();
			var refDataBuilder = new RefDataBuilder(_mockRepository.Object);
			var mockEarlyBirdCalculator = new Mock<EarlyBirdCalculator>(refDataBuilder) ;
			_earlyBirdCalculator = mockEarlyBirdCalculator.Object;
		}

		[TestInitialize]
		public void Setup()
		{
			_refDatas = new List<RefData>
			{
				new RefData {Amount = 13, RefDataTypeId = 1, Name = EarlyBirdRate, RefDataType = new RefDataType
				{
					Name = EarlyBirdRate,
					DaysAllowed = DaysAllowed.Weekdays,
					EntryStartHours = 6, EntryStartMinutes = 0, EntryEndHours = 9,  EntryEndMinutes = 0,
					ExitStartHours = 15, ExitStartMinutes = 30, ExitEndHours = 23, ExitEndMinutes = 30

				}},
			};
			_mockRepository.Setup(x => x.Matches(It.IsAny<ICriteria<RefData>>())).Returns(_refDatas.AsQueryable());
		}

		[TestMethod]
		public void WhenEntryIsBetween6AmAnd9AmAndExitIsBetween330PmAnd11PmParkingRateAmountShouldBe13()
		{
			//Arrange
			const string earlyBirdRate = "Early Bird Rate";
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 6, 0, 0),
				ExitTime = new DateTime(2018, 1, 5, 15, 30, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 13, Name = earlyBirdRate };

			//Act
			var actualRate = _earlyBirdCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);

		}

		[TestMethod]
		public void WhenEntryIsBetween6PmAnd12AmAndExitNextDayBefore6AmParkingRateAmountShouldBe13()
		{
			//Arrange
			const string earlyBirdRate = "Early Bird Rate";
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 6, 0, 0),
				ExitTime = new DateTime(2018, 1, 5, 15, 30, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 13, Name = earlyBirdRate };

			//Act
			var actualRate = _earlyBirdCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);
		}

		[TestMethod]
		public void WhenEntryIsOnWeekendParkingRateAmountShouldBeZero()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 5, 6, 0, 0),
				ExitTime = new DateTime(1981, 12, 5, 15, 30, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 0, Name = "Early Bird Rate" };

			//Act
			var actualRate = _earlyBirdCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);
		}

		[TestMethod]
		public void WhenEntryIsOutsideEarlyBirdHoursParkingRateAmountShouldBeZero()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(1981, 12, 4, 10, 0, 0),
				ExitTime = new DateTime(1981, 12, 4, 15, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 0, Name = "Early Bird Rate" };

			//Act
			var actualRate = _earlyBirdCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
			Assert.AreEqual(expectedParkingRate.Name, actualRate.ParkingRate.Name);
		}
	}
}