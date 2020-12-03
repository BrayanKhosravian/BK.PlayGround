using ConsoleApp1s;

namespace BK.PlayGround.ConsoleApp.MessageLoop
{
	[Example(nameof(MessageLoopExample), typeof(MessageLoopExample))]
	class MessageLoopExample : IExample
	{
		public void Execute()
		{
			var loop = new MessageLoop();
			loop.Init();

			while (true)
			{
				
			}
		}
	}
}