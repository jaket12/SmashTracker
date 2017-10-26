using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.ViewModels
{
	public class SiteLayout
	{
		public String[] Characters = Enum.GetNames(typeof(Models.Character)).ToArray();
		public String[] RuleSets = Enum.GetNames(typeof(Models.RuleSets)).ToArray();
		public object Data;
	}
}