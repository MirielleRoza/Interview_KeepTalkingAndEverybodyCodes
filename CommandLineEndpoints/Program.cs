global using CommandLine;
global using CommandLineEndpoints;
global using DataAccess;

internal class Program
{
    async static Task<int> Main(string[] args)
    {
        return await Parser.Default.ParseArguments<CommandLineModel>(args).MapResult(async (CommandLineModel options) =>
        {
            Console.WriteLine("Starting search through CSV with argument: ");
            var fdRepo = new CameraRepository();

            var result = await fdRepo.GetEntriesByContainingText(options.Search);

            //print it.
            foreach (var dataPoint in result)
            {
                //assumptions made for the sake of time:
                //number will always be after the second dash and be three digits long. anything after that is inconsistent, but also irrelevant for our purposes.
                var cameraNumber = dataPoint.CameraName.Split('-')[2].Substring(0,3);

                Console.WriteLine($"{cameraNumber} | {dataPoint.CameraName} | {dataPoint.Latitude} | {dataPoint.Longitutude}");
            }

            return 1;
        },
        errors => Task.FromResult(-1));

    }
}
