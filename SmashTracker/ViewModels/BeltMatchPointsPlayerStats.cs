using SmashTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.ViewModels
{
	public class BeltMatchPointsPlayerStats
	{
		public Player Player { get; set; }
		public List<int> Placements { get; set; }
		public List<int> KillCounts { get; set; }
		public int FinalPlacement { get; set; }
		public int FinalScore { get; set; }
	}
}