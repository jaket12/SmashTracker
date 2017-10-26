using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	/// <summary>
	/// List of different game modes that can be played in a Match.
	/// Casual, Coin, Tournament, Belt Match...
	/// </summary>
	public enum RuleSets
	{
		Stock = 0,
		Time = 1,
		Coin = 2,
		Casual = 3,
		Tournament = 4,
		BeltMatchPoints = 5,
		BeltMatchFinalPoints = 6,
		BeltMatchElimination = 7,
		BeltMatchFinalElimination = 8,
		BeltMatchRoundRobin = 9,
		BeltMatchFinalRoundRobin = 10
	}
}