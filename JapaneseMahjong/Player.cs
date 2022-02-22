using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class Player : INotifyPropertyChanged
	{
		public int ID { get; } // 1-4: ESWN
		public IDictionary<IEnumerable<Tile>, IEnumerable<IGroup>> ReadyDict { get; private set; } = new Dictionary<IEnumerable<Tile>, IEnumerable<IGroup>>();
		public Tile DrawedTile { get; private set; }
		public ObservableCollection<Tile> Hand { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<Tile> River { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<IFullGroup> Opens { get; } = new ObservableCollection<IFullGroup>();
		public TaskCompletionSource<(Tile,bool)> DiscardTile { get; set; }
		public TaskCompletionSource<CallType> SelfCall { get; set; }
		public TaskCompletionSource<Tile> KanTile { get; set; }
		public IDictionary<CallType, List<Tile>> SelfActions { get; set; }
		public int Points { get; set; }

		public Player(int id)
		{
			Debug.Assert(id >= 0 && id <= 3);
			ID = id;
		}

		public event PropertyChangedEventHandler PropertyChanged;

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
			DrawedTile = tile;
		}

		public void Discard(Tile tile, bool fromDraw) // discard one tile to the river
		{
			if (!fromDraw) {
				if (!Hand.Remove(tile)) {
					throw new Exception();
				}
				Hand.Add(DrawedTile);
			}
			River.Add(tile);
			DrawedTile = default;
			ReadyDict = Yaku.GetReadyTiles(Hand);
		}


		public bool IsNeedDraw() => (Opens.Count() * 3) + Hand.Count() < 14;

		public bool CheckSelfCall(IEnumerable<Tile> wall)
		{
			var actions = new Dictionary<CallType, List<Tile>>
			{
				{ CallType.None, null }
			};
			// check if can close Kan
			var dict = new Dictionary<Tile, int>();
			foreach (var tile in Hand) {
				if (dict.ContainsKey(tile)) {
					dict[tile]++;
				} else {
					dict.Add(tile, 1);
				}
			}
			foreach (var (tile, count) in dict) {
				if (count != 4) {
					continue;
				}
				// can close Kan
				if (actions.ContainsKey(CallType.Kan)) {
					actions[CallType.Kan].Add(tile);
				} else {
					actions.Add(CallType.Kan, new List<Tile> { tile });
				}
			}
			// check if can add Kan
			foreach (var open in Opens.Where(o => o.Type == GroupType.Triplet)) {
				var tile = open.Tiles.First();
				if (!Hand.Any(t => t == tile)) {
					continue;
				}
				// can add Kan
				if (actions.ContainsKey(CallType.Kan)) {
					actions[CallType.Kan].Add(tile);
				} else {
					actions.Add(CallType.Kan, new List<Tile> { tile });
				}
			}
			// check if tsumo
			if (ReadyDict.Keys.SelectMany(tiles => tiles).Any(t => t == DrawedTile)) {
				actions.Add(CallType.Tsumo, null);
			}
			// check if can riichi
			if (Opens.All(g => !(g is OpenGroup)) && 
				Points >= 1000 && 
				ReadyDict.Count > 0 && 
				wall.Count() >= 4) {
				actions.Add(CallType.Riichi, null);
			}
			SelfActions = actions;
			return true;
		}

		public void Pon(Player player) // call a tile from another playerd
		{
			var tile = player.River.Last();
			var group = Hand.TakeWhile(t => t == tile).Append(tile);
			Opens.Add(new OpenGroup(group, ID - player.ID));
			Hand.Remove(tile);
			Hand.Remove(tile);
			Hand.Remove(tile);
		}
		public void CloseKan(Tile tile)
		{
			throw new NotImplementedException();
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
		None, Chi, Pon, Kan, Ron, Tsumo, Riichi
	}
}
