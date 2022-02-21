using System;
using System.Collections.Generic;
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
		public Stack<Tile> Wall { get; private set; }
		public IList<Tile> Sea { get; private set; }
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
			var p0 = set.Skip(13 * 0).Take(13);
			var p1 = set.Skip(13 * 1).Take(13);
			var p2 = set.Skip(13 * 2).Take(13);
			var p3 = set.Skip(13 * 3).Take(13);
			Players[0].NewHand(p0);
			Players[1].NewHand(p1);
			Players[2].NewHand(p2);
			Players[3].NewHand(p3);

			Wall = new Stack<Tile>(set.Skip(13 * 4).Take(70));
			Sea = set.TakeLast(14).ToList();
		}

		public async Task RunAsync()
		{
			Deal();
			while (!IsOver) {
				if (ActivePlayer.IsNeedDraw()) {
					ActivePlayer.Draw(Wall.Pop());
				}
				// check if can self call 
				while (ActivePlayer.CheckSelfCall() != CallType.None) {
					// push action list to the user

					// if called
					// make open group
					// draw from the sea
				}
				// wait for user to discard a tile
				ActivePlayer.DiscardTile = new TaskCompletionSource<Tile>();
				var discardTile = await ActivePlayer.DiscardTile.Task;
				ActivePlayer.Discard(discardTile);
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
