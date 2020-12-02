using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace ConsoleApp1s.BackgroundWorkerPool
{
	[ExampleAttribute(nameof(BackGroundWorkerPoolExample), typeof(BackGroundWorkerPoolExample))]
	class BackGroundWorkerPoolExample : IExample
	{
		private BackgroundWorkerPool _workerPool = new BackgroundWorkerPool();

		public void Execute()
		{
			var items = Enumerable.Range(0, int.MaxValue - 1);
			_workerPool.DoWork += WorkerOnDoWork;
			_workerPool.RunWorkerCompleted += WorkerOnRunWorkerCompleted;

			foreach (var item in items)
			{
				Thread.Sleep(50);
				Tick(item);
			}
		}

		private static void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{


		}

		private static void WorkerOnDoWork(object sender, DoWorkEventArgs e)
		{
			Console.WriteLine($"Background worker! {Thread.CurrentThread.ManagedThreadId}");
			Thread.Sleep(1000);
		}

		private int _counter = 0;

		private void Tick(int item)
		{
			_counter++;

			if (_counter == 4)
			{
				_workerPool.RunWorkerAsync();
				_counter = 0;
			}

			Console.WriteLine(item);

		}
	}
}