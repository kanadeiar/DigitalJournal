namespace DigitalJournal.Controllers;

public class HomeController : Controller
{
    private Random rnd = new Random();
    public IActionResult Index()
    {
        var lstModel = new List<SimpleReportViewModel>();
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Склад сырья",
            Quantity = rnd.Next(100),
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Прессование продукта",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Автоклавирование",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Упаковка",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Склад готовой продукции",
            Quantity = rnd.Next(100)
        });
        return View(lstModel);
    }
    public IActionResult Products()
    {
        var lstModel = new List<SimpleReportViewModel>();
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Кирпич полуторный 250x120x65",
            Quantity = rnd.Next(100),
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Кирпич двойной 250x120x138",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Кирпич евро пустотелый 250x120x120",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Кирпич полнотелый 250x120x88",
            Quantity = rnd.Next(100)
        });
        return View(lstModel);
    }
    public IActionResult Speed()
    {
        var lstModel = new List<SimpleReportViewModel>();
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Понедельник",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Вторник",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Среда",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Четверг",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Пятница",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Суббота",
            Quantity = rnd.Next(100)
        });
        lstModel.Add(new SimpleReportViewModel
        {
            DimensionOne = "Воскресенье",
            Quantity = rnd.Next(100)
        });
        return View(lstModel);
    }
    public IActionResult Quality()
    {
        var lstModel = new List<StackedViewModel>();
        lstModel.Add(new StackedViewModel
        {
            StackedDimensionOne = "Первый оператор",
            LstData = new List<SimpleReportViewModel>()
            {
                new SimpleReportViewModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportViewModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportViewModel()
                {
                    DimensionOne="Брак",
                    Quantity = rnd.Next(10)
                }
            }
        });
        lstModel.Add(new StackedViewModel
        {
            StackedDimensionOne = "Второй оператор",
            LstData = new List<SimpleReportViewModel>()
            {
                new SimpleReportViewModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportViewModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportViewModel()
                {
                    DimensionOne="Брак",
                    Quantity = rnd.Next(10)
                }
            }
        });
        lstModel.Add(new StackedViewModel
        {
            StackedDimensionOne = "Третий оператор",
            LstData = new List<SimpleReportViewModel>()
            {
                new SimpleReportViewModel()
                {
                    DimensionOne="Норма",
                    Quantity = rnd.Next(100)
                },
                new SimpleReportViewModel()
                {
                    DimensionOne="Плохо",
                    Quantity = rnd.Next(30)
                },
                new SimpleReportViewModel()
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

    public class SimpleReportViewModel
    {
        public string DimensionOne { get; set; }
        public int Quantity { get; set; }
    }
    public class StackedViewModel
    {
        public string StackedDimensionOne { get; set; }
        public List<SimpleReportViewModel> LstData { get; set; }
    }
}

