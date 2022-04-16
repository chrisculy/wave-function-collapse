namespace WFC.Core;

public class Algorithm
{
	public Algorithm(Configuration configuration, int? seed = null)
	{
		_configuration = configuration;
		_random = seed != null ? new Random(seed.Value) : new Random(); 
	}
	
	public void Run(Output output)
	{
		while (!output.IsComplete())
		{
			var tile = output.GetTileWithLeastEntropy(_random);
			output.CollapseTile(tile, _configuration.PickProtoTile(tile.PossibleProtoTiles, _random));

			Propagate(output, tile);
		}
	}

	private void Propagate(Output output, Tile tile)
	{
		var tilesToProcess = new Queue<Tile>();
		tilesToProcess.Enqueue(tile);
		while (tilesToProcess.Any())
		{
			var currentTile = tilesToProcess.Dequeue();
			foreach (var neighbor in output.GetUndecidedNeighbors(currentTile))
			{
				if (neighbor.PossibleProtoTiles.Count == 0)
				{
					Console.WriteLine("Found a neighbor tile during propagation that has no possible prototiles remaining.");
					continue;
				}

				var validNeighborProtoTiles = output.GetValidNeighborProtoTiles(currentTile, neighbor);
				if (validNeighborProtoTiles.Count == 0)
					throw new InvalidOperationException("Failed to find solution");

				var eliminatedPossibleProtoTiles = neighbor.PossibleProtoTiles.Except(validNeighborProtoTiles).ToList();
				if (eliminatedPossibleProtoTiles.Count != 0)
				{
					output.UpdatePossibleProtoTiles(neighbor, validNeighborProtoTiles);
					tilesToProcess.Enqueue(neighbor);
				}
			}
		}
	}

	private Configuration _configuration;
	private Random _random;
}