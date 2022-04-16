namespace WFC.Core.Serialization;

public class SerializableOutput
{
	public int Width { get; set; }
	public int Height { get; set; }
	public int Depth { get; set; }
	public List<string> ProtoTiles { get; set; }

	// Each tile simply stores the index of the prototile at that location
	public List<int> Tiles { get; set; }
}