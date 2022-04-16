namespace WFC.Core;

public static class AdjacencyAlgorithms2d
{
	public static IList<int> GetAdjacentIndices(Output output, int x, int y, int z)
	{
		var indices = new List<int>();
		if (x > 0)
		{
			// left
			indices.Add(output.Index(x - 1, y, 0));
		}

		if (x < output.Width - 1)
		{
			// right
			indices.Add(output.Index(x + 1, y, 0));
		}

		if (y > 0)
		{
			// down
			indices.Add(output.Index(x, y - 1, 0));
		}

		if (y < output.Height - 1)
		{
			// up
			indices.Add(output.Index(x, y + 1, 0));
		}

		return indices;
	}

	public static int GetAdjacencyIndex(Tile tile, Tile neighbor)
	{
		// 2d adjacency indices
		// up: 0
		// right: 1
		// down: 2
		// left : 3

		if (tile.X == neighbor.X)
			return tile.Y < neighbor.Y ? 0 : 2;
		
		if (tile.Y == neighbor.Y)
			return tile.X < neighbor.X ? 1 : 3;

		throw new InvalidOperationException($"Tiles {tile} and {neighbor} are not adjacent as expected.");
	}
}