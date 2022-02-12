﻿namespace DigitalJournal.Blazor.Factory1;

public partial class Factory1Press1ShiftDataEdit
{
    DigitalJournalContext _Context => Service;
    [Parameter]
    public int Id { get; set; }
    public bool IsModeCreate => Id == 0;

    public Factory1Press1ShiftData? Data { get; set; }
    public IDictionary<int, string> Factory1Shifts { get; set; } = new Dictionary<int, string>();
    public IDictionary<int, string> Profiles { get; set; } = new Dictionary<int, string>();
    public IDictionary<int, string> ProductTypes { get; set; } = new Dictionary<int, string>();

    protected override async Task OnParametersSetAsync()
    {
        if (IsModeCreate)
        {
            Data = new Factory1Press1ShiftData();
            if (DateTime.Now.Hour < 8)
                Data.Time = DateTime.Today.AddHours(-4);
            else if (DateTime.Now.Hour >= 20)
                Data.Time = DateTime.Today.AddHours(8).AddHours(12);
            else
                Data.Time = DateTime.Today.AddHours(8);
        }
        else
            Data = await _Context.Factory1Press1ShiftData.FindAsync(Id);
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
            _Context.Factory1Press1ShiftData.Add(Data);
        }
        await _Context.SaveChangesAsync();
        NavManager.NavigateTo("factory1/press1");
    }
    public string Mode => IsModeCreate ? "Добавление данных по прессу 1 за смену" : "Редактирование данных по прессу 1 за смену";
    public string Theme => IsModeCreate ? "success" : "info";
    public string TextColor => IsModeCreate ? "white" : "dark";
}

