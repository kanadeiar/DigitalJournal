namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Autoclave1ShiftDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Factory1Autoclave1ShiftData>? Query => _Context.Factory1Autoclave1ShiftDatas;
    public IEnumerable<Factory1Autoclave1ShiftData>? Datas { get; set; }

    public EAutoclave1Order DataOrder { get; set; } = EAutoclave1Order.TimeDesc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(EAutoclave1Order order)
    {
        DataOrder = order switch
        {
            EAutoclave1Order.TimeAsc when DataOrder == EAutoclave1Order.TimeAsc => EAutoclave1Order.TimeDesc,
            EAutoclave1Order.ShiftAsc when DataOrder == EAutoclave1Order.ShiftAsc => EAutoclave1Order.ShiftDesc,
            EAutoclave1Order.UserAsc when DataOrder == EAutoclave1Order.UserAsc => EAutoclave1Order.UserDesc,
            EAutoclave1Order.ProductNameAsc when DataOrder == EAutoclave1Order.ProductNameAsc => EAutoclave1Order.ProductNameDesc,
            EAutoclave1Order.ProductCountAsc when DataOrder == EAutoclave1Order.ProductCountAsc => EAutoclave1Order.ProductCountDesc,
            EAutoclave1Order.AutoclaveAsc when DataOrder == EAutoclave1Order.AutoclaveAsc => EAutoclave1Order.AutoclaveDesc,
            EAutoclave1Order.TimeStartAsc when DataOrder == EAutoclave1Order.TimeStartAsc => EAutoclave1Order.TimeStartDesc,
            EAutoclave1Order.AutoclaveTimeAsc when DataOrder == EAutoclave1Order.AutoclaveTimeAsc => EAutoclave1Order.AutoclaveTimeDesc,
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
            EAutoclave1Order.TimeAsc => Query.OrderBy(x => x.Time),
            EAutoclave1Order.TimeDesc => Query.OrderByDescending(x => x.Time),
            EAutoclave1Order.ShiftAsc => Query.Include(x => x.Factory1Shift).OrderBy(x => x.Factory1Shift.Name),
            EAutoclave1Order.ShiftDesc => Query.Include(x => x.Factory1Shift).OrderByDescending(x => x.Factory1Shift.Name),
            EAutoclave1Order.UserAsc => Query.Include(x => x.Profile).OrderBy(x => x.Profile.SurName),
            EAutoclave1Order.UserDesc => Query.Include(x => x.Profile).OrderByDescending(x => x.Profile.SurName),
            EAutoclave1Order.ProductNameAsc => Query.Include(x => x.Factory1ProductType).OrderBy(x => x.Factory1ProductType.Name),
            EAutoclave1Order.ProductNameDesc => Query.Include(x => x.Factory1ProductType).OrderByDescending(x => x.Factory1ProductType.Name),
            EAutoclave1Order.ProductCountAsc => Query.OrderBy(x => x.AutoclavedCount),
            EAutoclave1Order.ProductCountDesc => Query.OrderByDescending(x => x.AutoclavedCount),
            EAutoclave1Order.AutoclaveAsc => Query.OrderBy(x => x.AutoclaveNumber),
            EAutoclave1Order.AutoclaveDesc => Query.OrderByDescending(x => x.AutoclaveNumber),
            EAutoclave1Order.TimeStartAsc => Query.OrderBy(x => x.TimeStart),
            EAutoclave1Order.TimeStartDesc => Query.OrderByDescending(x => x.TimeStart),
            EAutoclave1Order.AutoclaveTimeAsc => Query.OrderBy(x => x.AutoclavedTime),
            EAutoclave1Order.AutoclaveTimeDesc => Query.OrderByDescending(x => x.AutoclavedTime),
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

public enum EAutoclave1Order
{
    TimeAsc,
    TimeDesc,
    ShiftAsc,
    ShiftDesc,
    UserAsc,
    UserDesc,
    AutoclaveAsc,
    AutoclaveDesc,
    TimeStartAsc,
    TimeStartDesc,
    AutoclaveTimeAsc,
    AutoclaveTimeDesc,
    ProductNameAsc,
    ProductNameDesc,
    ProductCountAsc,
    ProductCountDesc,
}