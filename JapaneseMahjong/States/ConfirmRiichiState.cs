using System;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class ConfirmRiichiState : IGameState
	{
		private readonly Tile tile;
		private readonly IGameState prevState;

		public ConfirmRiichiState(Tile tile, IGameState prevState)
		{
			this.tile = tile;
			this.prevState = prevState;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			var isConfirm = await game.ActivePlayer.ConfirmRiichiAsync(tile);
			if (!isConfirm) {
				return prevState;
			}
			// TODO: player in riichi state
			return new DiscardTileState(tile);
		}
	}
}
