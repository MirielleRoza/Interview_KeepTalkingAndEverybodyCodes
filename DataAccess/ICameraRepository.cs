namespace DataAccess;

public interface ICameraRepository
{
    Task<List<CameraDataEntry>> GetEntriesByContainingText(string? searchTerm);

    Task<List<CameraDataEntry>> GetAllEntries();
}