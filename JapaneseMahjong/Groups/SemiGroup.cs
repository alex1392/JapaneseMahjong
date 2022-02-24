using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong
{
	public class SemiGroup : IGroup
	{
		public SemiGroupType Type { get; private set; }

		public List<Tile> ReadyTiles { get; }

		public IEnumerable<Tile> Tiles { get; }
		public SemiGroup(IEnumerable<Tile> tiles)
		{
			ReadyTiles = new List<Tile>();
			Tiles = tiles;
			SetProperties();
		}

		private void SetProperties()
		{
			if (Tiles.Count() == 1) {
				ReadyTiles.Add(Tiles.First());
				Type = SemiGroupType.Single;
				return;
			}
			if (Tiles.Distinct().Count() == 1) {
				ReadyTiles.Add(Tiles.First());
				Type = SemiGroupType.Pair;
				return;
			}
			var suit = Tiles.First().Suit;
			if (suit == Suit.Honor) {
				Type = SemiGroupType.None;
				return;
			}
			if (Math.Abs(Tiles.First().Value - Tiles.Last().Value) == 1) {
				if (Tiles.Any(t => t.Value == 1)) {
					ReadyTiles.Add(new Tile(3, suit));
					Type = SemiGroupType.Side;
				} else if (Tiles.Any(t => t.Value == 9)) {
					ReadyTiles.Add(new Tile(7, suit));
					Type = SemiGroupType.Side;
				} else {
					var vmin = Tiles.Min(t => t.Value);
					var vmax = Tiles.Max(t => t.Value);
					ReadyTiles.Add(new Tile(vmin - 1, suit));
					ReadyTiles.Add(new Tile(vmax + 1, suit));
					Type = SemiGroupType.TwoSides;
				}

			} else if (Math.Abs(Tiles.First().Value - Tiles.Last().Value) == 2) {
				ReadyTiles.Add(new Tile((Tiles.First().Value + Tiles.Last().Value) / 2, suit));
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
