using MoreLinq;
using Newtonsoft.Json;
using SmashTracker.Contexts;
using SmashTracker.Models;
using SmashTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SmashTracker.Api
{
	[RoutePrefix("api/SmashApi")]
    public class SmashApiController : ApiController
    {
		private SmashContext Db = new SmashContext();
		public HttpConfiguration config = GlobalConfiguration.Configuration;

		public SmashApiController()
		{
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}
		
		[HttpGet, Route("GetTest")]
		public HttpResponseMessage GetTest()
		{
			var TestMessage = "Hey, this is a test Get message response!";

			var ResponseMessage = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonConvert.SerializeObject(TestMessage), Encoding.UTF8, "application/json")
			};

			return ResponseMessage;
		}

		[HttpPost, Route("PostTest")]
		public HttpResponseMessage PostTest(Player player)
		{
			var TestMessage = "This is a Post message reponse! The input you gave was " + player.Name;

			var ResponseMessage = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonConvert.SerializeObject(TestMessage), Encoding.UTF8, "application/json")
			};

			return ResponseMessage;
		}

		[HttpGet, Route("GetAllPlayers")]
		public HttpResponseMessage GetAllPlayers()
		{
			var allplayers = Db.Players.ToArray();

			var ResponseMessage = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonConvert.SerializeObject(allplayers), Encoding.UTF8, "application/json")
			};

			return ResponseMessage;
		}

		[HttpPost, Route("SaveNewPlayer")]
		public HttpResponseMessage SaveNewPlayer(Player player)
		{
			var Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Ambiguous,
				Content = new StringContent(JsonConvert.SerializeObject("Ambiguous"), Encoding.UTF8, "application/json")
			};

			try
			{
				var validationerrormessages = new List<string>();

				player.Id = 0;
				if (Db.Players.Any(x => x.Name == player.Name))
				{
					validationerrormessages.Add("Player name is already in use");
				}
				if (Db.Players.Any(x => x.PlayerTag == player.PlayerTag))
				{
					validationerrormessages.Add("Player tag is already in use");
				}

				if (validationerrormessages.Count == 0)
				{
					var newplayer = Db.Players.Add(player);
					Db.SaveChanges();

					Response.StatusCode = HttpStatusCode.OK;
					Response.Content = new StringContent(JsonConvert.SerializeObject(newplayer, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
					return Response;
				} else
				{
					Response.StatusCode = HttpStatusCode.InternalServerError;
					Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = "Validation failed", MessageDetail = string.Join("; ", validationerrormessages) }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
					return Response;
				}
			}
			catch (Exception e)
			{
				Response.StatusCode = HttpStatusCode.InternalServerError;
				Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = e.Message, MessageDetail = e.InnerException.ToString() }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
				return Response;
			}
		}

		[HttpPost, Route("EditPlayer")]
		public HttpResponseMessage EditPlayer(Player player)
		{
			var Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Ambiguous,
				Content = new StringContent(JsonConvert.SerializeObject("Ambiguous"), Encoding.UTF8, "application/json")
			};
			var CurrentPlayer = new Player();

			try
			{
				var validationerrormessages = new List<string>();

				if (player.Name.Length <= 0 || player.Name.Length > 20)
				{
					validationerrormessages.Add("Player must have a name up to 20 characters");
				}

				if (player.PlayerTag.Length <= 0 || player.PlayerTag.Length > 10)
				{
					validationerrormessages.Add("Player must have a tag up to 10 characters");
				}

				if (!(player.Id > 0)) {
					validationerrormessages.Add("Player Id not found");
				} else
				{
					CurrentPlayer = Db.Players.FirstOrDefault(x => x.Id == player.Id);

					if (CurrentPlayer == null)
					{
						validationerrormessages.Add("Player not found");
					}
				}

				if (validationerrormessages.Count == 0)
				{
					CurrentPlayer.Name = player.Name;
					CurrentPlayer.PlayerTag = player.PlayerTag;
					Db.SaveChanges();

					Response.StatusCode = HttpStatusCode.OK;
					Response.Content = new StringContent(JsonConvert.SerializeObject(CurrentPlayer, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
					return Response;
				}
				else
				{
					Response.StatusCode = HttpStatusCode.InternalServerError;
					Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = "Validation failed", MessageDetail = string.Join("; ", validationerrormessages) }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
					return Response;
				}
			}
			catch (Exception e)
			{
				Response.StatusCode = HttpStatusCode.InternalServerError;
				Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = e.Message, MessageDetail = e.InnerException.ToString() }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
				return Response;
			}
		}

		[HttpPost, Route("SaveNewMatch")]
		public HttpResponseMessage SaveNewMatch(Match match)
		{
			var Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Ambiguous,
				Content = new StringContent(JsonConvert.SerializeObject("Ambiguous"), Encoding.UTF8, "application/json")
			};

			try
			{
				var validationerror = false;
				match.Id = 0;
				if (match.Teams == null || match.Teams.Count < 2 || match.Teams.Count > 8)
				{
					validationerror = true;
				}
				foreach (var team in match.Teams)
				{
					team.Id = 0;
					team.MatchId = 0;
					if (team.Placement < 1 || team.Placement > match.Teams.Count)
					{
						validationerror = true;
					}

					if (team.Players == null || team.Players.Count == 0)
					{
						validationerror = true;
					}

					foreach (var matchplayer in team.Players)
					{
						if (matchplayer.PlayerId < 1 || matchplayer.PlayerId > Db.Players.OrderByDescending(x => x.Id).First().Id)
						{
							validationerror = true;
						}
						if (matchplayer.Id != 0)
						{
							matchplayer.Id = 0;
						}
						if (matchplayer.Player != null)
						{
							matchplayer.Player = null;
						}
					}
				}

				if (validationerror)
				{
					Response.StatusCode = HttpStatusCode.InternalServerError;
					Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage {Message = "Validation failed" }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
					return Response;
				}

				Db.Matches.Add(match);
				Db.SaveChanges();

				Response.StatusCode = HttpStatusCode.OK;
				Response.Content = new StringContent(JsonConvert.SerializeObject(match.Id, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
				return Response;
			}
			catch (Exception e)
			{
				Response.StatusCode = HttpStatusCode.InternalServerError;
				Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = e.Message, MessageDetail = e.InnerException.ToString() }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
				return Response;
			}
		}

		[HttpGet, Route("GetMatches")]
		public HttpResponseMessage GetMatches(int top = -1, int skip = -1, string orderby = "", string orderbydesc = "")
		{
			var Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Ambiguous,
				Content = new StringContent(JsonConvert.SerializeObject("Ambiguous"), Encoding.UTF8, "application/json")
			};
			var result = new List<Match>();

			var matches = Db.Matches.Include("Teams.Players.Player").Where(x => x.Deleted == false);

			if (!String.IsNullOrWhiteSpace(orderby))
			{
				switch (orderby.ToLower())
				{
					case "id":
						matches = matches.OrderBy(x => x.Id);
						break;
					case "ruleset":
						matches = matches.OrderBy(x => x.RuleSet);
						break;
					case "asterisk":
						matches = matches.OrderBy(x => x.Asterisk);
						break;
					case "created":
						matches = matches.OrderBy(x => x.Created);
						break;
					case "modified":
						matches = matches.OrderBy(x => x.Modified);
						break;
				}
			} else if (!String.IsNullOrWhiteSpace(orderbydesc))
			{
				switch (orderbydesc.ToLower())
				{
					case "id":
						matches = matches.OrderByDescending(x => x.Id);
						break;
					case "ruleset":
						matches = matches.OrderByDescending(x => x.RuleSet);
						break;
					case "asterisk":
						matches = matches.OrderByDescending(x => x.Asterisk);
						break;
					case "created":
						matches = matches.OrderByDescending(x => x.Created);
						break;
					case "modified":
						matches = matches.OrderByDescending(x => x.Modified);
						break;
				}
			}

			if (skip >= 0)
			{
				matches = matches.Skip(skip);
			}

			if (top >= 0)
			{
				matches = matches.Take(top);
			}
			

			
			try
			{
				result = matches.ToList();
				foreach (var match in result)
				{
					match.Teams = match.Teams.OrderBy(x => x.Placement).ToList();
				}
				Response.StatusCode = HttpStatusCode.OK;
				Response.Content = new StringContent(JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
				return Response;
			} catch (Exception e)
			{
				Response.StatusCode = HttpStatusCode.InternalServerError;
				Response.Content = new StringContent(JsonConvert.SerializeObject(new ApiErrorMessage { Message = e.Message, MessageDetail = e.InnerException.ToString() }, Formatting.None, new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore}), Encoding.UTF8, "application/json");
				return Response;
			}

		}

		[HttpGet, Route("GetLatestBeltMatchStats")]
		public HttpResponseMessage GetLatestBeltMatchStats()
		{
			var Response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.Ambiguous,
				Content = new StringContent(JsonConvert.SerializeObject("Ambiguous"), Encoding.UTF8, "application/json")
			};

			//Check for the last match of any beltmatchfinal type.
			var BeltMatchFinalTypes = new List<RuleSets> { RuleSets.BeltMatchFinalElimination, RuleSets.BeltMatchFinalPoints, RuleSets.BeltMatchRoundRobin };
			var BeltMatchNormalTypes = new List<RuleSets> { RuleSets.BeltMatchElimination, RuleSets.BeltMatchPoints, RuleSets.BeltMatchRoundRobin };
			var LatestBeltNormalMatch = Db.Matches.Where(x => BeltMatchNormalTypes.Contains(x.RuleSet) && x.Deleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
			var LatestBeltMatchFinal = Db.Matches.Include("Teams.Players.Player").Where(x => BeltMatchFinalTypes.Contains(x.RuleSet) && x.Deleted == false).OrderByDescending(x => x.Id).FirstOrDefault();
			var BeltMatches = new List<Match>();
			var BeltMatchStats = new BeltMatchStatsViewModel();

			if (LatestBeltMatchFinal != null)
			{
				//If it does exist, check if there is an ongoing match right now
				if (LatestBeltNormalMatch != null && LatestBeltMatchFinal.Id < LatestBeltNormalMatch.Id)
				{
					//There is an ongoing match, but we do know the winner of the previous one.
					//That doesn't really matter, since ongoing overwrites the old one.
					LatestBeltMatchFinal = null;
				}
				else
				{
					//There are no current matches, so this is the final result.
					//Search for the beltmatchfinal that happen before it.
					var PreviousBeltMatchFinal = Db.Matches.Where(x => BeltMatchFinalTypes.Contains(x.RuleSet) && x.Id < LatestBeltMatchFinal.Id && x.Deleted == false).OrderByDescending(x => x.Id).FirstOrDefault();

					if (PreviousBeltMatchFinal != null)
					{
						//If it does exist, get all beltmatch (normal) inbetween the two finals of any type. This is the tournament that was held.
						BeltMatches = Db.Matches.Include("Teams.Players.Player").Where(x => BeltMatchNormalTypes.Contains(x.RuleSet) && x.Id < LatestBeltMatchFinal.Id && x.Id > PreviousBeltMatchFinal.Id && x.Deleted == false).ToList();
					}
					else
					{
						//If it doesn't exist, get all beltmatch (normal) of any type created before the latest final. This is probably the tournament that was held.
						BeltMatches = Db.Matches.Include("Teams.Players.Player").Where(x => BeltMatchNormalTypes.Contains(x.RuleSet) && x.Id < LatestBeltMatchFinal.Id && x.Deleted == false).ToList();
					}

				}
			}
			else
			{
				//Never finished a match. Get all beltmatch (normal) of any type. This is probably the ongoing tournament that is being held.
				BeltMatches = Db.Matches.Include("Teams.Players.Player").Where(x => BeltMatchNormalTypes.Contains(x.RuleSet) && x.Deleted == false).ToList();
			}

			//Parse out some stats for these matches depending on what type of final round it was
			if (BeltMatches.Count() > 0)
			{
				//There are matches that lead up to the final one (if the final one exists yet)
				BeltMatchStats.Matches = BeltMatches.ToArray();
				BeltMatchStats.RuleSet = (int)BeltMatches.First().RuleSet;
			} else
			{
				//No belt match has ever been played
			}
			if (LatestBeltMatchFinal != null)
			{
				//We finished the whole set and a winner was chosen
				BeltMatchStats.RuleSet = (int)LatestBeltMatchFinal.RuleSet;//Final match would be the 'real' rule set in case they got mixed up
				BeltMatchStats.FinalMatch = LatestBeltMatchFinal;

				switch (LatestBeltMatchFinal.RuleSet)
				{
					case RuleSets.BeltMatchFinalElimination:
						//The person to win the final round is the total winner
						BeltMatchStats.WinningMatchPlayer = LatestBeltMatchFinal.Teams.Where(x => x.Placement == 1).First().Players.First();
						break;
					case RuleSets.BeltMatchFinalPoints:
						//The person with the highest average placement (plus most kills per match) is the total winner
						var players = new List<BeltMatchPointsPlayerStats>();//Stat tracking for all players between matches

						//Tally up the placements for each player in normal matches
						foreach (var match in BeltMatches)
						{
							foreach (var team in match.Teams)
							{
								foreach (var matchplayer in team.Players)
								{
									if (!players.Exists(x => x.Player.Id == matchplayer.Player.Id))
									{
										players.Add(new BeltMatchPointsPlayerStats() {
											Player = matchplayer.Player,
											Placements = new List<int>(),
											KillCounts = new List<int>()
										});
									}
									players.First(x => x.Player.Id == matchplayer.Player.Id).Placements.Add(team.Placement);
									players.First(x => x.Player.Id == matchplayer.Player.Id).KillCounts.Add(team.KillCount);
									players.First(x => x.Player.Id == matchplayer.Player.Id).FinalScore += team.Placement;
								}
							}
						}

						//Tally up the final match
						foreach (var team in LatestBeltMatchFinal.Teams)
						{
							foreach (var matchplayer in team.Players)
							{
								if (!players.Exists(x => x.Player.Id == matchplayer.Player.Id))
								{
									players.Add(new BeltMatchPointsPlayerStats()
									{
										Player = matchplayer.Player,
										Placements = new List<int>(),
										KillCounts = new List<int>()
									});
								}
								players.First(x => x.Player.Id == matchplayer.Player.Id).Placements.Add(team.Placement);
								players.First(x => x.Player.Id == matchplayer.Player.Id).KillCounts.Add(team.KillCount);
								players.First(x => x.Player.Id == matchplayer.Player.Id).FinalScore += team.Placement;
							}
						}
						
						//All players have their stats recorded. Determine most kills per match
						foreach (var match in BeltMatches)
						{
							//Might have multiple teams getting the award
							var highestkillcountteams = match.Teams
								.GroupBy(x => x.KillCount).OrderByDescending(x => x.Key).FirstOrDefault().ToList();

							foreach (var leadingteam in highestkillcountteams)
							{
								foreach (var leadingmatchplayer in leadingteam.Players)
								{
									players.First(x => x.Player.Id == leadingmatchplayer.Player.Id).FinalScore -= 1;
								}
							}
						}

						//Repeat kill award for final match
						var highestfinalkillcountteams = LatestBeltMatchFinal.Teams
								.GroupBy(x => x.KillCount).OrderByDescending(x => x.Key).FirstOrDefault().ToList();

						foreach (var leadingteam in highestfinalkillcountteams)
						{
							foreach (var leadingmatchplayer in leadingteam.Players)
							{
								players.First(x => x.Player.Id == leadingmatchplayer.Player.Id).FinalScore -= 1;
							}
						}

						//Winner is whoever has the lowest final score
						players = players.OrderBy(x => x.FinalScore).ToList();
						var placement = 1;
						var scoretobeat = players.OrderBy(x => x.FinalScore).First().FinalScore;
						foreach (var player in players.OrderBy(x => x.FinalScore))
						{
							if (player.FinalScore <= scoretobeat)
							{
								player.FinalPlacement = placement;
							} else
							{
								player.FinalPlacement = ++placement;
								scoretobeat = player.FinalScore;
							}
						}

						//bad code to get what I want. Get the matchplayer from the final match, who was on the winning team. doesn't work for team games at all, but should later on.
						BeltMatchStats.WinningMatchPlayer = LatestBeltMatchFinal.Teams.Where(x => x.Players.Where(y => y.PlayerId == players.First().Player.Id).Count() > 0).First().Players.First();
						BeltMatchStats.PointsMatchStats = players.ToArray();
						break;
					case RuleSets.BeltMatchFinalRoundRobin:
						//Need to put this in
						break;
				}
				
			} else
			{
				//No match has ever been finished
			}


			Response.StatusCode = HttpStatusCode.OK;
			Response.Content = new StringContent(JsonConvert.SerializeObject(BeltMatchStats, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
			return Response;
			
		}
	}
}
