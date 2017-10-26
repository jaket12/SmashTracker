using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	/// <summary>
	/// A single instance of a game played between 2 or more players.
	/// Contains info about the game like teams and rules.
	/// </summary>
	[Table("Match")]
	public class Match : BaseEntity
	{
		[Key, Column(Order = 0)]
		public int Id { get; set; }

		public virtual ICollection<MatchTeam> Teams { get; set; }
		
		public RuleSets RuleSet { get; set; }

		public bool Asterisk { get; set; }
		
	}
}