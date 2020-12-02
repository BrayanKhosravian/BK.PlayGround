using System;

namespace ConsoleApp1s
{
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
}