namespace DigitalJournal.Blazor.Components.Factory1;

public partial class Factory1ShowGComponent
{
    [Parameter]
    public IQueryable<Factory1GeneralShiftData>? Query { get; set; }
    public Factory1GeneralShiftData? Data { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Data = await Query
            .Include(x => x.Factory1ProductType)
            .Include(x => x.Factory1PackProductType)
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
    }
}

