using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Group : IEquatable<Group>
	{
		private IEnumerable<Tile> tiles;

		public GroupType Type { get; private set; }
		public IEnumerable<Tile> Tiles
		{
			get => tiles;
			protected set
			{
				tiles = value;
				SetType();
			}
		}

		private void SetType()
		{
			Type = Tiles.Distinct().Count() != 1
				? GroupType.Sequence
				: ((Tiles.Count()) switch
				{
					2 => GroupType.Pair,
					3 => GroupType.Triplet,
					4 => GroupType.Quad,
					_ => throw new Exception(),
				});
		}

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
			return ToString(true);
		}
		public string ToString(bool isCompact = false)
		{
			if (!isCompact) {
				return Tiles.GetString();
			}
			return string.Join(null, Tiles.Select(t => t.Value.ToString())) + Tiles.First().Suit.ToString().ToLower()[0];
		}

		#region Implement IEquatable

		public override bool Equals(object obj)
		{
			return Equals(obj as Group);
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
	public class OpenGroup : Group
	{
		/// <summary>
		/// index = -1 : obtain from the next player
		/// index = +1/-3 : obtain from the previous player
		/// index = +2/-2 : obtain from the opposite player
		/// </summary>
		public int ObtainIndex { get; }

		public OpenGroup(IEnumerable<Tile> tiles, int obtainIndex) : base(tiles)
		{
			Debug.Assert(obtainIndex >= -3 && obtainIndex <= 2);
			Tiles = new List<Tile>(tiles);
			ObtainIndex = obtainIndex;
		}

		public void AddedQuad(Tile tile)
		{
			Debug.Assert(Type == GroupType.Triplet);
			Debug.Assert(tile == Tiles.First());
			Tiles = Tiles.Append(tile);
		}
	}

	public enum GroupType
	{
		Pair,
		Sequence,
		Triplet,
		Quad,
	}
}
