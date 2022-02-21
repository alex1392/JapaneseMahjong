using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong
{
	public struct OpenGroup : IFullGroup
	{
		/// <summary>
		/// index = -1 : obtain from the next player
		/// index = +1/-3 : obtain from the previous player
		/// index = +2/-2 : obtain from the opposite player
		/// </summary>
		public int ObtainIndex { get; }

		public IEnumerable<Tile> Tiles { get; private set; }

		public GroupType Type { get; private set; }

		public OpenGroup(IEnumerable<Tile> tiles, int obtainIndex)
		{
			Debug.Assert(obtainIndex >= -3 && obtainIndex <= 2);
			Tiles = new List<Tile>(tiles);
			ObtainIndex = obtainIndex;

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

		public void AddedQuad(Tile tile)
		{
			Debug.Assert(Type == GroupType.Triplet);
			Debug.Assert(tile == Tiles.First());
			Tiles = Tiles.Append(tile);
		}

		public override string ToString()
		{
			return this.GetString(true);
		}
	}
}
