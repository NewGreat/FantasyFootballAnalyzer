using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyLeagueData;
using System.IO;

namespace FantasyRosterBot
{
	class Program
	{
		static void Main(string[] args)
		{
			IEnumerable<RankedPlayer> runningbacks = null;
			IEnumerable<RankedPlayer> widereceivers = null;
			IEnumerable<RankedPlayer> quarterbacks = null;
			IEnumerable<RankedPlayer> tightends = null;

			if(args.Length > 0)
			{
				CsvRankingReader reader = new CsvRankingReader(args[0]);

				runningbacks = GetPlayers(reader, "RB");
				widereceivers = GetPlayers(reader, "WR");
				quarterbacks = GetPlayers(reader, "QB");
				tightends = GetPlayers(reader, "TE");
			}
			
			IEnumerable<Roster> rosters = LeagueDataRetriever.GetRosters("632523");
			FantasyReportGenerator.CreatePositionReport(rosters, runningbacks, "RB");
			FantasyReportGenerator.CreatePositionReport(rosters, widereceivers, "WR");
			FantasyReportGenerator.CreatePositionReport(rosters, quarterbacks, "QB");
			FantasyReportGenerator.CreatePositionReport(rosters, tightends, "TE");
		}

		private static IEnumerable<RankedPlayer> GetPlayers(CsvRankingReader Reader, string Position)
		{
			return Reader.GetPlayers(Position).OrderBy<RankedPlayer, double>((player) => player.ProjectedPointsPPR).Reverse().ToList();
		}
	}
}
