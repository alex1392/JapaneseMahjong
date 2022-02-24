using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecidetKanTileState : IGameState
	{
		private readonly IEnumerable<Tile> list;
		private readonly IGameState prevState;

		public DecidetKanTileState(IEnumerable<Tile> list)
		{
			this.list = list;
		}

		public DecidetKanTileState(IEnumerable<Tile> list, IGameState prevState) : this(list)
		{
			this.prevState = prevState;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{

			var tile = await game.ActivePlayer.DecideKanTileAsync(list);
			if (tile == default) {
				return prevState;
			}
			return new CloseKanState(tile);
		}
	}
}
