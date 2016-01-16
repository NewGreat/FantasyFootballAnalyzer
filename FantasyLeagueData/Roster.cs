using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyLeagueData
{
	public class Roster
	{
		public IList<Player> Players { get; private set; }

		internal Roster()
		{
			Players = new List<Player>();
		}

		internal Player AddPlayer(string Name)
		{
			Player ret = null;
			Player athlete = new Player { Name = Name };

			if(!this.Players.Any(p=>p.Name == athlete.Name))
			{
				Players.Add(athlete);
				ret = athlete;
			}
			return ret;
		}

		public string TeamName { get; set; }
	}
}
