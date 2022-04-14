namespace WFC.Core;

public class Dimensions
{
	public static Dimensions Create2d(int width, int height) => new Dimensions(new [] { width, height });
	public static Dimensions Create3d(int width, int height, int depth) => new Dimensions(new [] { width, height, depth });

	public int Width { get => _dimensions.Count >= 1 ? _dimensions.ElementAtOrDefault(0) : -1; }
	public int Height { get => _dimensions.Count >= 2 ? _dimensions.ElementAtOrDefault(1) : -1; }
	public int Depth { get => _dimensions.Count >= 3 ? _dimensions.ElementAtOrDefault(2) : -1; }

	public Dimensions(IReadOnlyList<int> dimensions)
	{
		_dimensions = dimensions;
	}

	public int GetDimension(int index) => index < _dimensions.Count ? _dimensions[index] : -1;

	private IReadOnlyList<int> _dimensions;
}