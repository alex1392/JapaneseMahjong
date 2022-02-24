﻿using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DrawTileState : IGameState
	{

		public DrawTileState(bool isFromSea)
		{
			IsFromSea = isFromSea;
		}

		public bool IsFromSea { get; set; } = false;
		public async Task<IGameState> UpdateAsync(Game game)
		{
			if (game.ActivePlayer.IsNeedDraw()) {
				game.ActivePlayer.Draw(IsFromSea ? game.Sea.Pop() : game.Wall.Dequeue());
			}
			return await Task.Run(() => new DecideSelfCallState());
		}
	}
}
