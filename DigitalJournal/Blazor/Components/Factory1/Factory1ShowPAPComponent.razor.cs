namespace DigitalJournal.Blazor.Components.Factory1;

public partial class Factory1ShowPAPComponent
{
    [Parameter]
    public IQueryable<Factory1Press1ShiftData>? QueryPress { get; set; }
    [Parameter]
    public IQueryable<Factory1Autoclave1ShiftData>? QueryAutoclave { get; set; }
    [Parameter]
    public IQueryable<Factory1Pack1ShiftData>? QueryPack { get; set; }

    public Factory1Press1ShiftData? Factory1Press1ShiftData { get; set; }
    public Factory1Autoclave1ShiftData? Factory1Autoclave1ShiftData { get; set; }
    public Factory1Pack1ShiftData? Factory1Pack1ShiftData { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Factory1Press1ShiftData = await QueryPress
            .Include(x => x.Factory1ProductType)
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
        Factory1Autoclave1ShiftData = await QueryAutoclave
            .Include(x => x.Factory1ProductType)
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
        Factory1Pack1ShiftData = await QueryPack
            .Include(x => x.Factory1ProductType)
            .OrderByDescending(x => x.Time).FirstOrDefaultAsync();
    }
}

