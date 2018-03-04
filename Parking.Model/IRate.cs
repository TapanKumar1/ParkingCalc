namespace Parking.Model
{
	public interface IRate
	{
		ParkingRate ParkingRate { get; set; }
		TimeSlot TimeSlot { get; set; }
	}
}