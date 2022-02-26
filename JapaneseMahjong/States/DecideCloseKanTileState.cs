using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideCloseKanTileState : IGameState
	{
		private readonly IEnumerable<Tile> tiles;
		private readonly IGameState prevState;

		public DecideCloseKanTileState(IEnumerable<Tile> tiles, IGameState prevState)
		{
			this.tiles = tiles;
			this.prevState = prevState;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			if (tiles.Count() == 1) {
				return new CloseKanState(tiles.First());
			}

			var tile = await game.ActivePlayer.DecideKanTileAsync(tiles);
			if (tile == default) {
				return prevState;
			}
			return new CloseKanState(tile);
		}
	}
}
