using System;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public class EndState : IGameState
	{
		public Task<IGameState> UpdateAsync(Game game)
		{
			game.IsOver = true;
			throw new NotImplementedException();
		}
	}
}
