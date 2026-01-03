namespace WFC.Core;

public class Configuration
{
	public IList<ProtoTile> ProtoTiles { get; }

	public Configuration(IList<ProtoTile> protoTiles, AdjacencyAlgorithmKind adjacencyAlgorithmKind)
	{
		ProtoTiles = protoTiles;

		switch (adjacencyAlgorithmKind)
		{
			case AdjacencyAlgorithmKind.ADJACENCY_2D:
				_getAdjacencyIndex = AdjacencyAlgorithms2d.GetAdjacencyIndex;
				_getAdjacentIndices = AdjacencyAlgorithms2d.GetAdjacentIndices;
				break;
			case AdjacencyAlgorithmKind.ADJACENCY_3D:
			case AdjacencyAlgorithmKind.ADJACENCY_HEX:
			default:
				throw new NotImplementedException($"Adjacency algorithm kind {adjacencyAlgorithmKind} is not yet implemented!");
		}
	}

	public int GetAdjacencyIndex(Tile tile, Tile neighbor) => _getAdjacencyIndex(tile, neighbor);
	public IList<int> GetAdjacentIndices(Output output, int x, int y, int z) => _getAdjacentIndices(output, x, y, z);

	public ProtoTile PickProtoTile(IList<ProtoTile> possibleProtoTiles, Random random)
	{
		var weightSum = possibleProtoTiles.Sum(x => x.Weight);
		var weightedIndex = random.Next(weightSum);
		var currentWeight = 0;
		foreach (var protoTile in possibleProtoTiles)
		{
			currentWeight += protoTile.Weight;
			if (weightedIndex < currentWeight)
				return protoTile;
		}

		return possibleProtoTiles.LastOrDefault();
	}

	private Func<Output, int, int, int, IList<int>> _getAdjacentIndices;
	private Func<Tile, Tile, int> _getAdjacencyIndex;
}
