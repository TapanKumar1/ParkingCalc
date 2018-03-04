namespace Parking.Model
{
	public class Rate : IRate
	{
		public ParkingRate ParkingRate { get; set; }
		public TimeSlot TimeSlot { get; set; }
	}
}