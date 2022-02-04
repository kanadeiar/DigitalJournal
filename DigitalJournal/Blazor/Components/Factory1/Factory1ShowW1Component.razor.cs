namespace DigitalJournal.Blazor.Components.Factory1;

public partial class Factory1ShowW1Component
{
    [Parameter]
    public IQueryable<Factory1Warehouse1ShiftData>? Query { get; set; }
    public Factory1Warehouse1ShiftData? Factory1Warehouse1ShiftData { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Factory1Warehouse1ShiftData = await Query
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
    }
}

