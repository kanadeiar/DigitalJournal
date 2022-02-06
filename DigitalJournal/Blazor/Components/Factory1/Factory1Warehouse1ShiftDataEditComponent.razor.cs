namespace DigitalJournal.Blazor.Components.Factory1;

public partial class Factory1Warehouse1ShiftDataEditComponent
{
    [Parameter]
    public int Id { get; set; }
    public bool IsModeCreate => Id == 0;
    [Parameter]
    public DbSet<Factory1Warehouse1ShiftData> DbSet { get; set; }
    [Parameter]
    public IQueryable<Factory1Shift> Factory1ShiftsQuery { get; set; }
    [Parameter]
    public IQueryable<Profile> Factory1ProfilesQuery { get; set; }
    [Parameter]
    public EventCallback SaveChangesCallback { get; set; }
    public async Task SaveChangesInvoke()
    {
        await SaveChangesCallback.InvokeAsync();
    }

    public Factory1Warehouse1ShiftData? Data { get; set; }
    public IDictionary<int, string> Factory1Shifts { get; set; } = new Dictionary<int, string>();
    public IDictionary<int, string> Profiles { get; set; } = new Dictionary<int, string>();

    protected override async Task OnParametersSetAsync()
    {
        if (IsModeCreate)
        {
            Data = new Factory1Warehouse1ShiftData();
            if (DateTime.Now.Hour < 8)
                Data.Time = DateTime.Today.AddHours(-4);
            else
                Data.Time = DateTime.Today.AddHours(8);
        }
        else
            Data = await DbSet.FindAsync(Id);
        Factory1Shifts = await Factory1ShiftsQuery
            .ToDictionaryAsync(s => s.Id, s => s.Name);
        Profiles = await Factory1ProfilesQuery
            .ToDictionaryAsync(x => x.Id, x => $"{x.SurName} {x.FirstName.FirstOrDefault()}.{x.Patronymic.FirstOrDefault()}.");
    }

    public async Task HandleValidSubmit()
    {
        if (IsModeCreate && Data is { })
        {
            DbSet.Add(Data);
        }
        await SaveChangesInvoke();
    }

    public string Mode => IsModeCreate ? "Добавление данных за смену" : "Редактирование данных за смену";
    public string Theme => IsModeCreate ? "success" : "info";
    public string TextColor => IsModeCreate ? "white" : "dark";
}

