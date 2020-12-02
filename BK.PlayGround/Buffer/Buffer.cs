using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using ConsoleApp1s.MicroTimer;

namespace ConsoleApp1s.Buffer
{
	sealed class ConcurrentBuffer<T> : Buffer<T>
	{
		private SynchronizationContext _synchronizationContext;
		private BackgroundWorkerPool.BackgroundWorkerPool _workerPool = new BackgroundWorkerPool.BackgroundWorkerPool();


		public ConcurrentBuffer(int maxCount, int maxDelayMs, SynchronizationContext synchronizationContext) 
			: base(maxCount, maxDelayMs)
		{
			_synchronizationContext = synchronizationContext;
		}

		public override void Enqueue(in T item)
		{
			_workerPool.RunWorkerAsync();
			
		}
	}

	class Buffer<T>
	{
		protected readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
		private readonly int _maxCount;
		private readonly MicroTimer.MicroTimer _timer;

		public delegate void BufferThresholdReached(ImmutableQueue<T> buffer);
		public event BufferThresholdReached ThresholdReached;

		public Buffer(int maxCount, int maxDelayMs)
		{
			_maxCount = maxCount;
			_timer = new MicroTimer.MicroTimer(maxDelayMs * 1000);
			_timer.MicroTimerElapsed += TimerOnMicroTimerElapsed;
		}

		public virtual void Enqueue(in T item)
		{
			_queue.Enqueue(item);
			_timer.Start();
			if (_queue.Count >= _maxCount)
			{
				_timer.Stop();
				ThresholdReached?.Invoke(ImmutableQueue.CreateRange<T>(_queue));
				_queue.Clear();
			}
		}

		private void TimerOnMicroTimerElapsed(object sender, in MicroTimerEventArgs args)
		{
			_timer.Stop();
			ThresholdReached?.Invoke(ImmutableQueue.CreateRange<T>(_queue));
			_queue.Clear();
		}
	}
}
