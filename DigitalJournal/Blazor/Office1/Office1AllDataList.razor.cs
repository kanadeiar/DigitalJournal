namespace DigitalJournal.Blazor.Office1;

public partial class Office1AllDataList
{
    DigitalJournalContext _Context => Service;

    public IQueryable<Office1Skills>? Query => _Context.Office1Skills;
    public IEnumerable<Office1Skills>? Datas { get; set; }

    public AllOffice1Order DataOrder { get; set; } = AllOffice1Order.SurFirstNameAsc;
    public int Page { get; set; } = 1;
    public int PagesCount { get; set; } = 1;

    public async Task SetOrder(AllOffice1Order order)
    {
        DataOrder = order switch
        {
            AllOffice1Order.SurFirstNameAsc when DataOrder == AllOffice1Order.SurFirstNameAsc => AllOffice1Order.SurFirstNameDesc,
            AllOffice1Order.PositionAsc when DataOrder == AllOffice1Order.PositionAsc => AllOffice1Order.PositionDesc,
            AllOffice1Order.AssemblerAsc when DataOrder == AllOffice1Order.AssemblerAsc => AllOffice1Order.AssemblerDesc,
            AllOffice1Order.CCppAsc when DataOrder == AllOffice1Order.CCppAsc => AllOffice1Order.CCppDesc,
            AllOffice1Order.CSharpAsc when DataOrder == AllOffice1Order.CSharpAsc => AllOffice1Order.CSharpDesc,
            AllOffice1Order.JavaAsc when DataOrder == AllOffice1Order.JavaAsc => AllOffice1Order.JavaDesc,
            AllOffice1Order.PHPAsc when DataOrder == AllOffice1Order.PHPAsc => AllOffice1Order.PHPDesc,
            AllOffice1Order.SQLAsc when DataOrder == AllOffice1Order.SQLAsc => AllOffice1Order.SQLDesc,
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
            AllOffice1Order.SurFirstNameAsc => Query.OrderBy(x => x.SurName).ThenBy(x => x.FirstName),
            AllOffice1Order.SurFirstNameDesc => Query.OrderByDescending(x => x.SurName).ThenByDescending(x => x.FirstName),
            AllOffice1Order.PositionAsc => Query.Include(x => x.Office1Position).OrderBy(x => x.Office1Position.Name),
            AllOffice1Order.PositionDesc => Query.Include(x => x.Office1Position).OrderByDescending(x => x.Office1Position.Name),
            AllOffice1Order.AssemblerAsc => Query.OrderBy(x => x.Assembler),
            AllOffice1Order.AssemblerDesc => Query.OrderByDescending(x => x.Assembler),
            AllOffice1Order.CCppAsc => Query.OrderBy(x => x.CCpp),
            AllOffice1Order.CCppDesc => Query.OrderByDescending(x => x.CCpp),
            AllOffice1Order.CSharpAsc => Query.OrderBy(x => x.CSharp),
            AllOffice1Order.CSharpDesc => Query.OrderByDescending(x => x.CSharp),
            AllOffice1Order.JavaAsc => Query.OrderBy(x => x.Java),
            AllOffice1Order.JavaDesc => Query.OrderByDescending(x => x.Java),
            AllOffice1Order.PHPAsc => Query.OrderBy(x => x.PHP),
            AllOffice1Order.PHPDesc => Query.OrderByDescending(x => x.PHP),
            AllOffice1Order.SQLAsc => Query.OrderBy(x => x.SQL),
            AllOffice1Order.SQLDesc => Query.OrderByDescending(x => x.SQL),
            _ => Query.OrderBy(x => x.SurName).ThenBy(x => x.FirstName),
        };
        Datas = await query
            .Include(x => x.Office1Position)
            .Skip((Page - 1) * 10)
            .Take(10)
            .ToArrayAsync();
    }
}

public enum AllOffice1Order
{
    SurFirstNameAsc,
    SurFirstNameDesc,
    PositionAsc,
    PositionDesc,
    AssemblerAsc,
    AssemblerDesc,
    CCppAsc,
    CCppDesc,
    CSharpAsc,
    CSharpDesc,
    JavaAsc,
    JavaDesc,
    PHPAsc,
    PHPDesc,
    SQLAsc,
    SQLDesc,
}