using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class CloseKanState : IGameState
	{
		private readonly Tile tile;

		public CloseKanState(Tile tile)
		{
			this.tile = tile;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			game.ActivePlayer.CloseKan(tile);
			return await Task.Run(() => new DrawTileState(true));
		}
	}
}
