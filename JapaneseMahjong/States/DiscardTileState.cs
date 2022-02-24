using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DiscardTileState : IGameState
	{
		private readonly Tile tile;

		public DiscardTileState(Tile tile)
		{
			this.tile = tile;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			game.ActivePlayer.Discard(tile);
			return await Task.Run(() => new DecideCallState());
		}
	}
}
