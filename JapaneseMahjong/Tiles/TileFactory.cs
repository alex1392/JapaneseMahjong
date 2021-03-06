using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class TileFactory
	{
		public static IEnumerable<Tile> GetFullSet()
		{
			var tiles = new List<Tile>();
			var suits = Enum.GetValues(typeof(Suit)).Cast<Suit>();
			foreach (var suit in suits.Except(new[] { Suit.Honor, Suit.None })) {
				for (var value = 1; value <= 9; value++) {
					for (var i = 0; i < 3; i++) {
						tiles.Add(new Tile(value, suit));
					}
					tiles.Add(new Tile(value, suit, value == 5));
				}
			}
			for (var value = 1; value <= 7; value++) {
				for (var i = 0; i < 4; i++) {
					tiles.Add(new Tile(value, Suit.Honor));
				}
			}
			return tiles;
		}

		public static IEnumerable<Tile> GetTiles(string tileString, bool isCompact = false)
		{
			return !isCompact ? ByString(tileString) : ByCompactString(tileString);

			IEnumerable<Tile> ByCompactString(string tileString)
			{
				var list = new List<Tile>();
				foreach (var str in tileString.Split(' ')) {
					var suitChar = str[^1];
					var suit = Enum.GetValues(typeof(Suit)).Cast<Suit>().First(suit => suit.ToString().ToLower()[0] == suitChar);
					foreach (var ch in str[0..^1]) {
						var tile = new Tile(int.Parse(ch.ToString()), suit);
						list.Add(tile);
					}
				}
				return list;
			}

			IEnumerable<Tile> ByString(string tileString)
			{
				var tileList = new List<string>();
				for (var i = 0; i < tileString.Length; i += 2) {
					tileList.Add(tileString.Substring(i, 2));
				}
				return tileList.Select(s =>
				{
					s = s.ToLower();
					var value = int.Parse(s[0].ToString());
					var isRed = value == 0;
					value = isRed ? 5 : value;
					var suit = Enum.GetValues(typeof(Suit))
						.Cast<Suit>()
						.First(suit => suit.ToString().ToLower()[0] == s[1]);
					return new Tile(value, suit, isRed);
				}).ToList(); // It's important to execute the query and return a list, otherwise the query will be executed everytime, and therefore creating new tiles.
			}
		}

		// ex: 2m3m4m 2m3m4m 2m3m4m 5m5m
		// ex: 234m 234m 234m 55m
		public static IEnumerable<FullGroup> GetGroups(string groupsString, bool isCompact = false) => groupsString.Split(' ').Select(s => new FullGroup(s, isCompact));
	}
}
