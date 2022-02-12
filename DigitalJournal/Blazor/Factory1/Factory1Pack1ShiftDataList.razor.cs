namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Pack1ShiftDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Factory1Pack1ShiftData>? Query => _Context.Factory1Pack1ShiftDatas;
    public IEnumerable<Factory1Pack1ShiftData>? Datas { get; set; }

    public EPack1Order DataOrder { get; set; } = EPack1Order.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EPack1Order order)
    {
        DataOrder = order switch
        {
            EPack1Order.TimeAsc when DataOrder == EPack1Order.TimeAsc => EPack1Order.TimeDesc,
            EPack1Order.ShiftAsc when DataOrder == EPack1Order.ShiftAsc => EPack1Order.ShiftDesc,
            EPack1Order.UserAsc when DataOrder == EPack1Order.UserAsc => EPack1Order.UserDesc,
            EPack1Order.ProductNameAsc when DataOrder == EPack1Order.ProductNameAsc => EPack1Order.ProductNameDesc,
            EPack1Order.ProductCountAsc when DataOrder == EPack1Order.ProductCountAsc => EPack1Order.ProductCountDesc,
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
            EPack1Order.TimeAsc => Query.OrderBy(x => x.Time),
            EPack1Order.TimeDesc => Query.OrderByDescending(x => x.Time),
            EPack1Order.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EPack1Order.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EPack1Order.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EPack1Order.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EPack1Order.ProductNameAsc => Query.Include(x => x.Factory1ProductType).OrderBy(x => x.Factory1ProductType.Name),
            EPack1Order.ProductNameDesc => Query.Include(x => x.Factory1ProductType).OrderByDescending(x => x.Factory1ProductType.Name),
            EPack1Order.ProductCountAsc => Query.OrderBy(x => x.ProductCount),
            EPack1Order.ProductCountDesc => Query.OrderByDescending(x => x.ProductCount),
            _ => Query.OrderBy(x => x.Time),
        };
        Datas = await query
            .Include(x => x.Factory1Shift)
            .Include(x => x.Profile)
            .Include(x => x.Factory1ProductType)
            .Skip((Page - 1) * 10)
            .Take(10)
            .ToArrayAsync();
    }
}

public enum EPack1Order
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
}