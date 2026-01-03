using WFC.Core.Serialization;

namespace WFC.Core;

public class Output
{
	public int Width { get; }
	public int Height { get; }
	public int Depth { get; }

	public Output(Configuration configuration, int width, int height, int depth = 1, Func<int, int, int, IList<ProtoTile>> getInitialValidProtoTilesForPosition = null)
	{
		Width = width;
		Height = height;
		Depth = depth;
		_configuration = configuration;
		_getInitialValidProtoTilesForPosition = getInitialValidProtoTilesForPosition;

		_tiles = new Tile[Width * Height * Depth];
		for (var z = 0; z < Depth; z++)
		{
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					var initialValidProtoTiles = _getInitialValidProtoTilesForPosition is not null ? _getInitialValidProtoTilesForPosition(x, y, z) : _configuration.ProtoTiles;
					_tiles[Index(x,y,z)] = new Tile(x, y, z)
					{
						PossibleProtoTiles = initialValidProtoTiles.Count > 1 ? initialValidProtoTiles.ToList() : [],
						ProtoTile = initialValidProtoTiles.Count == 1 ? initialValidProtoTiles[0] : null
					};
				}
			}
		}
	}

	public SerializableOutput ToSerializable()
	{
		var protoTileIdToIndex = new Dictionary<string, int>();
		for (var index = 0; index < _configuration.ProtoTiles.Count; index++)
		{
			protoTileIdToIndex.Add(_configuration.ProtoTiles[index].Id, index);
		}

		return new SerializableOutput
		{
			Width = Width,
			Height = Height,
			Depth = Depth,
			ProtoTiles = _configuration.ProtoTiles.Select(x => x.Id).ToList(),
			Tiles = _tiles.Select(x => protoTileIdToIndex[x.ProtoTile.Id]).ToList(),
		};
	}

	public IReadOnlyList<Tile> GetUndecidedNeighbors(Tile tile)
	{
		return _configuration.GetAdjacentIndices(this, tile.X, tile.Y, tile.Z).Select(x => _tiles[x]).Where(tile => !TileIsDecided(tile)).ToList();
	}

	public IList<ProtoTile> GetValidNeighborProtoTiles(Tile tile, Tile neighbor)
	{
		var protoTiles = tile.ProtoTile != null ? tile.PossibleProtoTiles.Append(tile.ProtoTile) : tile.PossibleProtoTiles;
		var adjacencyIndex = _configuration.GetAdjacencyIndex(tile, neighbor);
		var validNeighborProtoTiles = new List<ProtoTile>();
		foreach (var protoTileIndex in protoTiles.SelectMany(x => x.NeighborIndices[adjacencyIndex]))
		{
			if (!validNeighborProtoTiles.Contains(_configuration.ProtoTiles[protoTileIndex]))
				validNeighborProtoTiles.Add(_configuration.ProtoTiles[protoTileIndex]);
		}

		return validNeighborProtoTiles;
	}

	public bool IsComplete()
	{
		return _tiles.All(TileIsDecided);
	}

	public Tile GetTileWithLeastEntropy(Random random)
	{
		if (_tilesWithReducedPossibleProtoTiles.Count == 0)
			return GetRandomTile(random);

		return _tilesWithReducedPossibleProtoTiles.First();
	}

	public void CollapseTile(Tile tile, ProtoTile protoTile)
	{
		tile.ProtoTile = protoTile;
		tile.PossibleProtoTiles.Clear();

		if (_tilesWithReducedPossibleProtoTiles.Remove(tile))
			SortTilesWithReducedPossibleProtoTiles();
	}

	public void UpdatePossibleProtoTiles(Tile tile, IList<ProtoTile> validNeighborProtoTiles)
	{
		tile.PossibleProtoTiles = validNeighborProtoTiles;
		if (!_tilesWithReducedPossibleProtoTiles.Contains(tile))
			_tilesWithReducedPossibleProtoTiles.Add(tile);

		SortTilesWithReducedPossibleProtoTiles();
	}

	public static bool TileIsDecided(Tile tile) => tile.ProtoTile != null && tile.PossibleProtoTiles.Count == 0;

	protected Tile GetRandomTile(Random random)
	{
		return _tiles[random.Next(_tiles.Length)];
	}

	internal int Index(int x, int y, int z) => z * Width * Height + y * Width + x;

	private void SortTilesWithReducedPossibleProtoTiles() => _tilesWithReducedPossibleProtoTiles = _tilesWithReducedPossibleProtoTiles.OrderBy(x => x.PossibleProtoTiles.Count).ToList();

	private Configuration _configuration;
	private readonly Func<int, int, int, IList<ProtoTile>> _getInitialValidProtoTilesForPosition;
	private Tile[] _tiles;
	private List<Tile> _tilesWithReducedPossibleProtoTiles = new List<Tile>();

}
