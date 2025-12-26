# WFC.Core

A .NET implementation of the Wave Function Collapse (WFC) algorithm for procedural generation.

## Overview

WFC.Core is a library that implements the Wave Function Collapse algorithm, a constraint-solving technique inspired by quantum mechanics. The algorithm generates output grids by iteratively collapsing possibilities based on adjacency constraints, making it ideal for procedural content generation such as tile-based maps, textures, and level layouts.

The library supports:

- 2D grid generation with customizable adjacency rules
- Weighted tile selection for controlling output distribution
- Configurable tile constraints and neighbor relationships
- Extensible architecture for different adjacency algorithms
- Serialization support for configurations and outputs

## Getting Started

### Installation

Add the WFC.Core project reference to your .csproj file:

```xml
<ItemGroup>
  <ProjectReference Include="path\to\WFC.Core\WFC.Core.csproj" />
</ItemGroup>
```

### Basic Usage

Here's a simple example that generates a 10x10 grid using WFC:

```csharp
using WFC.Core;

// Step 1: Define your prototype tiles
var protoTiles = new List<ProtoTile>
{
    new ProtoTile
    {
        Id = "grass",
        Weight = 10,
        NeighborIndices = new List<List<int>>
        {
            new() { 0, 1 }, // Up: can be grass or water
            new() { 0, 1 }, // Right: can be grass or water
            new() { 0, 1 }, // Down: can be grass or water
            new() { 0, 1 }  // Left: can be grass or water
        }
    },
    new ProtoTile
    {
        Id = "water",
        Weight = 5,
        NeighborIndices = new List<List<int>>
        {
            new() { 1 }, // Up: must be water
            new() { 1 }, // Right: must be water
            new() { 1 }, // Down: must be water
            new() { 1 }  // Left: must be water
        }
    }
};

// Step 2: Create configuration with 2D adjacency
var configuration = new Configuration(protoTiles, AdjacencyAlgorithmKind.ADJACENCY_2D);

// Step 3: Create output grid (10x10)
var output = new Output(configuration, width: 10, height: 10);

// Step 4: Run the algorithm
var algorithm = new Algorithm(configuration, seed: 12345);
algorithm.Run(output);

// Step 5: Access the generated tiles
// The output.ToSerializable() method can be used for saving/loading results
```

### Understanding Tile Adjacency

The adjacency system determines which tiles can be placed next to each other. For 2D grids, each tile has four adjacency directions:

- **Index 0**: Up (North)
- **Index 1**: Right (East)
- **Index 2**: Down (South)
- **Index 3**: Left (West)

Each ProtoTile's `NeighborIndices` property is a list of four lists, where each inner list contains the indices of ProtoTiles that can appear in that direction.

Example:

```csharp
// A grass tile that can have grass (index 0) or sand (index 1) above it
NeighborIndices = new List<List<int>>
{
    new() { 0, 1 }, // Up: grass or sand allowed
    new() { 0 },    // Right: only grass
    new() { 0, 1 }, // Down: grass or sand allowed
    new() { 0 }     // Left: only grass
}
```

## API Reference

### Core Classes

#### `Algorithm`

The main algorithm executor that performs the wave function collapse process.

**Constructor:**

```csharp
public Algorithm(Configuration configuration, int? seed = null)
```

- `configuration`: The configuration containing prototype tiles and adjacency rules
- `seed`: Optional random seed for deterministic generation

**Methods:**

```csharp
public void Run(Output output)
```

Executes the WFC algorithm on the provided output grid. Iteratively collapses tiles with the least entropy until the grid is complete.

---

#### `Configuration`

Manages prototype tiles and adjacency calculation algorithms.

**Constructor:**

```csharp
public Configuration(IList<ProtoTile> protoTiles, AdjacencyAlgorithmKind adjacencyAlgorithmKind)
```

- `protoTiles`: Collection of prototype tiles defining the tileset
- `adjacencyAlgorithmKind`: The type of adjacency algorithm (currently supports `ADJACENCY_2D`)

**Properties:**

```csharp
public IList<ProtoTile> ProtoTiles { get; }
```

The collection of all prototype tiles available for generation.

**Methods:**

```csharp
public ProtoTile PickProtoTile(IList<ProtoTile> possibleProtoTiles, Random random)
```

Selects a prototype tile from possibilities using weighted random selection.

---

#### `Output`

Represents the output grid being generated.

**Constructor:**

```csharp
public Output(Configuration configuration, int width, int height, int depth = 1)
```

- `configuration`: The configuration to use for this output
- `width`: Width of the output grid
- `height`: Height of the output grid
- `depth`: Depth of the output grid (defaults to 1 for 2D grids)

**Properties:**

```csharp
public int Width { get; }
public int Height { get; }
public int Depth { get; }
```

**Methods:**

```csharp
public SerializableOutput ToSerializable()
```

Converts the output to a serializable format for saving/loading.

```csharp
public bool IsComplete()
```

Returns true if all tiles in the grid have been collapsed.

