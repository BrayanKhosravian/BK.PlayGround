using System;

namespace ConsoleApp1s
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExampleAttribute : Attribute
	{
		public string Name { get; }
		public Type Type { get; }

		public ExampleAttribute(string name, Type type)
		{
			Name = name;
			Type = type;
		}
	}
}