using FantasyLeagueData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRosterBot
{
	public class FantasyReportGenerator
	{

		public static void CreatePositionReport(IEnumerable<Roster> Rosters, IEnumerable<RankedPlayer> ProjectedRanking, string Position)
		{
			if (Rosters != null)
			{
				foreach (Roster r in Rosters)
				{
					foreach (RankedPlayer ranked in ProjectedRanking)
					{
						foreach (Player roster_player in r.Players.Where(p => p.Position == Position))
						{
							if ((ExcludePunctuation(ranked.Name).StartsWith(ExcludePunctuation(roster_player.Name)) || ExcludePunctuation(roster_player.Name).StartsWith(ExcludePunctuation(ranked.Name))))
							{
								ranked.Available = false;
								ranked.Owner = r.TeamName;
							}
						}
					}
				}
				using (StreamWriter sw = new StreamWriter(string.Format("./{0}.txt", Position)))
				{
					foreach (RankedPlayer rp in ProjectedRanking)
					{
						sw.WriteLine(string.Format("{0,-20}{1,-15}{2,5}pts\t{3,5}pts ppr\t{4, -5}{5, -5}", rp.Name, rp.RealTeamName, rp.ProjectedPoints, rp.ProjectedPointsPPR, rp.Available ? "FREE AGENT" : "", rp.Owner));
					}
				}
			}
		}

		private static string ExcludePunctuation(string p)
		{
			return new string(p.Where(c => !char.IsPunctuation(c)).ToArray());
		}
	}
}
