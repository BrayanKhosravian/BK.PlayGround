using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

// BridgePattern.Run();
//Chain.Run();

namespace ConsoleApp1s
{
	readonly struct Letter
	{
		public int Priority { get; }
		public char L { get; }

		public Letter(int priority, char l)
		{
			Priority = priority;
			L = l;
		}
	}


	class Option
	{
		public int Id { get; }
		public string Name { get; }
		public Action Execute { get; }

		public Option(int id, string name, Action execute)
		{
			Id = id;
			Name = name;
			Execute = execute;
		}

		public override string ToString() => $"{Id}: {Name}";
	}

	class Program
	{
		private static InputManager _inputManager = new InputManager();

		static void Main()
		{
			while(true) ExecuteExamples();
		}

		private static void ExecuteExamples()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var exampleAttributes = assembly.GetTypes()
				.Where(t => t.IsDefined(typeof(ExampleAttribute), true))
				.SelectMany(t => t.GetCustomAttributes())
				.ToArray();

			var consoleOptions = new Dictionary<int, Option>();

			for (int i = 0; i < exampleAttributes.Length; i++)
			{
				var exampleAttribute = (ExampleAttribute) exampleAttributes[i];
				var id = i;
				var name = exampleAttribute.Name;
				void Execute() => ((IExample) Activator.CreateInstance(exampleAttribute.Type)).Execute();
				consoleOptions.Add(i, new Option(id, name, Execute));
			}

			ExecuteExample(consoleOptions);
		}

		private static void ExecuteExample(Dictionary<int, Option> options)
		{
			Console.WriteLine("Select which example you want to execute.");
			
			var min = 0;
			var max = options.Count == 0 ? 0 : options.Count;
			var prompt = $"Select an example between {min} and {max}.";

			foreach (var option in options)
				Console.WriteLine(option.Value);

			var selectedExampleIndex = _inputManager.GetInt(prompt, result => result > min && result < max);

			options[selectedExampleIndex].Execute.Invoke();
		}

	}

	class InputManager
	{
		public int GetInt(string prompt, params Func<int, bool>[] conditions)
		{
			int result;
			do
			{
				Console.WriteLine(prompt);
			} while (!int.TryParse(Console.ReadLine(), out result) && 
			         conditions.All(c => c.Invoke(result)));

			return result;
		}
	}
}