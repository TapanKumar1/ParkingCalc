using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking.Model
{
	public class RefData
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id{ get; set; }

		[ForeignKey("RefDataType")]
		public int RefDataTypeId { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public double Duration { get; set; }

		public virtual RefDataType RefDataType { get; set; }
		
	}
}