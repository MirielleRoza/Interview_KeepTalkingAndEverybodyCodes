using System.Reflection;

namespace DataAccess;

public class CameraRepository : ICameraRepository
{
    /// <summary>
    /// Returns all entries from the datafile, filtered by searchTerm if present.
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public async Task<List<CameraDataEntry>> GetEntriesByContainingText(string? searchTerm)
    {
        var dataSet = await ReadDataFile(); 

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            dataSet = dataSet.Where(x => x.CameraName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        return dataSet;
    }

    /// <summary>
    /// Returns all entries, without any filtering or pagination.
    /// </summary>
    /// <returns></returns>
    public async Task<List<CameraDataEntry>> GetAllEntries()
    {
        return await ReadDataFile();
    }

    /// <summary>
    /// Reads the datafile and converts it to a List of FileDataLineModels
    /// </summary>
    /// <returns></returns>
    private async Task<List<CameraDataEntry>> ReadDataFile()
    {
        var data = (await File.ReadAllTextAsync(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "/DataFiles/cameras-defb.csv")).Split(Environment.NewLine).ToList<string>();
        List<CameraDataEntry> output = new();

        //it's a CSV. we know the first line is the definition, so we can skip it.
        data.RemoveAt(0);
        foreach (var line in data.Where(x => x.Length > 0))
        {
            //split on commas
            var lineSplit = line.Split(";");
            if (lineSplit.Length != 3)
            {
                //This is a garbage line. do not leak errors in our dataset.
                //TODO: have a notification of sorts for someone to go check out the data and clean this up or find the cause.
                continue;
            }

            output.Add(new CameraDataEntry()
            {
                CameraName = lineSplit[0],
                Latitude = lineSplit[1],
                Longitutude = lineSplit[2]
            });
        }

        return output;
    }
}