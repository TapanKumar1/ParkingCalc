using Parking.Model;

namespace Parking.Engine
{
	public interface ICalculator<T>
	{
		IRate Calculate(TimeSlot timeSlot);
		IRate GetRate(TimeSlot timeSlot, decimal amount);
	}
}