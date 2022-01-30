namespace DigitalJournal.Blazor.Components;

public partial class IndexComponent
{
    [Parameter]
    public IQueryable<Factory1Warehouse1ShiftData>? Query { get; set; }
    public IEnumerable<Factory1Warehouse1ShiftData>? Datas { get; set; }
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
    }
}

