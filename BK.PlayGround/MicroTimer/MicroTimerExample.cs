﻿using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp1s.MicroTimer
{
	[Example(nameof(MicroTimerExample), typeof(MicroTimerExample))]
	class MicroTimerExample : IExample
	{
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private ConsoleApp1s.MicroTimer.MicroTimer _timer;
		private readonly InputManager _inputManager = new InputManager();

		private int _counter;

		public void Execute()
		{
			try
			{
				_timer = new ConsoleApp1s.MicroTimer.MicroTimer(200 * 1000);
				_timer.MicroTimerElapsed += TimerOnMicroTimerElapsed;
				_timer.Start();


				while (true)
				{
					if (_counter == 10)
					{
						_timer.Stop();
						_counter = 0;

						Console.WriteLine("Timer is paused after 10th count!");
						var duration = _inputManager.GetInt("Set a new duration", result => result > 0);

						_timer.MicroTimerElapsed -= TimerOnMicroTimerElapsed;
						_timer = new ConsoleApp1s.MicroTimer.MicroTimer(duration);
						_timer.MicroTimerElapsed += TimerOnMicroTimerElapsed;
						_timer.Start();
					}

				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			Console.ReadLine();

		}

		private void TimerOnMicroTimerElapsed(object sender, in MicroTimerEventArgs timerEventArgs)
		{
			Console.WriteLine($"Count: {_counter} Elapsed: {_stopwatch.Elapsed}");
			Interlocked.Increment(ref _counter);
			_stopwatch.Restart();
		}

	}
}
