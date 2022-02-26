using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideAddKanTileState : IGameState
	{
		private readonly IEnumerable<Tile> tiles;
		private readonly IGameState prevState;

		public DecideAddKanTileState(IEnumerable<Tile> tiles, IGameState prevState)
		{
			this.tiles = tiles;
			this.prevState = prevState;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			if (tiles.Count() == 1) {
				return new AddKanState(tiles.First());
			}

			var tile = await game.ActivePlayer.DecideKanTileAsync(tiles);
			if (tile == default) {
				return prevState;
			}
			return new AddKanState(tile);
		}
	}
}