using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	[AddINotifyPropertyChangedInterface]
	public abstract class PlayerBase
	{
		public ObservableCollection<Tile> Hand { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<Tile> River { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<IFullGroup> Opens { get; } = new ObservableCollection<IFullGroup>();
		public IDictionary<IEnumerable<Tile>, IEnumerable<IGroup>> ReadyDict { get; private set; } = new Dictionary<IEnumerable<Tile>, IEnumerable<IGroup>>();
		public int ID { get; } // 1-4: ESWN
		public int Points { get; set; }


		public PlayerBase(int id)
		{
			Debug.Assert(id >= 0 && id <= 3);
			ID = id;
		}
		public void Discard(Tile tile) // discard one tile to the river
		{
			if (!Hand.Remove(tile)) {
				throw new Exception();
			}
			River.Add(tile);
			ReadyDict = Yaku.GetReadyTiles(Hand);
		}

		public void Draw(Tile tile) // get one tile from the wall
		{
			Hand.Add(tile);
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

		public bool IsNeedDraw() => (Opens.Count() * 3) + Hand.Count() < 14;

		/// <param name="wall">Only used for check if there's enough tiles left.</param>
		public IDictionary<SelfCallType, List<Tile>> CheckSelfCall(IEnumerable<Tile> wall)
		{
			var actions = new Dictionary<SelfCallType, List<Tile>>
			{
				{ SelfCallType.None, null }
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
				if (actions.ContainsKey(SelfCallType.Kan)) {
					actions[SelfCallType.Kan].Add(tile);
				} else {
					actions.Add(SelfCallType.Kan, new List<Tile> { tile });
				}
			}
			// check if can add Kan
			foreach (var open in Opens.Where(o => o.Type == GroupType.Triplet)) {
				var tile = open.Tiles.First();
				if (!Hand.Any(t => t == tile)) {
					continue;
				}
				// can add Kan
				if (actions.ContainsKey(SelfCallType.Kan)) {
					actions[SelfCallType.Kan].Add(tile);
				} else {
					actions.Add(SelfCallType.Kan, new List<Tile> { tile });
				}
			}
			// check if tsumo
			if (ReadyDict.Keys.SelectMany(tiles => tiles).Any(t => t == Hand.Last())) {
				actions.Add(SelfCallType.Tsumo, null);
			}
			// check if can riichi
			if (Opens.All(g => !(g is OpenGroup)) &&
				Points >= 1000 &&
				ReadyDict.Count > 0 &&
				wall.Count() >= 4) {
				actions.Add(SelfCallType.Riichi, null);
			}
			return actions;
		}


		public void Pon(PlayerBase player) // call a tile from another playerd
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

		public bool CheckCall(PlayerBase player, Tile tile)
		{
			Debug.Assert(player != this);
			// check if can Chi (only from the previous player)

			// check if can Pon

			// check if can open Kan

			// check if can Ron

			throw new NotImplementedException();
		}
		public abstract Task<Tile> DecideDiscardTileAsync();
		public abstract Task<SelfCallType> DecideSelfCallAsync(ICollection<SelfCallType> keys);
		public abstract Task<CallType> DecideCallAsync();
		public abstract Task<Tile> DecideKanTileAsync(IEnumerable<Tile> list);
		public abstract Task<Tile> DecideRiichiTileAsync(IEnumerable<Tile> list);
		public abstract Task<bool> ConfirmRiichiAsync(Tile tile);
	}
}