using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JapaneseMahjong
{
	public class Player
	{
		public int ID { get; } // 1-4: ESWN
		public ObservableCollection<Tile> Hand { get; private set; } = new ObservableCollection<Tile>();
		public ObservableCollection<Tile> River { get; } = new ObservableCollection<Tile>();
		public IList<OpenGroup> Opens { get; } = new List<OpenGroup>();
		public TaskCompletionSource<Tile> DiscardTile { get; set; }

		public Player(int id)
		{
			Debug.Assert(id >= 0 && id <= 3);
			ID = id;
		}

		public void Reset()
		{
			Hand.Clear();
			Opens.Clear();
			River.Clear();
		}

		public void NewHand(IEnumerable<Tile> tiles)
		{
			Opens.Clear();
			River.Clear();
			Hand.Clear();
			foreach (var tile in tiles) {
				Hand.Add(tile);
			}
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
			Opens.Add(new OpenGroup(group, ID - player.ID));
			while (Hand.Remove(tile)) { }
		}

		public bool IsNeedDraw() => (Opens.Count() * 3) + Hand.Count() < 14;

		public CallType CheckSelfCall()
		{
			// check if can close Kan
			var i = 0;
			while (i < Hand.Count) {
				var tile = Hand[i];
				var count = Hand.Skip(i).TakeWhile(t => t == tile).Count();
				if (count == 4) {
					// can close Kan
				}
				i += count; 
			}
			// check if can add Kan
			foreach (var open in Opens.Where(o => o.Type == GroupType.Triplet)) {
				var tile = open.Tiles.First();
				if (Hand.Any(t => t == tile)) {
					// can add Kan
				}
			}
			// check if tsumo

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

	}

	public enum CallType
	{
		None, Chi, Pon, Kan, Ron
	}
}
