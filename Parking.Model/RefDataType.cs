using System;
using System.ComponentModel.DataAnnotations;

namespace Parking.Model
{
	public class RefDataType
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public int EntryStartHours { get; set; }
		public int EntryStartMinutes { get; set; }
		public int EntryEndHours { get; set; }
		public int EntryEndMinutes { get; set; }

		public int ExitStartHours { get; set; }
		public int ExitStartMinutes { get; set; }
		public int ExitEndHours { get; set; }
		public int ExitEndMinutes { get; set; }

		public DaysAllowed DaysAllowed { get; set; }

	}
	
	[Flags]
	public enum DaysAllowed
	{
		Sunday = 1,
		Monday = 2,
		Tuesday = 3,
		Wednesday = 4,
		Thursday = 5,
		Friday = 6,
		Saturday = 7,
		Weekdays = Monday + Tuesday + Wednesday + Thursday + Friday,
		Weekends = Saturday + Sunday,
		Everyday = Monday + Tuesday + Wednesday + Thursday + Friday + Saturday + Sunday
	}
}
