namespace DigitalJournal.Services;

public class HomeInfoService : IHomeInfoService
{
    private readonly DigitalJournalContext _journalContext;
    public HomeInfoService(DigitalJournalContext journalContext)
    {
        _journalContext = journalContext;
    }

    public async Task<IList<SimpleReportWebModel>> GetMoneyInfoForFactory1()
    {
        var list = new List<SimpleReportWebModel>();
        var warehouse1sum = await _journalContext.Factory1Warehouse1ShiftData.SumAsync(x => x.Tank1LooseRawValue);
        list.Add(new SimpleReportWebModel
        {
            DimensionOne = "Хранилище № 1",
            Quantity = Convert.ToInt32(warehouse1sum),
        });
        var press1sum = await _journalContext.Factory1Press1ShiftData.SumAsync(x => x.ProductCount);
        list.Add(new SimpleReportWebModel
        {
            DimensionOne = "Хранилище № 2",
            Quantity = press1sum,
        });
        var autoclave1sum = await _journalContext.Factory1Autoclave1ShiftDatas.SumAsync(x => x.AutoclavedCount);
        list.Add(new SimpleReportWebModel
        {
            DimensionOne = "Хранилище № 3",
            Quantity = autoclave1sum,
        });
        var pack1sum = await _journalContext.Factory1Pack1ShiftDatas.SumAsync(x => x.ProductCount);
        list.Add(new SimpleReportWebModel
        {
            DimensionOne = "Хранилище № 4",
            Quantity = pack1sum,
        });
        var warehouse2sum = await _journalContext.Factory1Warehouse2ShiftData.SumAsync(x => x.Place1ProductsCount);
        list.Add(new SimpleReportWebModel
        {
            DimensionOne = "Хранилище № 5",
            Quantity = warehouse2sum,
        });
        return list;
    }

    public async Task<IList<SimpleReportWebModel>> GetDataForPress1ForProducts()
    {
        var products = await _journalContext.Factory1ProductTypes.ToArrayAsync();
        var list = products.GroupJoin(_journalContext.Factory1Pack1ShiftDatas,
            p => p.Id,
            d => d.Factory1ProductTypeId, 
            (p, d) => new SimpleReportWebModel 
            { 
                DimensionOne = p.Name,
                Quantity = d.Sum(x => x.ProductCount),
            }).ToList();
        return list;
    }

    public async Task<IList<SimpleReportWebModel>> GetSpeedPackOfFacttory1()
    {
        var dates = await _journalContext.Factory1Pack1ShiftDatas
            .OrderByDescending(x => x.Time)
            .Take(10).Select(x => new SimpleReportWebModel
            {
                DimensionOne = x.Time.ToString("dd.MM.yyyy HH:mm"),
                Quantity = x.ProductCount,
            }).ToListAsync();
        return dates;
    }

    public async Task<IList<StackedWebModel>> GetQualityDataOfFactory1()
    {
        var operators = new List<StackedWebModel>();
        operators.Add(new StackedWebModel
        {
            StackedDimensionOne = "Первый оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne = "Норма",
                    Quantity = 70,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Плохо",
                    Quantity = 20,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Брак",
                    Quantity = 10,
                },
            }
        });
        operators.Add(new StackedWebModel
        {
            StackedDimensionOne = "Второй оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne = "Норма",
                    Quantity = 60,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Плохо",
                    Quantity = 25,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Брак",
                    Quantity = 15,
                },
            }
        });
        operators.Add(new StackedWebModel
        {
            StackedDimensionOne = "Третий оператор",
            LstData = new List<SimpleReportWebModel>()
            {
                new SimpleReportWebModel()
                {
                    DimensionOne = "Норма",
                    Quantity = 80,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Плохо",
                    Quantity = 15,
                },
                new SimpleReportWebModel()
                {
                    DimensionOne = "Брак",
                    Quantity = 5,
                },
            }
        });
        return operators;
    }
}


