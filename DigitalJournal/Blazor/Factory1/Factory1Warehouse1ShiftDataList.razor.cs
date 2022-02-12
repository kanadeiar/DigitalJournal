namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Warehouse1ShiftDataList
{
    DigitalJournalContext _Context => Service;
    public IQueryable<Factory1Warehouse1ShiftData>? Query => _Context.Factory1Warehouse1ShiftData;
    public IEnumerable<Factory1Warehouse1ShiftData>? Datas { get; set; }

    public EDataOrder DataOrder { get; set; } = EDataOrder.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EDataOrder order)
    {
        DataOrder = order switch
        {
            EDataOrder.TimeAsc when DataOrder == EDataOrder.TimeAsc => EDataOrder.TimeDesc,
            EDataOrder.ShiftAsc when DataOrder == EDataOrder.ShiftAsc => EDataOrder.ShiftDesc,
            EDataOrder.UserAsc when DataOrder == EDataOrder.UserAsc => EDataOrder.UserDesc,
            EDataOrder.Tank1Asc when DataOrder == EDataOrder.Tank1Asc => EDataOrder.Tank1Desc,
            EDataOrder.Tank2Asc when DataOrder == EDataOrder.Tank2Asc => EDataOrder.Tank2Desc,
            EDataOrder.Tank3Asc when DataOrder == EDataOrder.Tank3Asc => EDataOrder.Tank3Desc,
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
            EDataOrder.TimeAsc => Query.OrderBy(x => x.Time),
            EDataOrder.TimeDesc => Query.OrderByDescending(x => x.Time),
            EDataOrder.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EDataOrder.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EDataOrder.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EDataOrder.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EDataOrder.Tank1Asc => Query.OrderBy(x => x.Tank1LooseRawValue),
            EDataOrder.Tank1Desc => Query.OrderByDescending(x => x.Tank1LooseRawValue),
            EDataOrder.Tank2Asc => Query.OrderBy(x => x.Tank2LooseRawValue),
            EDataOrder.Tank2Desc => Query.OrderByDescending(x => x.Tank2LooseRawValue),
            EDataOrder.Tank3Asc => Query.OrderBy(x => x.Tank3LooseRawValue),
            EDataOrder.Tank3Desc => Query.OrderByDescending(x => x.Tank3LooseRawValue),
            _ => Query.OrderBy(x => x.Time),
        };
        Datas = await query
            .Include(x => x.Factory1Shift)
            .Include(x => x.Profile)
            .Skip((Page - 1) * 10)
            .Take(10)
            .ToArrayAsync();
    }
}

public enum EDataOrder
{
    TimeAsc,
    TimeDesc,
    ShiftAsc,
    ShiftDesc,
    UserAsc,
    UserDesc,
    Tank1Asc,
    Tank1Desc,
    Tank2Asc,
    Tank2Desc,
    Tank3Asc,
    Tank3Desc,
}