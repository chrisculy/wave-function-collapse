namespace WFC.Core;

public class Configuration
{
	public IOutput CreateBlankOutput(Dimensions dimensions)
	{
		throw new NotImplementedException();
	}

	public IProtoTile PickProtoTile(IList<IProtoTile> possibleProtoTiles, Random random)
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

		return possibleProtoTiles.Last();
	}
}
