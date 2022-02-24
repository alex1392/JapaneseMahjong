using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class DecideDiscardState : IGameState
	{
		public async Task<IGameState> UpdateAsync(Game game)
		{
			var tile = await game.ActivePlayer.DecideDiscardTileAsync();
			return new DiscardTileState(tile);
		}
	}
}
