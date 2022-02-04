namespace DigitalJournal.Blazor.Components.Factory1;

public partial class Factory1ShowW2Component
{
    [Parameter]
    public IQueryable<Factory1Warehouse2ShiftData>? Query { get; set; }
    public Factory1Warehouse2ShiftData? Factory1Warehouse2ShiftData { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Factory1Warehouse2ShiftData = await Query
            .Include(x => x.Place1ProductType)
            .Include(x => x.Place2ProductType)
            .Include(x => x.Place3ProductType)
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
    }
}

