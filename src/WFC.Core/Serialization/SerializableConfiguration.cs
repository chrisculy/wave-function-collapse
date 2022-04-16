namespace WFC.Core.Serialization;

public record SerializableConfiguration(AdjacencyAlgorithmKind AdjacencyAlgorithmKind, IList<ProtoTile> ProtoTiles);
