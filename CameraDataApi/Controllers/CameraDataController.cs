namespace CameraDataApi.Controllers;

[ApiController]
[Route("v1/cameraData")]
public class CameraDataController : ControllerBase
{

    private readonly ILogger<CameraDataController> _logger;
    private readonly ICameraRepository cameraRepo;

    public CameraDataController(ILogger<CameraDataController> logger, ICameraRepository cameraRepo)
    {
        _logger = logger;
        this.cameraRepo = cameraRepo;
    }

    [HttpGet()]
    public async Task<List<CameraDataEntry>> Get()
    {
        _logger.LogInformation("Get called.");
        return await cameraRepo.GetAllEntries();
    }

    [HttpGet()]
    [Route("search")]
    public async Task<List<CameraDataEntry>> GetFiltered([FromQuery] string searchTerm)
    {
        _logger.LogInformation("GetFiltered called.");
        return await cameraRepo.GetEntriesByContainingText(searchTerm);
    }
}