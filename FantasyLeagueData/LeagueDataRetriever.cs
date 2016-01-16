using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUtil;

namespace FantasyLeagueData
{
	public static class LeagueDataRetriever
	{

		public static Roster GetRoster(string LeagueId, string TeamId)
		{
			Roster ret;
			using (WebClient c = new WebClient())
			{
				ret = ParseRoster(c.DownloadString(string.Format("http://games.espn.go.com/ffl/clubhouse?leagueId={0}&teamId={1}",LeagueId,TeamId)));
			}
			return ret;
		}

		public static IEnumerable<Roster> GetRosters(string LeagueId)
		{
			string roster_page;
			using (WebClient c = new WebClient())
			{
				roster_page = c.DownloadString(string.Format("http://games.espn.go.com/ffl/leaguerosters?leagueId={0}", LeagueId));
			}
			return ParseRostersFromHtml(roster_page);
		}

		private static Roster ParseRoster(string Html)
		{
			Roster ret = new Roster();
			
			MatchCollection player_match = player_expr.Matches(Html);
			foreach (Match m in player_match)
			{
				Player p = ret.AddPlayer(m.Groups[1].Value);
				if(p!=null)
				{
					p.Position = ParsePosition(m.Groups[2].Value);
					p.RealTeamName = ParseTeam(m.Groups[2].Value);
				}
			}
			return ret;
		}

		private static Regex player_expr = new Regex("\"playername_\\d+\".*?<a.*?>(.*?)</a>(.*?)<.*?</td>", RegexOptions.Compiled | RegexOptions.Multiline);
		private static Regex player_table_expr = new Regex("playertable_\\d+.*?<a.*?>(.*?)</a>.*?</table>", RegexOptions.Compiled | RegexOptions.Multiline);
		private static Regex team_and_pos_expr = new Regex("(\\w+?)(?:&.*?)*(RB|WR|TE|QB|K|D/ST)", RegexOptions.Compiled | RegexOptions.Multiline);
		
		private static string ParseTeam(string TeamAndPos)
		{
			string ret = "";

			Match m = team_and_pos_expr.Match(TeamAndPos);
			if (m.Groups.Count >= 3)
			{
				ret = m.Groups[1].Value;
			}
			return ret;
		}

		private static string ParsePosition(string TeamAndPos)
		{
			string ret = "";
			
			Match m = team_and_pos_expr.Match(TeamAndPos);
			if(m.Groups.Count >=3)
			{
				ret = m.Groups[2].Value;
			}
			return ret;
		}
		private static IEnumerable<Roster> ParseRostersFromHtml(string roster_page)
		{
			
			MatchCollection table_match = player_table_expr.Matches(roster_page);
			foreach(Match m in table_match)
			{
				Roster r = ParseRoster(m.Value);
				r.TeamName = m.Groups[1].Value;
				yield return r;
			}
		}
	}
}
