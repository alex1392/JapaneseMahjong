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
		public int FieldID { get; private set; } // 1-2: ES
		public int RoundID { get; private set; } // 1-4: ESWN
		public int StrikeCount { get; private set; }
		public int ActiveID { get; private set; }
		public PlayerBase ActivePlayer => Players[ActiveID];
		public IList<PlayerBase> Players { get; private set; }
		public ObservableQueue<Tile> Wall { get; private set; } = new ObservableQueue<Tile>();
		public ObservableStack<Tile> Sea { get; private set; } = new ObservableStack<Tile>();
		public bool IsOver { get; set; }
		public IGameState State { get; private set; } = new DrawTileState();
		public Game()
		{
			Players = new List<PlayerBase>
			{
				new HumanPlayer(0),
				new ComputerPlayer(1),
				new ComputerPlayer(2),
				new ComputerPlayer(3),
			};
			IsOver = false;
			FieldID = 1;
			RoundID = 1;
			StrikeCount = 0;
		}

		public virtual void Deal()
		{
			var set = TileFactory.GetFullSet();
			var rand = new Random();
			// shuffle tiles, NEED to put ToList() here, otherwise rand.Next() will be called every time the query executes
			set = set.OrderBy(t => rand.Next()).ToList();
			for (int i = 0; i < Players.Count; i++) {
				Players[i].Deal(set.Skip(13 * i).Take(13));
			}

			foreach (var tile in set.Skip(13 * 4).Take(70)) {
				Wall.Enqueue(tile);
			}
			foreach (var item in set.TakeLast(14)) {
				Sea.Push(item);
			}
		}

		public async Task RunAsync()
		{
			while (!(State is EndState)) {
				State = await State.UpdateAsync(this);
			}


			// check if anyone is able to call
			//foreach (var player in Players) {
			//	if (player.CheckCall(ActivePlayer, discardTile)) {
			//		// push action list to the user

			//		// wait for the user to decide whether to call

			//	}
			//}
			// wait for all user

			// resolve which action has the highest priority

			// if not call, move to the next player
			//ActiveID++;
			// if called, move to the called player
			//ActiveID = 3;
			//ActivePlayer.Hand.Add(discardTile);
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

	public class GameStub : Game
	{
		public override void Deal()
		{
			var set = TileFactory.GetFullSet();
			// shuffle tiles, NEED to put ToList() here, otherwise rand.Next() will be called every time the query executes
			set = set.ToList();
			for (int i = 0; i < Players.Count; i++) {
				Players[i].Deal(set.Skip(13 * i).Take(13));
			}

			foreach (var tile in set.Skip(13 * 4).Take(70)) {
				Wall.Enqueue(tile);
			}
			foreach (var item in set.TakeLast(14)) {
				Sea.Push(item);
			}
		}
	}
}
