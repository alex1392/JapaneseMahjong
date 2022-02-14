using System;
using System.Collections.Generic;
using System.Text;

namespace JapaneseMahjong
{
	public class Game
	{
		public int FieldID { get; set; } // 1-2: ES
		public int RoundID { get; set; } // 1-4: ESWN
		public int StrikeCount { get; set; }
		public IList<Player> Players { get; private set; }
		public IEnumerable<Tile> Wall { get; private set; }
		public IEnumerable<Tile> Sea { get; private set; }
		public bool IsOver { get; set; }
		public Game()
		{
			// initialise players and tiles
			Players = new List<Player>();
			var (playerTiles, wall, sea) = TilesFactory.Deal();
			for (var i = 0; i < 4; i++) {
				Players.Add(new Player(i));
				Players[i].NewHand(playerTiles[i]);
			}
			Wall = wall;
			Sea = sea;
			IsOver = false;
			FieldID = 1;
			RoundID = 1;
			StrikeCount = 0;
		}

		public void Start()
		{
			while (!IsOver) {

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
