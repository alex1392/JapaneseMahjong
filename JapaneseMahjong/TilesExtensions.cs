using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public static class TilesExtensions
	{
		public static string GetString(this IEnumerable<Tile> tiles, bool isCompact = false)
		{
			if (!isCompact) {
				return string.Join(null, tiles.Select(t => t.ToString()));
			}
			return string.Join(' ', tiles.GroupBy(t => t.Suit).Select(g => string.Join(null, g.Select(t => t.Value.ToString())) + g.Key.ToString().ToLower()[0]));
		}

		public static string GetString(this IEnumerable<Group> groups, bool isCompact = false)
		{
			groups = groups.OrderBy(g => g.Tiles.First().SortCode);
			if (!isCompact) {
				return string.Join(' ', groups.Select(group => group.GetString())); 
			}
			return string.Join(' ', groups.GroupBy(g => g.Tiles.First().Suit).Select(igroup => string.Join(' ', igroup.Select(g => g.GetString(true)))));
		}

		public static string GetString(this IGroup group, bool isCompact = false)
		{
			if (!isCompact) {
				return group.Tiles.GetString();
			}
			return string.Join(null, group.Tiles.Select(t => t.Value.ToString())) + group.Tiles.First().Suit.ToString().ToLower()[0];
		}
	}
}
