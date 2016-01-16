using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLeagueData
{
	public class CsvRankingReader
	{
		public CsvRankingReader(string Path)
		{
			m_PlayerPool = new List<IDictionary<string, string>>();
			using(StreamReader sr = new StreamReader(Path))
			{
				SetColumnHeaders(sr.ReadLine());

				while(!sr.EndOfStream)
				{
					ProcessPlayerRecord(sr.ReadLine());
				}
			}
		}

		/// <summary>
		/// Gets the players by position.
		/// </summary>
		/// <param name="Position">The position.</param>
		/// <returns></returns>
		public IEnumerable<RankedPlayer> GetPlayers(string Position)
		{
			foreach(Dictionary<string,string> d in m_PlayerPool.Where(p=> p["Pos"] == Position))
			{
				yield return new RankedPlayer(d);
			}
			
		}

		private void ProcessPlayerRecord(string Fields)
		{
			string[] split_fields = Fields.Split(',');
			Dictionary<string, string> player = new Dictionary<string, string>();
			for(int i = 0; i < split_fields.Length;i++)
			{
				player[m_ColumnHeaders[i]] = split_fields[i];
			}
			m_PlayerPool.Add(player);
		}

		private void SetColumnHeaders(string Columns)
		{
			m_ColumnHeaders = Columns.Split(',');
		}

		private string[] m_ColumnHeaders;
		private IList<IDictionary<string, string>> m_PlayerPool;
	}
}
