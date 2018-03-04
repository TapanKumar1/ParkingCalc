using Parking.Model;

namespace Parking.Engine
{
	public class ParkingCalculator
	{
		private readonly ICalculator<EarlyBirdCalculator> _earlyBirdCalculator;
		private readonly ICalculator<StandardRateCalculator> _standardRateCalculator;
		private readonly ICalculator<NightRateCalculator> _nightRateCalculator;
		private readonly ICalculator<WeekendRateCalculator> _weekendRateCalculator;

		public ParkingCalculator(ICalculator<EarlyBirdCalculator> earlyBirdCalculator,
			ICalculator<StandardRateCalculator> standardRateCalculator, ICalculator<NightRateCalculator> nightRateCalculator, ICalculator<WeekendRateCalculator> weekendRateCalculator)
		{
			_earlyBirdCalculator = earlyBirdCalculator;
			_standardRateCalculator = standardRateCalculator;
			_nightRateCalculator = nightRateCalculator;
			_weekendRateCalculator = weekendRateCalculator;
		}

		public IRate Calculate(TimeSlot timeSlot)
		{
			//Early Bird Calculator
			var rate = _earlyBirdCalculator.Calculate(timeSlot);
			if(rate.ParkingRate.Amount > 0) return rate;

			// Night Rate Calculator
			rate = _nightRateCalculator.Calculate(timeSlot);
			if (rate.ParkingRate.Amount > 0) return rate;

			// Weekend Rate Calculator
			rate = _weekendRateCalculator.Calculate(timeSlot);
			if (rate.ParkingRate.Amount > 0) return rate;

			//Standard Rate Calculator
			rate = _standardRateCalculator.Calculate(timeSlot);
			if (rate.ParkingRate.Amount > 0) return rate;

			return new Rate { ParkingRate = new ParkingRate { Amount = 0.0m, Name = "Unknown"}, TimeSlot = timeSlot} ;
		}
	}
}