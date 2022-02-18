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
    public IActionResult Quality()
    {
        var lstModel = new List<StackedWebModel>();
        lstModel.Add(new StackedWebModel
        {
            StackedDimensionOne = "Первый оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Брак",
                    Quantity = rnd.Next(10)
                }
            }
        });
        lstModel.Add(new StackedWebModel
        {
            StackedDimensionOne = "Второй оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Брак",
                    Quantity = rnd.Next(10)
                }
            }
        });
        lstModel.Add(new StackedWebModel
        {
            StackedDimensionOne = "Третий оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportWebModel()
                {
                    DimensionOne="Брак",
                    Quantity = rnd.Next(10)
                }
            }
        });
        return View(lstModel);
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

