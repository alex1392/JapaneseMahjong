using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class Player
	{
		public int ID { get; } // 1-4: ESWN
		public IList<Tile> Hand { get; private set; }
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

		public void NewHand(IList<Tile> tiles)
		{
			Hand = tiles;
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

		public void Pon(Player player) // call a tile from another playerd
		{
			var tile = player.River.Last();
			var group = Hand.TakeWhile(t => t == tile).Append(tile);
			OpenGroups.Add(new OpenGroup(GroupType.Triplet, group, ID - player.ID));
			while (Hand.Remove(tile)) { }
		}

		public bool IsNeedDraw() => (OpenGroups.Count() * 3) + Hand.Count() < 14;

		public CallType CheckSelfCall()
		{
			// check if can close Kan

			// check if can add Kan

			return CallType.None;
		}

		public CallType CheckCall(Player player, Tile tile)
		{
			if (player == this) {
				return CallType.None;
			}
			// check if can Chi (only from the previous player)

			// check if can Pon

			// check if can open Kan

			// check if can Ron

			return CallType.None;
		}

		public void SortHand()
		{
			Hand = Hand.OrderBy(t => t.GetOrderCode()).ToList();
		}

		public async Task<Tile> DiscardAsync()
		{
			return await Task.Run(() => 
			{
				Console.WriteLine("Enter the index of tile you want to discard.");
				var input = Console.ReadLine();
				var index = int.Parse(input);
				return Hand[index];
			});
		}
	}

	public enum CallType
	{
		None, Chi, Pon, Kan, Ron
	}
}
