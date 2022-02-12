namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Warehouse2ShiftDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Factory1Warehouse2ShiftData>? Query => _Context.Factory1Warehouse2ShiftData;
    public IEnumerable<Factory1Warehouse2ShiftData>? Datas { get; set; }

    public EWarehouse2Order DataOrder { get; set; } = EWarehouse2Order.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EWarehouse2Order order)
    {
        DataOrder = order switch
        {
            EWarehouse2Order.TimeAsc when DataOrder == EWarehouse2Order.TimeAsc => EWarehouse2Order.TimeDesc,
            EWarehouse2Order.ShiftAsc when DataOrder == EWarehouse2Order.ShiftAsc => EWarehouse2Order.ShiftDesc,
            EWarehouse2Order.UserAsc when DataOrder == EWarehouse2Order.UserAsc => EWarehouse2Order.UserDesc,
            EWarehouse2Order.Place1Asc when DataOrder == EWarehouse2Order.Place1Asc => EWarehouse2Order.Place1Desc,
            EWarehouse2Order.Place2Asc when DataOrder == EWarehouse2Order.Place2Asc => EWarehouse2Order.Place2Desc,
            EWarehouse2Order.Place3Asc when DataOrder == EWarehouse2Order.Place3Asc => EWarehouse2Order.Place3Desc,
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
            EWarehouse2Order.TimeAsc => Query.OrderBy(x => x.Time),
            EWarehouse2Order.TimeDesc => Query.OrderByDescending(x => x.Time),
            EWarehouse2Order.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EWarehouse2Order.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EWarehouse2Order.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EWarehouse2Order.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EWarehouse2Order.Place1Asc => Query.OrderBy(x => x.Place1ProductsCount),
            EWarehouse2Order.Place1Desc => Query.OrderByDescending(x => x.Place1ProductsCount),
            EWarehouse2Order.Place2Asc => Query.OrderBy(x => x.Place2ProductsCount),
            EWarehouse2Order.Place2Desc => Query.OrderByDescending(x => x.Place2ProductsCount),
            EWarehouse2Order.Place3Asc => Query.OrderBy(x => x.Place3ProductsCount),
            EWarehouse2Order.Place3Desc => Query.OrderByDescending(x => x.Place3ProductsCount),
            _ => Query.OrderBy(x => x.Time),
        };
        Datas = await query
            .Include(x => x.Factory1Shift)
            .Include(x => x.Profile)
            .Include(x => x.Place1ProductType)
            .Include(x => x.Place2ProductType)
            .Include(x => x.Place3ProductType)
            .Skip((Page - 1) * 10)
            .Take(10)
            .ToArrayAsync();
    }
}

public enum EWarehouse2Order
{
    TimeAsc,
    TimeDesc,
    ShiftAsc,
    ShiftDesc,
    UserAsc,
    UserDesc,
    Place1Asc,
    Place1Desc,
    Place2Asc,
    Place2Desc,
    Place3Asc,
    Place3Desc,
}