using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong
{
	public struct SemiGroup : IGroup
	{
		public SemiGroupType Type { get; }

		public List<Tile> ReadyTiles { get; }

		public IEnumerable<Tile> Tiles { get; }
		public SemiGroup(IEnumerable<Tile> tiles)
		{
			Debug.Assert(tiles.Count() <= 3);
			Debug.Assert(tiles.Select(t => t.Suit).Distinct().Count() == 1);
			ReadyTiles = new List<Tile>();
			Tiles = tiles;

			if (tiles.Count() == 1) {
				ReadyTiles.Add(tiles.First());
				Type = SemiGroupType.Single;
				return;
			}
			if (tiles.Distinct().Count() == 1) {
				ReadyTiles.Add(tiles.First());
				Type = SemiGroupType.Pair;
				return;
			}
			var suit = tiles.First().Suit;
			if (suit == Suit.Honor) {
				Type = SemiGroupType.None;
				return;
			}
			if (Math.Abs(tiles.First().Value - tiles.Last().Value) == 1) {
				if (tiles.Any(t => t.Value == 1)) {
					ReadyTiles.Add(new Tile(3, suit));
					Type = SemiGroupType.Side;
				} else if (tiles.Any(t => t.Value == 9)) {
					ReadyTiles.Add(new Tile(7, suit));
					Type = SemiGroupType.Side;
				} else {
					var vmin = tiles.Min(t => t.Value);
					var vmax = tiles.Max(t => t.Value);
					ReadyTiles.Add(new Tile(vmin - 1, suit));
					ReadyTiles.Add(new Tile(vmax + 1, suit));
					Type = SemiGroupType.TwoSides;
				}

			} else if (Math.Abs(tiles.First().Value - tiles.Last().Value) == 2) {
				ReadyTiles.Add(new Tile((tiles.First().Value + tiles.Last().Value) / 2, suit));
				Type = SemiGroupType.Middle;
			} else {
				Type = SemiGroupType.None;
			}

		}

		public override string ToString()
		{
			return this.GetString(true);
		}
	}

	public enum SemiGroupType
	{
		None,
		TwoSides,
		Middle,
		Side,
		Pair,
		Single,
	}
}
