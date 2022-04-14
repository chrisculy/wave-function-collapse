namespace WFC.Core;

public class Algorithm
{
	public Algorithm(Configuration configuration, int? seed = null)
	{
		_configuration = configuration;
		_random = seed != null ? new Random(seed.Value) : new Random(); 
	}
	
	public IOutput Run(Dimensions dimensions)
	{
		IOutput output = _configuration.CreateBlankOutput(dimensions);
		while (!output.IsComplete())
		{
			var tile = output.GetTileWithLeastEntropy(_random);
			output.CollapseTile(tile, _configuration.PickProtoTile(tile.PossibleProtoTiles, _random));

			Propagate(output, tile);
		}

		return output;
	}

	private void Propagate(IOutput output, ITile tile)
	{
		var tilesToProcess = new Queue<ITile>();
		tilesToProcess.Enqueue(tile);
		while (tilesToProcess.Any())
		{
			var currentTile = tilesToProcess.Dequeue();
			foreach (var neighbor in output.GetUndecidedNeighbors(currentTile))
			{
				
			}
		}
	}

	private Configuration _configuration;
	private Random _random;
}