using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ConsoleApp1s
{
	class Program
	{
		private static readonly InputManager _inputManager = new InputManager();

		static void Main()
		{
			while (true)
			{
				ExecuteExamples();
				Console.WriteLine(Environment.NewLine);
			}
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
			var max = options.Count == 0 ? 0 : options.Count - 1;
			var prompt = $"Select an example between {min} and {max}";

			foreach (var option in options)
				Console.WriteLine(option.Value);

			var selectedExampleIndex = _inputManager.GetInt(prompt, result => result >= min && result <= max);

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
				Console.Write($"{prompt}: ");
			} while (!int.TryParse(Console.ReadLine(), out result) || // has to repeat when int is not a number
			         !conditions.All(c => c.Invoke(result)));  // has to repeat when conditions are not met

			Console.WriteLine(Environment.NewLine);

			return result;
		}
	}
}