using System.Collections.Generic;
using System.Data.Entity;
using Parking.Model;

namespace Parking.Data
{
	public class DbConnection : DbContext
	{
		public DbConnection() : base("name=ParkingDbContext")
		{

		}

		public IDbSet<RefData> RefDatas { get; set; }
		public IDbSet<RefDataType> RefDataTypes { get; set; }

		public virtual IDbSet<T> DbSet<T>() where T : class
		{
			return Set<T>();
		}

		public virtual void Commit()
		{
			SaveChanges();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			Database.SetInitializer(new Initialiser());
		}

		internal class Initialiser : DropCreateDatabaseAlways<DbConnection>
		{
			protected override void Seed(DbConnection dbHelper)
			{
				var refDataTypes = new List<RefDataType>
				{
					new RefDataType
					{
						Name = "Early Bird Rate", Id = 1,
						EntryStartHours = 6, EntryStartMinutes = 0, EntryEndHours = 9, EntryEndMinutes = 0,
						ExitStartHours = 15, ExitStartMinutes = 30, ExitEndHours = 23, ExitEndMinutes = 30,
						DaysAllowed = DaysAllowed.Weekdays
					},
					new RefDataType
					{
						Name = "Night Rate", Id = 2,
						EntryStartHours = 18, EntryStartMinutes = 0, EntryEndHours = 23,  EntryEndMinutes = 59,
						ExitStartHours = 0, ExitStartMinutes = 1, ExitEndHours = 6, ExitEndMinutes = 0,
						DaysAllowed = DaysAllowed.Weekdays
					},
					new RefDataType
					{
						Name = "Weekend Rate", Id = 3,
						EntryStartHours = 0, EntryStartMinutes = 1, EntryEndHours = 0, EntryEndMinutes = 0,
						ExitStartHours = 0, ExitStartMinutes = 0, ExitEndHours = 23, ExitEndMinutes = 59,
						DaysAllowed = DaysAllowed.Weekends
					},
					new RefDataType
					{
						Name = "Standard Rate", Id = 4,
						DaysAllowed = DaysAllowed.Weekdays
					}
				};
				foreach (var refDataType in refDataTypes)
				{
					dbHelper.RefDataTypes.Add(refDataType);
				}

				var refDatas = new List<RefData>
				{
					new RefData {Name = "Flat Rate", RefDataTypeId = 1, Amount = 13},
					new RefData {Name = "Flat Rate", RefDataTypeId = 2, Amount = 6.50m},
					new RefData {Name = "Flat Rate", RefDataTypeId = 3, Amount = 10},
					new RefData {Name = "Hourly Rate", RefDataTypeId = 4, Amount = 5, Duration = 1},
					new RefData {Name = "Hourly Rate", RefDataTypeId = 4, Amount = 10, Duration = 2},
					new RefData {Name = "Hourly Rate", RefDataTypeId = 4, Amount = 15, Duration = 3},
					new RefData {Name = "Hourly Rate", RefDataTypeId = 4, Amount = 20, Duration = 3.1},
				};
				foreach (var refData in refDatas)
				{
					dbHelper.RefDatas.Add(refData);
				}
				dbHelper.Commit();
			}

		}
	}
}