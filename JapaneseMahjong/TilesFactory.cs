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
			var types = Enum.GetValues(typeof(TileType)).Cast<TileType>();
			foreach (var type in types.Where(t => t != TileType.Honor)) {
				for (var value = 1; value <= 9; value++) {
					for (var i = 0; i < 3; i++) {
						tiles.Add(new Tile(value, type));
					}
					tiles.Add(new Tile(value, type, value == 5));
				}
			}
			for (var value = 1; value <= 7; value++) {
				for (var i = 0; i < 4; i++) {
					tiles.Add(new Tile(value, TileType.Honor));
				}
			}
			return tiles;
		}

		
	}
}
