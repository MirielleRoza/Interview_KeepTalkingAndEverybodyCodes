namespace DataAccess;

public class FileDataRepository
{
    /// <summary>
    /// Returns all entries from the datafile, filtered by searchTerm if present.
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public async Task<List<FileDataLineModel>> GetEntriesByContainingText(string? searchTerm)
    {
        var dataSet = await ReadDataFile();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            dataSet = dataSet.Where(x => x.CameraName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        return dataSet;
    }

    /// <summary>
    /// Reads the datafile and converts it to a List of FileDataLineModels
    /// </summary>
    /// <returns></returns>
    private async Task<List<FileDataLineModel>> ReadDataFile()
    {
        var data = (await File.ReadAllTextAsync("DataFiles/cameras-defb.csv")).Split(System.Environment.NewLine).ToList<string>();
        List<FileDataLineModel> output = new();

        //it's a CSV. we know the first line is the definition, so we can skip it.
        data.RemoveAt(0);
        foreach(var line in data.Where(x => x.Length > 0))
        {
            //split on commas
            var lineSplit = line.Split(";");
            if(lineSplit.Length != 3)
            { 
                //This is a garbage line. do not leak errors in our dataset.
                //TODO: have a notification of sorts for someone to go check out the data and clean this up or find the cause.
                continue;
            }

            output.Add(new FileDataLineModel(){ 
                CameraName = lineSplit[0],
                Latitude = lineSplit[1],
                Longitutude = lineSplit[2]
            });
        }

        return output;
    }
}