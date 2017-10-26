using SmashTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmashTracker.Contexts
{
	public class SmashContext : SmashContextDebug
	{
		public SmashContext() : base("SmashContext")
		{

		}

		public SmashContext(string connection) : base(connection) { }

		public override int SaveChanges()
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		private void AddTimestamps()
		{
			var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

			var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
				? HttpContext.Current.User.Identity.Name
				: "Unknown";

			foreach (var entity in entities)
			{
				if (entity.State == EntityState.Added)
				{
					((BaseEntity)entity.Entity).Created = DateTime.UtcNow;
					((BaseEntity)entity.Entity).UserCreated = currentUsername;
				}

				((BaseEntity)entity.Entity).Modified = DateTime.UtcNow;
				((BaseEntity)entity.Entity).UserModified = currentUsername;
			}
		}

		public DbSet<Match> Matches { get; set; }
		public DbSet<MatchPlayer> MatchPlayers { get; set; }
		public DbSet<MatchTeam> MatchTeams { get; set; }
		public DbSet<Player> Players { get; set; }



	}
}