using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	/// <summary>
	/// An ad-hoc group of players for a specific Match that was played.
	/// A new MatchTeam is created for each Match, and does not carry over.
	/// Contains the data about the team and stats on how they did.
	/// Every match is a team against another team: 1v1 is just two teams of 1.
	/// </summary>
	[Table("MatchTeam")]
	public class MatchTeam : BaseEntity
	{
		[Key, Column(Order = 0)]
		public int Id { get; set; }

		[Required]
		public virtual int MatchId { get; set; }

		[ForeignKey("MatchId")]
		public virtual Match Match { get; set; }

		public ICollection<MatchPlayer> Players { get; set; }

		public int KillCount { get; set; }

		public int Placement { get; set; }
		
	}
}