using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class OpenGroup 
	{
		public GroupType Type { get; private set; }
		public IList<Tile> Tiles { get; } 
		/// <summary>
		/// index = -1 : obtain from the next player
		/// index = +1/-3 : obtain from the previous player
		/// index = +2/-2 : obtain from the opposite player
		/// </summary>
		public int ObtainIndex { get; }

		public OpenGroup(GroupType type, IEnumerable<Tile> tiles, int obtainIndex)
		{
			Debug.Assert(tiles.Count() == (type == GroupType.Sequence || type == GroupType.Triplet ? 3 : 4));
			Debug.Assert(obtainIndex >= -3 && obtainIndex <= 2);
			Type = type;
			Tiles = new List<Tile>(tiles);
			ObtainIndex = obtainIndex;
		}

		public void AddedQuad(Tile tile)
		{
			Debug.Assert(Type == GroupType.Triplet);
			Debug.Assert(tile == Tiles.First());
			Tiles.Add(tile);
			Type = GroupType.OpenQuad;
		}
	}

	public enum GroupType
	{
		Sequence,
		Triplet,
		OpenQuad,
		CloseQuad,
	}
}
