using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parking.Utils;

namespace Parking.Test
{
	[TestClass]
	public class DateTimeExtensionsTests
	{
		[TestMethod]
		public void WhenGivenDateAssertItIsWeekday()
		{
			var dt = new DateTime(1981, 12, 4);

			Assert.AreEqual(dt.IsWeekday(), true);
		}

		[TestMethod]
		public void WhenGivenDateAssertItIsWeekEnd()
		{
			var dt = new DateTime(1981, 12, 5);

			Assert.AreEqual(dt.IsWeekday(), false);
		}

	}
}