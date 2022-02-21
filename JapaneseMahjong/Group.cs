using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public struct Group : IEquatable<Group>, IFullGroup
	{
		public GroupType Type => Tiles.Distinct().Count() != 1
				? GroupType.Sequence
				: ((Tiles.Count()) switch
				{
					2 => GroupType.Pair,
					3 => GroupType.Triplet,
					4 => GroupType.Quad,
					_ => throw new Exception(),
				});
		public IEnumerable<Tile> Tiles { get; }

		// ex: 1m2m3m
		// ex: 123m
		public Group(string groupStr, bool isCompact = false)
		{
			if (!isCompact) {
				Tiles = TileFactory.GetTiles(groupStr);
			} else {
				var suit = Enum.GetValues(typeof(Suit)).Cast<Suit>().First(s => s.ToString().ToLower()[0] == groupStr[^1]);
				var values = groupStr[0..^1].Select(c => int.Parse(c.ToString()));
				Tiles = values.Select(v => new Tile(v, suit));
			}
		}
		public Group(IEnumerable<Tile> tiles)
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
			return obj is Group && Equals((Group)obj);
		}

		public bool Equals(Group other)
		{
			return other != null &&
				   Tiles.Except(other.Tiles).Count() == 0;
		}

		public override int GetHashCode()
		{
			var hash = new HashCode();
			foreach (var tile in Tiles) {
				hash.Add(tile);
			}
			return hash.ToHashCode();
		}

		public static bool operator ==(Group left, Group right)
		{
			return EqualityComparer<Group>.Default.Equals(left, right);
		}

		public static bool operator !=(Group left, Group right)
		{
			return !(left == right);
		}
		#endregion
	}

	public enum GroupType
	{
		Pair,
		Sequence,
		Triplet,
		Quad,
	}
}
