using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JapaneseMahjong
{
	public class Player
	{
		public int ID { get; } // 1-4: ESWN
		public Tiles Hand { get; private set; }
		public IList<Tile> River { get; } = new List<Tile>();
		public IList<OpenGroup> OpenGroups { get; } = new List<OpenGroup>();

		public Player(int id)
		{
			Debug.Assert(id >= 1 && id <= 4);
			ID = id;
		}

		public void Reset()
		{
			Hand.Clear();
			OpenGroups.Clear();
			River.Clear();
		}

		public void NewHand(IEnumerable<Tile> tiles)
		{
			Hand = new Tiles(tiles);
		}

		public void Draw(Tile tile) // get one tile from the wall
		{
			Hand.Add(tile);
		}

		public void Discard(Tile tile) // discard one tile to the river
		{
			Hand.Remove(tile);
			River.Add(tile);
		}

		public void Pon(Player player) // call a tile from another player
		{
			var tile = player.River.Last();
			var group = Hand.TakeWhile(t => t == tile).Append(tile);
			OpenGroups.Add(new OpenGroup(GroupType.Triplet, group, ID - player.ID));
			while (Hand.Remove(tile)) { }
		}
	}

	public enum CallType
	{
		Chi, Pon, Kan, Ron
	}
}
