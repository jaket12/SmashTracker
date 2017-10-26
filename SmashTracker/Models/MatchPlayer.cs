using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	/// <summary>
	/// An ad-hoc player that is used only one time for a MatchTeam.
	/// This MatchPlayer holds the information about what this player did
	/// for a single match. It references to the actual player.
	/// </summary>
	[Table("MatchPlayer")]
	public class MatchPlayer : BaseEntity
	{
		[Key, Column(Order = 0)]
		public int Id { get; set; }

		[Required]
		public virtual int PlayerId { get; set; }

		[ForeignKey("PlayerId")]
		public virtual Player Player { get; set; }

		public Character Character { get; set; }
		
	}
}