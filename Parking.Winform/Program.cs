using System;
using System.Windows.Forms;
using Parking.BL;
using Parking.Engine;
using SimpleInjector;

namespace Parking.Winform
{
	static class Program
	{
		private static Container _container;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Bootstrap();
			AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
			Application.Run(_container.GetInstance<Form1>());
		}

		private static void Bootstrap()
		{
			// Create the container as usual.
			_container = new Container();
			BLModules.Register(_container);
			_container.Register<ICalculator<EarlyBirdCalculator>, EarlyBirdCalculator>();
			_container.Register<ICalculator<StandardRateCalculator>, StandardRateCalculator>();
			_container.Register<ICalculator<NightRateCalculator>, NightRateCalculator>();
			_container.Register<ICalculator<WeekendRateCalculator>, WeekendRateCalculator>();
			// Register your types, for instance:
			_container.Register<Form1>();
		}
	}
}
