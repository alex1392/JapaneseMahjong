using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class TilesFactory
	{
		public static IEnumerable<Tile> GetFullSet()
		{
			var tiles = new List<Tile>();
			var suits = Enum.GetValues(typeof(Suit)).Cast<Suit>();
			foreach (var suit in suits.Where(t => t != Suit.Honor)) {
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

		
	}
}
