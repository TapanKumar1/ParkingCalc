using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using Parking.Engine;
using Parking.Model;

namespace Parking.Winform
{
	public partial class Form1 : Form
	{
		private readonly ICalculator<EarlyBirdCalculator> _earlyBirdCalculator;
		private readonly ICalculator<StandardRateCalculator> _standardRateCalculator;
		private readonly ICalculator<NightRateCalculator> _nightRateCalculator;
		private readonly ICalculator<WeekendRateCalculator> _weekendCalculator;

		public Form1(ICalculator<EarlyBirdCalculator> earlyBirdCalculator, ICalculator<StandardRateCalculator> standardRateCalculator, ICalculator<NightRateCalculator> nightRateCalculator, ICalculator<WeekendRateCalculator> weekendCalculator)
		{
			_earlyBirdCalculator = earlyBirdCalculator;
			_standardRateCalculator = standardRateCalculator;
			_nightRateCalculator = nightRateCalculator;
			_weekendCalculator = weekendCalculator;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			var calculatedRate = new ParkingCalculator(_earlyBirdCalculator, _standardRateCalculator, _nightRateCalculator, _weekendCalculator)
								.Calculate(new TimeSlot
								{
									EntryTime = dtpEntry.Value,
									ExitTime = dtpExit.Value
								});

			result.Text = JsonConvert.SerializeObject(calculatedRate, Formatting.Indented);
			this.Cursor = Cursors.Default;
		}
	}
}
