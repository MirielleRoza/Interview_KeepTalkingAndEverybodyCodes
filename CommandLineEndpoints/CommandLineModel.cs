namespace CommandLineEndpoints;

public class CommandLineModel
{
    [Option(shortName: 's', longName: "search", Required = false)]
    public string? Search { get; set; }
}
