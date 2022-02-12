namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Press1ShiftDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Factory1Press1ShiftData>? Query => _Context.Factory1Press1ShiftData;
    public IEnumerable<Factory1Press1ShiftData>? Datas { get; set; }

    public EPress1Order DataOrder { get; set; } = EPress1Order.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EPress1Order order)
    {
        DataOrder = order switch
        {
            EPress1Order.TimeAsc when DataOrder == EPress1Order.TimeAsc => EPress1Order.TimeDesc,
            EPress1Order.ShiftAsc when DataOrder == EPress1Order.ShiftAsc => EPress1Order.ShiftDesc,
            EPress1Order.UserAsc when DataOrder == EPress1Order.UserAsc => EPress1Order.UserDesc,
            EPress1Order.ProductNameAsc when DataOrder == EPress1Order.ProductNameAsc => EPress1Order.ProductNameDesc,
            EPress1Order.ProductCountAsc when DataOrder == EPress1Order.ProductCountAsc => EPress1Order.ProductCountDesc,
            EPress1Order.Loose1Asc when DataOrder == EPress1Order.Loose1Asc => EPress1Order.Loose1Desc,
            EPress1Order.Loose2Asc when DataOrder == EPress1Order.Loose2Asc => EPress1Order.Loose2Desc,
            EPress1Order.Loose3Asc when DataOrder == EPress1Order.Loose3Asc => EPress1Order.Loose3Desc,
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
            EPress1Order.TimeAsc => Query.OrderBy(x => x.Time),
            EPress1Order.TimeDesc => Query.OrderByDescending(x => x.Time),
            EPress1Order.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EPress1Order.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EPress1Order.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EPress1Order.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EPress1Order.ProductNameAsc => Query.Include(x => x.Factory1ProductType).OrderBy(x => x.Factory1ProductType.Name),
            EPress1Order.ProductNameDesc => Query.Include(x => x.Factory1ProductType).OrderByDescending(x => x.Factory1ProductType.Name),
            EPress1Order.ProductCountAsc => Query.OrderBy(x => x.ProductCount),
            EPress1Order.ProductCountDesc => Query.OrderByDescending(x => x.ProductCount),
            EPress1Order.Loose1Asc => Query.OrderBy(x => x.Loose1RawValue),
            EPress1Order.Loose1Desc => Query.OrderByDescending(x => x.Loose1RawValue),
            EPress1Order.Loose2Asc => Query.OrderBy(x => x.Loose2RawValue),
            EPress1Order.Loose2Desc => Query.OrderByDescending(x => x.Loose2RawValue),
            EPress1Order.Loose3Asc => Query.OrderBy(x => x.Loose3RawValue),
            EPress1Order.Loose3Desc => Query.OrderByDescending(x => x.Loose3RawValue),
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

public enum EPress1Order
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
    Loose1Asc,
    Loose1Desc,
    Loose2Asc,
    Loose2Desc,
    Loose3Asc,
    Loose3Desc,
}