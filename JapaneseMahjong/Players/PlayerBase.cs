using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public abstract class PlayerBase : INotifyPropertyChanged
	{
		/// <summary>
		/// After drawing a tile, it will be put in <see cref="DrawedTile"/> at first. Only after discard a tile from hand, that <see cref="DrawedTile"/> will be added into the hand.
		/// </summary>
		public ObservableCollection<Tile> Hand { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<Tile> River { get; } = new ObservableCollection<Tile>();
		public ObservableCollection<IFullGroup> Opens { get; } = new ObservableCollection<IFullGroup>();
		public IDictionary<IEnumerable<Tile>, IEnumerable<IGroup>> ReadyDict { get; private set; } = new Dictionary<IEnumerable<Tile>, IEnumerable<IGroup>>();
		public int ID { get; } // 1-4: ESWN
		public int Points { get; set; }
		public Tile DrawedTile { get; private set; }

		public PlayerBase()
		{
			Hand.CollectionChanged += Hand_CollectionChanged;
		}


		public PlayerBase(int id) : this()
		{
			Debug.Assert(id >= 0 && id <= 3);
			ID = id;
		}


		#region Events
		private void Hand_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(DrawedTile));
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		} 
		#endregion

		#region Checks
		public bool IsNeedDraw() => (Opens.Count() * 3) + Hand.Count() < 14;

		public IEnumerable<Tile> CheckCloseKan()
		{
			var dict = new Dictionary<Tile, int>(new TileComparer());
			foreach (var tile in Hand) {
				if (dict.ContainsKey(tile)) {
					dict[tile]++;
				} else {
					dict.Add(tile, 1);
				}
			}
			var list = new List<Tile>();
			foreach (var (tile, count) in dict) {
				if (count != 4) {
					continue;
				}
				list.Add(tile);
			}
			return list;
		}
		public IEnumerable<Tile> CheckAddKan()
		{
			var list = new List<Tile>();
			foreach (var open in Opens.Where(o => o.Type == FullGroupType.Triplet)) {
				var tile = open.Tiles.First();
				if (!Hand.Any(t => t == tile)) {
					continue;
				}
				list.Add(tile);
			}
			return list;
		}
		public IEnumerable<Tile> CheckRiichi(IEnumerable<Tile> wall)
		{
			if (Opens.All(g => !(g is OpenGroup)) &&
				Points >= 1000 &&
				//ReadyDict.Count > 0 && // need to check when there's 14 tiles
				wall.Count() >= 4) {
				// TODO: add riichi tiles
			}
			return new List<Tile>();
		}
		public bool CheckTsumo()
		{
			if (ReadyDict.Keys.SelectMany(tiles => tiles).Any(t => t == DrawedTile)) {
				// TODO: check if there's any valid yaku
				return true;
			}
			return false;
		}

		public IDictionary<CallType, List<Tile>> CheckCall(PlayerBase player, Tile tile)
		{
			Debug.Assert(player != this);
			var actions = new Dictionary<CallType, List<Tile>>
			{
				{ CallType.None, null }
			};
			var obtainIndex = ID - player.ID;
			var isPrevPlayer = obtainIndex == -1 || obtainIndex == +3;
			// check if can Ron
			if (ReadyDict.Keys.SelectMany(tiles => tiles).Any(t => t == tile)) {
				// TODO: check if there's any valid yaku
				actions.Add(CallType.Ron, null);
			}
			// check if can Pon
			if (Hand.Where(t => t == tile).Count() >= 3) {
				actions.Add(CallType.Pon, null);
			}
			// check if can open Kan
			if (Opens.Any(g => g.Type == FullGroupType.Triplet && g.Tiles.First() == tile)) {
				actions.Add(CallType.Kan, null);
			}
			// check if can Chi (only from the previous player)
			if (isPrevPlayer && tile.Suit != Suit.Honor) {
				// ex tile = 3
				// check for 12
				// check for 24
				// check for 45

				var sameSuit = Hand.Where(t => t.Suit == tile.Suit);
				var tile_2 = sameSuit.FirstOrDefault(t => t.Value == tile.Value - 2);
				var tile_1 = sameSuit.FirstOrDefault(t => t.Value == tile.Value - 1);
				var tile1 = sameSuit.FirstOrDefault(t => t.Value == tile.Value + 1);
				var tile2 = sameSuit.FirstOrDefault(t => t.Value == tile.Value + 2);
				//var list
				if (tile_2 != default && tile_1 != default) {

				}
			}

			return actions;
		}
		#endregion

		#region Actions
		public void Reset()
		{
			Hand.Clear();
			Opens.Clear();
			River.Clear();
		}

		public void Deal(IEnumerable<Tile> tiles)
		{
			Opens.Clear();
			River.Clear();
			Hand.Clear();
			foreach (var tile in tiles) {
				Hand.Add(tile);
			}
		}
		public void Discard(Tile tile)
		{
			if (tile != DrawedTile) {
				var result = Hand.Remove(tile);
				Debug.Assert(result == true);
				Hand.Add(DrawedTile);
			}
			DrawedTile = default;
			River.Add(tile);
			ReadyDict = Yaku.GetReadyTiles(Hand);
		}

		public void Draw(Tile tile) 
		{
			DrawedTile = tile;
		}
		public void Pon(PlayerBase player) // call a tile from another player
		{
			var tile = player.River.Last();
			var tiles = Hand.Where(t => t == tile).Take(2);
			var group = tiles.Append(tile).ToList(); // need to make a list before removing the item from the original collection
			Opens.Add(new OpenGroup(group, ID - player.ID));
			foreach (var t in tiles) {
				Hand.Remove(t);
			}
		}
		public void CloseKan(Tile tile)
		{
			var group = Hand.TakeWhile(t => t == tile).ToList(); // need to make a list before removing the item from the original collection
			Opens.Add(new FullGroup(group));
			foreach (var t in group) {
				Hand.Remove(t);
			}
		}
		public void AddKan(Tile tile)
		{
			var group = Opens.Where(g => g is OpenGroup &&
										g.Type == FullGroupType.Triplet)
							.First(g => g.Tiles.First() == tile) as OpenGroup;
			group.AddKan(tile);
			Hand.Remove(tile);
		}
		#endregion
		
		#region Decision making interface
		public abstract Task<Tile> DecideDiscardTileAsync();
		public abstract Task<SelfCallType> DecideSelfCallAsync(IEnumerable<SelfCallType> options);
		public abstract Task<CallType> DecideCallAsync();
		public abstract Task<Tile> DecideKanTileAsync(IEnumerable<Tile> list);
		public abstract Task<Tile> DecideRiichiTileAsync(IEnumerable<Tile> list);
		public abstract Task<bool> ConfirmRiichiAsync(Tile tile);

		#endregion
	}
}