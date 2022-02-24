using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JapaneseMahjong
{
	public class OpenGroup : FullGroup
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
			ObtainIndex = obtainIndex;
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
