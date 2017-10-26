using SmashTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	public class BeltMatchStatsViewModel
	{
		public Match FinalMatch { get; set; }

		public Match[] Matches { get; set; }

		public MatchPlayer WinningMatchPlayer { get; set; }

		public int RuleSet { get; set; }

		public BeltMatchPointsPlayerStats[] PointsMatchStats { get; set; }
		/*
		public int TitleDefendedCount { get; set; }

		public int TotalTimesWon { get; set; }


		public int StocksRemaining { get; set; }

		public int FinalPointScore { get; set; }
		*/
	}
}