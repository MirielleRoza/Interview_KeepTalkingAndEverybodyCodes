using CameraViewer.Models;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CameraViewer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICameraRepository _cameraRepo;

    public List<CameraDataViewModel> divByThree = new();
    public List<CameraDataViewModel> divByFive = new();
    public List<CameraDataViewModel> divByBoth = new();
    public List<CameraDataViewModel> divByNone = new();

    public IndexModel(ILogger<IndexModel> logger, ICameraRepository cameraRepo)
    {
        _logger = logger;
        _cameraRepo = cameraRepo;
    }

    public async void OnGet()
    {
        var fullDataSet = await _cameraRepo.GetAllEntries();       

        foreach (var entry in fullDataSet)
        {
            var number = int.Parse(entry.CameraName.Split('-')[2].Substring(0, 3));
            var isDivableByThree = number % 3 == 0;
            var isDivableByFive = number % 5 == 0;
            var viewModel = new CameraDataViewModel()
            {
                CameraName = entry.CameraName,
                CameraNumber = number,
                Latitude = entry.Latitude,
                Longitutude = entry.Longitutude
            };

            if (isDivableByThree && isDivableByFive)
            {
                divByBoth.Add(viewModel);
            }
            else if (isDivableByThree)
            {
                divByThree.Add(viewModel);
            }
            else if (isDivableByFive)
            {
                divByFive.Add(viewModel);
            }
            else
            {
                divByNone.Add(viewModel);
            }
        }

    }
}