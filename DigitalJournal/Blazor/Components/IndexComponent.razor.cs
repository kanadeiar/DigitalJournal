namespace DigitalJournal.Blazor.Components;

public partial class IndexComponent
{
    [Parameter]
    public IQueryable<Factory1Warehouse1ShiftData>? Query { get; set; }
    public IEnumerable<Factory1Warehouse1ShiftData>? Datas { get; set; }
    [Parameter]
    public IQueryable<Factory1Warehouse2ShiftData>? Query2 { get; set; }
    public IEnumerable<Factory1Warehouse2ShiftData>? Datas2 { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await UpdateData();
    }
    private async Task UpdateData()
    {
        Datas = await Query
            .Include(x => x.Profile)
            .OrderByDescending(x => x.Time)
            .Take(10)
            .ToArrayAsync();
        Datas2 = await Query2
            .Include(x => x.Profile)
            .Include(x => x.Place1ProductType)
            .Include(x => x.Place2ProductType)
            .Include(x => x.Place3ProductType)
            .OrderByDescending(x => x.Time)
            .Take(10)
            .ToArrayAsync();
    }
}

