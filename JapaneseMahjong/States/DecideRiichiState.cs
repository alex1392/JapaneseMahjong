using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideRiichiState : IGameState
	{
		private readonly IEnumerable<Tile> list;
		private readonly IGameState prevState;

		public DecideRiichiState(IEnumerable<Tile> list)
		{
			this.list = list;
		}

		public DecideRiichiState(IEnumerable<Tile> list, IGameState prevState) : this(list)
		{
			this.prevState = prevState;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			var tile = await game.ActivePlayer.DecideRiichiTileAsync(list);
			if (tile == default) {
				return prevState;
			}
			return new ConfirmRiichiState(tile, this);
		}
	}
}
