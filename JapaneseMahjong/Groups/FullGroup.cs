using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class FullGroup : IEquatable<FullGroup>, IFullGroup
	{
		public FullGroupType Type => Tiles.Distinct().Count() != 1
				? FullGroupType.Sequence
				: ((Tiles.Count()) switch
				{
					2 => FullGroupType.Pair,
					3 => FullGroupType.Triplet,
					4 => FullGroupType.Quad,
					_ => throw new Exception(),
				});
		public IEnumerable<Tile> Tiles { get; protected set; }


		// ex: 1m2m3m
		// ex: 123m
		public FullGroup(string groupStr, bool isCompact = false)
		{
			if (!isCompact) {
				Tiles = TileFactory.GetTiles(groupStr);
			} else {
				var suit = Enum.GetValues(typeof(Suit)).Cast<Suit>().First(s => s.ToString().ToLower()[0] == groupStr[^1]);
				var values = groupStr[0..^1].Select(c => int.Parse(c.ToString()));
				Tiles = values.Select(v => new Tile(v, suit));
			}
		}
		public FullGroup(IEnumerable<Tile> tiles)
		{
			Tiles = tiles;
		}

		public override string ToString()
		{
			return this.GetString(true);
		}
		

		#region Implement IEquatable

		public override bool Equals(object obj)
		{
			return obj is FullGroup && Equals((FullGroup)obj);
		}

		public bool Equals(FullGroup other)
		{
			return other != null &&
				   !Tiles.Except(other.Tiles).Any();
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			foreach (var tile in Tiles) {
				hash.Add(tile);
			}
			return hash.ToHashCode();
		}

		public static bool operator ==(FullGroup left, FullGroup right)
		{
			return EqualityComparer<FullGroup>.Default.Equals(left, right);
		}

		public static bool operator !=(FullGroup left, FullGroup right)
		{
			return !(left == right);
		}
		#endregion
	}

	public enum FullGroupType
	{
		Pair,
		Sequence,
		Triplet,
		Quad,
	}
}
