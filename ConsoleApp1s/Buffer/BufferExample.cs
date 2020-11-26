using System;
using System.Collections.Immutable;
using System.Linq;

namespace ConsoleApp1s.Buffer
{
	[Example(nameof(BufferExample), typeof(BufferExample))]
	class BufferExample : IExample
	{
		public void Execute()
		{
			
		}

		public void Execute2()
		{
			var buffer = new Buffer<int>(4, 100);
			buffer.ThresholdReached += BufferOnThresholdReached;

			Console.WriteLine("Adding 4 items immediately!");
			foreach(var item in Enumerable.Range(0,4))
				buffer.Enqueue(item);
			Console.WriteLine("hit any key to continue\n\n");
			Console.ReadLine();

			Console.WriteLine("Adding only 3 items which results in a timer thresholdreached!");
			foreach (var item in Enumerable.Range(0,3))
				buffer.Enqueue(item);
			Console.WriteLine("hit any key to break\n\n");
			Console.ReadLine();

		}

		private void BufferOnThresholdReached(ImmutableQueue<int> buffer)
		{
			Console.WriteLine($"ThresholdReached! items: {string.Join(", ", buffer)}");
		}
	}
}