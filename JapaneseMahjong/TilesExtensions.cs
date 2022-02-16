using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public static class TilesExtensions
	{
		public static string GetString(this IEnumerable<Tile> tiles) => string.Join(null, tiles.Select(t => t.ToString()));
	}
}
