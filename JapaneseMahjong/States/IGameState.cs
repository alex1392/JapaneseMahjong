using System.Text;
using System.Threading.Tasks;

namespace JapaneseMahjong
{
	public interface IGameState
	{
		Task<IGameState> UpdateAsync(Game game);
	}
}
