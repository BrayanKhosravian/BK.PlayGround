using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp1s.AccurateTimer
{
	[Example(nameof(AccurateTimerExample), typeof(AccurateTimerExample))]
	class AccurateTimerExample : IExample
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public void Execute()
		{
			var timer = new AccurateTimer();
			_stopwatch.Start();
			timer.Create(0, 10, CallbackDelegate);
		}

		private void CallbackDelegate(IntPtr lpparameter, bool timerorwaitfired)
		{
			Console.WriteLine($"Elapsed: {_stopwatch.Elapsed}");
			_stopwatch.Restart();
		}
	}
}
