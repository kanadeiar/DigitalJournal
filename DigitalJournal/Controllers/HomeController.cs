namespace DigitalJournal.Controllers;

public class HomeController : Controller
{
    private Random rnd = new Random();
    private readonly IHomeInfoService _homeInfoService;
    public HomeController(IHomeInfoService homeInfoService)
    {
        _homeInfoService = homeInfoService;
    }
    public async Task<IActionResult> Index()
    {
        var models = await _homeInfoService.GetMoneyInfoForFactory1();
        return View(models);
    }
    public async Task<IActionResult> Products()
    {
        var models = await _homeInfoService.GetDataForPress1ForProducts();
        return View(models);
    }
    public async Task<IActionResult> Speed()
    {
        var models = await _homeInfoService.GetSpeedPackOfFacttory1();
        return View(models);
    }
    public async Task<IActionResult> Quality()
    {
        var models = await _homeInfoService.GetQualityDataOfFactory1();
        return View(models);
    }
    public IActionResult About()
    {
        return View();
    }

    public IActionResult Error(string id)
    {
        return id switch
        {
            "404" => View("Error404"),
            _ => Content($"Status --- {id}"),
        };
    }
}

