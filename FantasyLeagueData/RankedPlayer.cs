using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLeagueData
{
	public class RankedPlayer : Player
	{
		public double ProjectedPoints { get; set; }
		public double ProjectedPointsPPR { get; set; }
		public int Rank { get; set; }
		public bool Available { get; set; }
		public string Owner { get; set; }

		public RankedPlayer(IDictionary<string, string> PlayerDict)
		{
			Available = true;
			double proj_points;
			double proj_points_ppr;

			this.Name = PlayerDict["Player"];
			this.Position = PlayerDict["Pos"];
			this.RealTeamName = PlayerDict["Team"];
			if(double.TryParse(PlayerDict["FFPts"], out proj_points))
			{
				ProjectedPoints = proj_points;
			}
			if (double.TryParse(PlayerDict["FFPtsPPR"], out proj_points_ppr))
			{
				ProjectedPointsPPR = proj_points_ppr;
			}
			
		}

		
	}
}
