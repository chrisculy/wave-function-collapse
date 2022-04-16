using System.Text.Json;
using WFC.Core;
using WFC.Core.Serialization;

class Program
{
	/// <param name="configurationFile">Path to the JSON configuration file</param>
	/// <param name="outputFile">Path to the JSON file to write the output to</param>
	/// <param name="width">Width of the output (X dimension)</param>
	/// <param name="height">Height of the output (Y dimension)</param>
	/// <param name="depth">Depth of the output (Z dimension)</param>
	public static async Task Main(string configurationFile, string outputFile, int width, int height, int depth = 1)
	{
		SerializableConfiguration serializedConfiguration;
		using (var configurationFileStream = File.OpenRead(configurationFile))
		{
			serializedConfiguration = await JsonSerializer.DeserializeAsync<SerializableConfiguration>(configurationFileStream);
		}

		var configuration = new Configuration(serializedConfiguration.ProtoTiles, serializedConfiguration.AdjacencyAlgorithmKind);
		var output = new Output(configuration, width, height, depth);
		var wfc = new Algorithm(configuration);
		wfc.Run(output);

		using (var outputFileStream = File.Create(outputFile))
		{
			await JsonSerializer.SerializeAsync(outputFileStream, output.ToSerializable(), new JsonSerializerOptions { WriteIndented = true });
		}
	}

	private static IList<ProtoTile> LoadProtoTiles()
	{
		throw new NotImplementedException();
	}
}
