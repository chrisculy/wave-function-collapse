namespace WFC.Core;

public interface ITile
{
	IProtoTile ProtoTile { get; set; }
	IList<IProtoTile> PossibleProtoTiles { get; set; }
}