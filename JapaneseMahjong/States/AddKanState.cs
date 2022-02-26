using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class AddKanState : IGameState
	{
		private readonly Tile tile;

		public AddKanState(Tile tile)
		{
			this.tile = tile;
		}

		public async Task<IGameState> UpdateAsync(Game game)
		{
			game.ActivePlayer.AddKan(tile);
			return await Task.Run(() => new DrawTileState(true));
		}
	}
}
