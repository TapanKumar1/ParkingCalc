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
	public class StandardRateCalculatorTests
	{
		private readonly ICalculator<StandardRateCalculator> _standardCalculator;
		private readonly Mock<IRepository<RefData>> _mockRepository;
		private const string StandardRate = "Standard Rate";
		private List<RefData> _refDatas;
		public StandardRateCalculatorTests()
		{
			_mockRepository = new Mock<IRepository<RefData>>();
			var refDataBuilder = new RefDataBuilder(_mockRepository.Object);
			var standardRateCalculator = new Mock<StandardRateCalculator>(refDataBuilder);
			_standardCalculator = standardRateCalculator.Object;
		}

		[TestInitialize]
		public void Setup()
		{
			_refDatas = new List<RefData>
			{
				new RefData {Amount = 5, RefDataTypeId = 4, Name = StandardRate, Duration = 1},
				new RefData {Amount = 10, RefDataTypeId = 4, Name = StandardRate, Duration = 2},
				new RefData {Amount = 15, RefDataTypeId = 4, Name = StandardRate, Duration = 3},
				new RefData {Amount = 20, RefDataTypeId = 4, Name = StandardRate, Duration = 3.1}
			};
			_mockRepository.Setup(x => x.Matches(It.IsAny<ICriteria<RefData>>())).Returns(_refDatas.AsQueryable());
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate5WhenDurationIsUpto1Hour()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 5, 10, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 5, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate10WhenDurationIsBetween1And2Hours()
		{
			//Arrange
			
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 5, 11, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 10, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate15WhenDurationIsBetween2And3Hours()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 5, 12, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 15, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate20WhenDurationIsMoreThan3Hours()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 5, 13, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 20, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate40WhenDurationIs2Days()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 6, 13, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 40, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

		[TestMethod]
		public void GivenEntryAndExitTimeShouldReturnRate60WhenDurationIs3Days()
		{
			//Arrange
			var timeSlot = new TimeSlot
			{
				EntryTime = new DateTime(2018, 1, 5, 9, 1, 0),
				ExitTime = new DateTime(2018, 1, 7, 13, 00, 0)
			};
			var expectedParkingRate = new ParkingRate { Amount = 60, Name = StandardRate };

			//Act
			var actualRate = _standardCalculator.Calculate(timeSlot);

			//Assert
			Assert.AreEqual(expectedParkingRate.Amount, actualRate.ParkingRate.Amount);
		}

	}
}