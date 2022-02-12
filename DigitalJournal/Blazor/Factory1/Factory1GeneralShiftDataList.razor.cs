namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1GeneralShiftDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Factory1GeneralShiftData>? Query => _Context.Factory1GeneralShiftData;
    public IEnumerable<Factory1GeneralShiftData>? Datas { get; set; }

    public EF1GeneralOrder DataOrder { get; set; } = EF1GeneralOrder.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EF1GeneralOrder order)
    {
        DataOrder = order switch
        {
            EF1GeneralOrder.TimeAsc when DataOrder == EF1GeneralOrder.TimeAsc => EF1GeneralOrder.TimeDesc,
            EF1GeneralOrder.ShiftAsc when DataOrder == EF1GeneralOrder.ShiftAsc => EF1GeneralOrder.ShiftDesc,
            EF1GeneralOrder.UserAsc when DataOrder == EF1GeneralOrder.UserAsc => EF1GeneralOrder.UserDesc,
            EF1GeneralOrder.ProductNameAsc when DataOrder == EF1GeneralOrder.ProductNameAsc => EF1GeneralOrder.ProductNameDesc,
            EF1GeneralOrder.ProductCountAsc when DataOrder == EF1GeneralOrder.ProductCountAsc => EF1GeneralOrder.ProductCountDesc,
            EF1GeneralOrder.LooseAsc when DataOrder == EF1GeneralOrder.LooseAsc => EF1GeneralOrder.LooseDesc,
            EF1GeneralOrder.AutoclaveNumberAsc when DataOrder == EF1GeneralOrder.AutoclaveNumberAsc => EF1GeneralOrder.AutoclaveNumberAsc,
            EF1GeneralOrder.PackProductNameAsc when DataOrder == EF1GeneralOrder.PackProductNameAsc => EF1GeneralOrder.PackProductNameDesc,
            EF1GeneralOrder.PackProductCountAsc when DataOrder == EF1GeneralOrder.PackProductCountAsc => EF1GeneralOrder.PackProductCountDesc,
            _ => order,
        };
        await SetPage(1);
    }
    public async Task SetPage(int page)
    {
        Page = page;
        await UpdateDataAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateDataAsync();
        PagesCount = Query.Count() / 10;
    }

    private async Task UpdateDataAsync()
    {
        var query = DataOrder switch
        {
            EF1GeneralOrder.TimeAsc => Query.OrderBy(x => x.Time),
            EF1GeneralOrder.TimeDesc => Query.OrderByDescending(x => x.Time),
            EF1GeneralOrder.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EF1GeneralOrder.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EF1GeneralOrder.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EF1GeneralOrder.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EF1GeneralOrder.ProductNameAsc => Query.Include(x => x.Factory1ProductType).OrderBy(x => x.Factory1ProductType.Name),
            EF1GeneralOrder.ProductNameDesc => Query.Include(x => x.Factory1ProductType).OrderByDescending(x => x.Factory1ProductType.Name),
            EF1GeneralOrder.ProductCountAsc => Query.OrderBy(x => x.ProductCount),
            EF1GeneralOrder.ProductCountDesc => Query.OrderByDescending(x => x.ProductCount),
            EF1GeneralOrder.LooseAsc => Query.OrderBy(x => x.Loose1RawValue),
            EF1GeneralOrder.LooseDesc => Query.OrderByDescending(x => x.Loose1RawValue),
            EF1GeneralOrder.AutoclaveNumberAsc => Query.OrderBy(x => x.AutoclaveNumber),
            EF1GeneralOrder.AutoclaveNumberDesc => Query.OrderByDescending(x => x.AutoclaveNumber),
            EF1GeneralOrder.PackProductNameAsc => Query.Include(x => x.Factory1PackProductType).OrderBy(x => x.Factory1PackProductType.Name),
            EF1GeneralOrder.PackProductNameDesc => Query.Include(x => x.Factory1PackProductType).OrderByDescending(x => x.Factory1PackProductType.Name),
            EF1GeneralOrder.PackProductCountAsc => Query.OrderBy(x => x.PackProductCount),
            EF1GeneralOrder.PackProductCountDesc => Query.OrderByDescending(x => x.PackProductCount),
            _ => Query.OrderBy(x => x.Time),
        };
        Datas = await query
            .Include(x => x.Factory1Shift)
            .Include(x => x.Profile)
            .Include(x => x.Factory1ProductType)
            .Include(x => x.Factory1PackProductType)
            .Skip((Page - 1) * 10)
            .Take(10)
            .ToArrayAsync();
    }
}

public enum EF1GeneralOrder
{
    TimeAsc,
    TimeDesc,
    ShiftAsc,
    ShiftDesc,
    UserAsc,
    UserDesc,
    ProductNameAsc,
    ProductNameDesc,
    ProductCountAsc,
    ProductCountDesc,
    LooseAsc,
    LooseDesc,
    AutoclaveNumberAsc,
    AutoclaveNumberDesc,
    PackProductNameAsc,
    PackProductNameDesc,
    PackProductCountAsc,
    PackProductCountDesc,
}