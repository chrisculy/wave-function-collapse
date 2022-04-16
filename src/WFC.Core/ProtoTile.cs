namespace WFC.Core;

public class ProtoTile
{
	public string Id { get; set; }
	public int Weight { get; set; } = 1;
	public List<List<int>> NeighborIndices { get; set; }
}
