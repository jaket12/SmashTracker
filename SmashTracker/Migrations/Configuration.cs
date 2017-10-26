namespace SmashTracker.Migrations
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	public sealed class Configuration : DbMigrationsConfiguration<Contexts.SmashContext>
	{
		private bool EnableDebugging = false;//If true, a pop up will appear on Update-Database that lets you step in and test.
		private bool InsertTestData = true;//If true, place data in the db for local testing.
		private Random random = new Random();
		private Array Characters = Enum.GetValues(typeof(Character));
		private List<string> Players = new List<string>();

		public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Contexts.SmashContext context)
        {
			if (EnableDebugging && System.Diagnostics.Debugger.IsAttached == false)
				System.Diagnostics.Debugger.Launch();

			if (InsertTestData)
			{
				SeedPlayers(context);
				SeedMatches(context);
			}
		}

		/// <summary>
		/// Easy list of players you know are gonna be in there.
		/// </summary>
		/// <param name="context"></param>
		private void SeedPlayers(Contexts.SmashContext context)
		{
			var Players = new List<Player>();
			Players.Add(new Player()
			{
				Name = "Rich",
				PlayerTag = "RICH"
			});
			Players.Add(new Player()
			{
				Name = "Clement",
				PlayerTag = "tITAN"
			});
			Players.Add(new Player()
			{
				Name = "Jesse",
				PlayerTag = "JAG"
			});
			Players.Add(new Player()
			{
				Name = "Odden",
				PlayerTag = "ODIN"
			});
			Players.Add(new Player()
			{
				Name = "Troy",
				PlayerTag = "Troy"
			});
			Players.Add(new Player()
			{
				Name = "Jake",
				PlayerTag = "DERP"
			});
			Players.Add(new Player()
			{
				Name = "Grozek",
				PlayerTag = "GROZ"
			});
			Players.Add(new Player()
			{
				Name = "Zach",
				PlayerTag = "ZACH"
			});
			Players.Add(new Player()
			{
				Name = "Kim",
				PlayerTag = "KIM"
			});
			Players.Add(new Player()
			{
				Name = "Noriega",
				PlayerTag = "NORI"
			});
			Players.Add(new Player()
			{
				Name = "Steve",
				PlayerTag = "STEVE"
			});
			Players.Add(new Player()
			{
				Name = "Jeff",
				PlayerTag = "BIBBY"
			});
			Players.Add(new Player()
			{
				Name = "Gus",
				PlayerTag = "MLGSonic"
			});

			foreach (var player in Players)
			{
				var userexists = context.Players.Any(x => x.Name == player.Name || x.PlayerTag == player.PlayerTag);

				if (!userexists)
				{
					context.Players.Add(player);
				}
			}

			context.SaveChanges();
		}

		/// <summary>
		/// Given some details, pull random players and characters to create a faked match that was played.
		/// Teams will always be even in player count (1v1, 2v2...) and will never have one person on two teams in the same match.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="teamcount">How many teams</param>
		/// <param name="teamplayercount">How many players are in a team</param>
		/// <param name="ruleset">Rules the match was played under</param>
		/// <param name="asterisk">Did Clement play?</param>
		/// <returns></returns>
		private Match GenerateRandomMatch(Contexts.SmashContext context, int teamcount, int teamplayercount, RuleSets ruleset, bool asterisk)
		{
			var tempplayers = Players.ToList();
			var tempteams = new List<MatchTeam>();

			for (var i = 0; i < teamcount; i++)
			{
				var teamplayers = new List<MatchPlayer>();
				for (var j = 0; j < teamplayercount; j++)
				{
					var randomplayerid = (tempplayers.Count == 1) ? 0 : random.Next(tempplayers.Count);
					var randomplayername = tempplayers[randomplayerid];
					var randomplayer = context.Players.First(x => x.Name == randomplayername);
					var randomcharacter = (Character)Characters.GetValue(random.Next(Characters.Length));
					tempplayers.RemoveAt(randomplayerid);

					teamplayers.Add(new MatchPlayer()
					{
						Character = randomcharacter,
						Player = randomplayer
					});
					
				}

				var randomkillcount = random.Next((teamcount - 1) * 4);
				var placement = i + 1;

				tempteams.Add(new MatchTeam()
				{
					KillCount = randomkillcount,
					Placement = placement,
					Players = teamplayers
				});
			}

			tempteams = tempteams.OrderByDescending(x => x.KillCount).ToList();
			var tempplacement = 1;
			foreach (var team in tempteams)
			{
				team.Placement = tempplacement++;
			}

			var Match = new Match()
			{
				Asterisk = asterisk,
				RuleSet = ruleset,
				Teams = tempteams,
				UserCreated = "Seed Data"
			};



			return Match;
		}

		private void SeedMatches(Contexts.SmashContext context)
		{
			if (context.Matches.Count() > 0)
			{
				return;
			}

			Players = context.Players.Select(x => x.Name).ToList();
			var Matches = new List<Match>();

			Matches.Add(GenerateRandomMatch(context, 2, 1, RuleSets.Tournament, false));//1v1
			Matches.Add(GenerateRandomMatch(context, 2, 2, RuleSets.Tournament, false));//2v2
			Matches.Add(GenerateRandomMatch(context, 2, 3, RuleSets.Tournament, false));//3v3
			Matches.Add(GenerateRandomMatch(context, 2, 4, RuleSets.Tournament, false));//4v4
			Matches.Add(GenerateRandomMatch(context, 3, 1, RuleSets.Tournament, false));//1v1v1 FFA
			Matches.Add(GenerateRandomMatch(context, 3, 2, RuleSets.Tournament, false));//2v2v2
			Matches.Add(GenerateRandomMatch(context, 4, 2, RuleSets.Tournament, false));//2v2v2v2
			Matches.Add(GenerateRandomMatch(context, 8, 1, RuleSets.Tournament, false));//8 way FFA
			//Matches.Add(GenerateRandomMatch(context, 8, 1, RuleSets.BeltMatchElimination, false));//Belt match 1
			//Matches.Add(GenerateRandomMatch(context, 4, 1, RuleSets.BeltMatchElimination, false));//Belt match 2
			//Matches.Add(GenerateRandomMatch(context, 2, 1, RuleSets.BeltMatchFinalElimination, false));//Belt match 3 Final
			//Matches.Add(GenerateRandomMatch(context, 6, 1, RuleSets.BeltMatchPoints, false));//Belt match 1 B
			//Matches.Add(GenerateRandomMatch(context, 6, 1, RuleSets.BeltMatchPoints, false));//Belt match 2 B
			//Matches.Add(GenerateRandomMatch(context, 6, 1, RuleSets.BeltMatchPoints, false));//Belt match 3 B
			//Matches.Add(GenerateRandomMatch(context, 6, 1, RuleSets.BeltMatchFinalPoints, false));//Belt match 4 B Final


			context.Matches.AddRange(Matches);
			
			context.SaveChanges();
		}
    }
}
