namespace WFC.Core;

public class Tile
{
	public int X { get; private set; }
	public int Y { get; private set; }
	public int Z { get; private set; }

	public Tile(int x = 0, int y = 0, int z = 0)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public ProtoTile ProtoTile { get; set; }
	public IList<ProtoTile> PossibleProtoTiles { get; set; }
}