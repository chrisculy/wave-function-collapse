namespace WFC.Core;

public interface IOutput
{
	bool IsComplete();
	ITile GetTileWithLeastEntropy(Random _random);
	IReadOnlyList<ITile> GetUndecidedNeighbors(ITile currentTile);
	void CollapseTile(ITile currentTile, IProtoTile protoTile);
}

public class MockOutput : IOutput
{
	public void CollapseTile(ITile currentTile, IProtoTile protoTile)
	{
		currentTile.ProtoTile = protoTile;
		currentTile.PossibleProtoTiles.Clear();
	}

	public ITile GetTileWithLeastEntropy(Random _random)
	{
		throw new NotImplementedException();
	}

	public IReadOnlyList<ITile> GetUndecidedNeighbors(ITile currentTile)
	{
		throw new NotImplementedException();
	}

	public bool IsComplete()
	{
		throw new NotImplementedException();
	}
}