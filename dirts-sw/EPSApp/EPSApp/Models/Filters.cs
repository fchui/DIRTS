using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPSApp.Models
{
	public class Filters
	{
		public string Name { get; set; }

		public string Key { get; set; }
		public override string ToString()
		{
			return Name;
		}
	}

	public class GroupedFiltersModel : List<Filters>
	{
		public string Name { get; private set; }
		public GroupedFiltersModel(string name, List<Filters> filters) : base(filters)
		{
			Name = name;
		}
		public override string ToString()
		{
			return Name;
		}
	}
}