```csharp
public Tile GetTileWithLeastEntropy(Random random)
```

Retrieves the tile with the fewest possible states (least entropy) for the next collapse operation.

---

#### `ProtoTile`

Defines a prototype tile with its constraints and properties.

**Properties:**

```csharp
public string Id { get; set; }
```

Unique identifier for the tile type.

```csharp
public int Weight { get; set; }
```

Weight used for weighted random selection (higher = more likely to appear). Defaults to 1.

```csharp
public List<List<int>> NeighborIndices { get; set; }
```

Adjacency constraints. For 2D grids, contains four lists (up, right, down, left), each listing valid neighbor ProtoTile indices.

---

#### `Tile`

Represents a single tile position in the output grid.

**Properties:**

```csharp
public int X { get; }
public int Y { get; }
public int Z { get; }
```

Position coordinates in the output grid.

```csharp
public ProtoTile ProtoTile { get; set; }
```

The collapsed ProtoTile at this position (null if not yet collapsed).

```csharp
public IList<ProtoTile> PossibleProtoTiles { get; set; }
```

List of ProtoTiles that could still be placed at this position (empty when collapsed).

---

### Enumerations

#### `AdjacencyAlgorithmKind`

Specifies the type of adjacency calculation to use.

```csharp
public enum AdjacencyAlgorithmKind
{
    ADJACENCY_2D,   // 4-directional adjacency (up, right, down, left)
    ADJACENCY_3D,   // 6-directional adjacency (not yet implemented)
    ADJACENCY_HEX   // Hexagonal adjacency (not yet implemented)
}
```

---

### Serialization

#### `SerializableConfiguration`

Record type for serializing/deserializing configurations.

```csharp
public record SerializableConfiguration(
    AdjacencyAlgorithmKind AdjacencyAlgorithmKind,
    IList<ProtoTile> ProtoTiles
);
```

#### `SerializableOutput`

Class for serializing/deserializing output grids.

**Properties:**

```csharp
public int Width { get; set; }
public int Height { get; set; }
public int Depth { get; set; }
public List<string> ProtoTiles { get; set; }
public List<int> Tiles { get; set; }
```

The `Tiles` list stores indices into the `ProtoTiles` list, representing which ProtoTile is at each position.

---

## Advanced Examples

### Creating Complex Tile Sets

```csharp
// Define a more complex tileset with grass, sand, and water
var protoTiles = new List<ProtoTile>
{
    new ProtoTile
    {
        Id = "grass",
        Weight = 20,
        NeighborIndices = new List<List<int>>
        {
            new() { 0, 1 },    // Up: grass or sand
            new() { 0, 1 },    // Right: grass or sand
            new() { 0, 1 },    // Down: grass or sand
            new() { 0, 1 }     // Left: grass or sand
        }
    },
    new ProtoTile
    {
        Id = "sand",
        Weight = 10,
        NeighborIndices = new List<List<int>>
        {
            new() { 0, 1, 2 }, // Up: grass, sand, or water
            new() { 0, 1, 2 }, // Right: grass, sand, or water
            new() { 0, 1, 2 }, // Down: grass, sand, or water
            new() { 0, 1, 2 }  // Left: grass, sand, or water
        }
    },
    new ProtoTile
    {
        Id = "water",
        Weight = 15,
        NeighborIndices = new List<List<int>>
        {
            new() { 1, 2 },    // Up: sand or water
            new() { 1, 2 },    // Right: sand or water
            new() { 1, 2 },    // Down: sand or water
            new() { 1, 2 }     // Left: sand or water
        }
    }
};
```

### Deterministic Generation

Use the same seed to generate identical outputs:

```csharp
var configuration = new Configuration(protoTiles, AdjacencyAlgorithmKind.ADJACENCY_2D);
var output = new Output(configuration, 20, 20);

// Using a specific seed ensures reproducible results
var algorithm = new Algorithm(configuration, seed: 42);
algorithm.Run(output);
```

### Serializing Results

```csharp
// After generation
var serializableOutput = output.ToSerializable();

// Use System.Text.Json or Newtonsoft.Json to save
var json = JsonSerializer.Serialize(serializableOutput);
File.WriteAllText("output.json", json);
```

## Algorithm Details

The WFC algorithm follows these steps:

1. **Initialize**: All tiles start in a superposition of all possible ProtoTiles
2. **Observe**: Select the tile with the least entropy (fewest possibilities)
3. **Collapse**: Choose one ProtoTile for the selected tile using weighted random selection
4. **Propagate**: Update neighboring tiles to remove impossible ProtoTiles based on adjacency constraints
5. **Repeat**: Continue until all tiles are collapsed or a contradiction is found

If the algorithm encounters a contradiction (no valid tiles for a position), it throws an `InvalidOperationException` with the message "Failed to find solution". This typically indicates that the constraint rules are too restrictive for the given grid size.

## Requirements

- .NET 8.0 or higher
- No external dependencies

## License

[License information to be added]

## Contributing

[Contribution guidelines to be added]
