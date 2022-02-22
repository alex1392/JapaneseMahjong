using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class Game
	{
		public int FieldID { get; set; } // 1-2: ES
		public int RoundID { get; set; } // 1-4: ESWN
		public int StrikeCount { get; set; }
		public int ActiveID { get; set; }
		public Player ActivePlayer => Players[ActiveID];
		public IList<Player> Players { get; private set; }
		public ObservableQueue<Tile> Wall { get; private set; } = new ObservableQueue<Tile>();
		public ObservableCollection<Tile> Sea { get; private set; } = new ObservableCollection<Tile>();
		public bool IsOver { get; set; }
		public Game()
		{
			Players = new List<Player>
			{
				new Player(0),
				new Player(1),
				new Player(2),
				new Player(3),
			};
			IsOver = false;
			FieldID = 1;
			RoundID = 1;
			StrikeCount = 0;
		}

		public void Deal()
		{
			var set = TileFactory.GetFullSet();
			var rand = new Random();
			// shuffle tiles, NEED to put ToList() here, otherwise rand.Next() will be called every time the query executes
			set = set.OrderBy(t => rand.Next()).ToList();
			for (int i = 0; i < Players.Count; i++) {
				Players[i].NewHand(set.Skip(13 * i).Take(13));
			}

			foreach (var tile in set.Skip(13 * 4).Take(70)) {
				Wall.Enqueue(tile);
			}
			foreach (var item in set.TakeLast(14)) {
				Sea.Add(item);
			}
		}

		public async Task RunAsync()
		{
			Deal();
			while (!IsOver) {
				if (ActivePlayer.IsNeedDraw()) {
					ActivePlayer.Draw(Wall.Dequeue());
				}
				// check if can self call 
				while (ActivePlayer.CheckSelfCall(Wall) && !ActivePlayer.SelfActions.Keys.All(call => call == CallType.None)) {
					// push action list to the user
					ActivePlayer.SelfCall = new TaskCompletionSource<CallType>();
					var action = await ActivePlayer.SelfCall.Task;
					if (action == CallType.None) {
						break;
					}
					// if called
					if (action == CallType.Tsumo) {
						// Active Player wins, game over, next round

						IsOver = true;
						return;
					}
					if (action == CallType.Kan) {
						Tile tile;
						if (ActivePlayer.SelfActions[action].Count == 1) {
							tile = ActivePlayer.SelfActions[action][0];
						} else if (ActivePlayer.SelfActions[action].Count > 1) {
							// push tile list to the user
							ActivePlayer.KanTile = new TaskCompletionSource<Tile>();
							tile = await ActivePlayer.KanTile.Task;
						} else {
							throw new Exception();
						}
						// do action with the tile
						// draw from the sea
						ActivePlayer.CloseKan(tile);
					} else if (action == CallType.Riichi) {
						// 
					}
				}
				// wait for user to discard a tile
				ActivePlayer.DiscardTile = new TaskCompletionSource<(Tile,bool)>();
				var (discardTile,fromDraw) = await ActivePlayer.DiscardTile.Task;
				ActivePlayer.Discard(discardTile, fromDraw);
				// check if anyone is able to call
				foreach (var player in Players) {
					var callType = player.CheckCall(ActivePlayer, discardTile);
					if (callType != CallType.None) {
						// push action list to the user

						// wait for the user to decide whether to call

					}
				}
				// wait for all user

				// resolve which action has the highest priority

				// if not call, move to the next player
				//ActiveID++;
				// if called, move to the called player
				//ActiveID = 3;
				//ActivePlayer.Hand.Add(discardTile);
			}
		}

		public void NextRound()
		{
			// if there's a winning strike
			StrikeCount++;
			// if no winning strike
			StrikeCount = 0;
			RoundID = RoundID == 4 ? 1 : RoundID + 1;
			FieldID = RoundID == 4 ? FieldID + 1 : FieldID;
		}
	}
}
