namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1GeneralShiftDataEdit
{
    DigitalJournalContext _Context => Service;
    [Parameter]
    public int Id { get; set; }
    public bool IsModeCreate => Id == 0;

    public Factory1GeneralShiftData? Data { get; set; }
    public IDictionary<int, string> Factory1Shifts { get; set; } = new Dictionary<int, string>();
    public IDictionary<int, string> Profiles { get; set; } = new Dictionary<int, string>();
    public IDictionary<int, string> ProductTypes { get; set; } = new Dictionary<int, string>();

    protected override async Task OnParametersSetAsync()
    {
        if (IsModeCreate)
        {
            Data = new Factory1GeneralShiftData();
            if (DateTime.Now.Hour < 8)
                Data.Time = DateTime.Today.AddHours(-4);
            else if (DateTime.Now.Hour >= 20)
                Data.Time = DateTime.Today.AddHours(8).AddHours(12);
            else
                Data.Time = DateTime.Today.AddHours(8);
            var press1 = await _Context.Factory1Press1ShiftData.FirstOrDefaultAsync(x => x.Time == Data.Time);
            Data.Factory1ProductTypeId = press1?.Factory1ProductTypeId ?? 0;
            Data.ProductCount = press1?.ProductCount ?? 0;
            Data.Loose1RawValue = press1?.Loose1RawValue ?? 0.0;
            Data.Loose2RawValue = press1?.Loose2RawValue ?? 0.0;
            Data.Loose2RawValue = press1?.Loose2RawValue ?? 0.0;
            var autoclave1 = await _Context.Factory1Autoclave1ShiftDatas.FirstOrDefaultAsync(x => x.Time == Data.Time);
            Data.AutoclaveNumber = autoclave1?.AutoclaveNumber ?? 0;
            var pack1 = await _Context.Factory1Pack1ShiftDatas.FirstOrDefaultAsync(x => x.Time == Data.Time);
            Data.Factory1PackProductTypeId = pack1?.Factory1ProductTypeId ?? 0;
            Data.PackProductCount = pack1?.ProductCount ?? 0;
        }
        else
            Data = await _Context.Factory1GeneralShiftData.FindAsync(Id);
        Factory1Shifts = await _Context.Factory1Shifts
            .ToDictionaryAsync(s => s.Id, s => s.Name);
        Profiles = await _Context.Profiles
            .ToDictionaryAsync(x => x.Id, x => $"{x.SurName} {x.FirstName.FirstOrDefault()}.{x.Patronymic.FirstOrDefault()}.");
        ProductTypes = await _Context.Factory1ProductTypes
            .ToDictionaryAsync(x => x.Id, x => $"[{x.Number}] {x.Name}");
    }
    public async Task HandleValidSubmit()
    {
        if (IsModeCreate && Data is { })
        {
            _Context.Factory1GeneralShiftData.Add(Data);
        }
        await _Context.SaveChangesAsync();
        NavManager.NavigateTo("factory1/general");
    }
    public string Mode => (Id == 0) ? "Добавление данных смены за смену" : "Редактирование данных смены за смену";
    public string Theme => IsModeCreate ? "success" : "info";
    public string TextColor => IsModeCreate ? "white" : "dark";
}

