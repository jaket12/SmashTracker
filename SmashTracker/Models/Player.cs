using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SmashTracker.Models
{
	/// <summary>
	/// An actual static player that is used between different Matches and Teams.
	/// If you want to track data between a person's sessions, it would be stored here.
	/// </summary>
	[Table("Player")]
	public class Player : BaseEntity
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(50)]
		[Index("NameTag", IsUnique = true, Order = 1)]
		public string Name { get; set; }

		[MaxLength(10)]
		[Index("NameTag", IsUnique = true, Order = 2)]
		public string PlayerTag { get; set; }
	}
}