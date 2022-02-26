using System.Collections.Generic;

namespace JapaneseMahjong
{
	public interface IGroup
	{
		IEnumerable<Tile> Tiles { get; }
	}

	public interface IFullGroup : IGroup
	{
		FullGroupType Type { get; }
	}
}