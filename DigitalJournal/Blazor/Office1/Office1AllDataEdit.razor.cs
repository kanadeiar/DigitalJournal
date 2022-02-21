namespace DigitalJournal.Blazor.Office1;
public partial class Office1AllDataEdit
{
    DigitalJournalContext _Context => Service;
    [Parameter]
    public int Id { get; set; }
    public bool IsModeCreate => Id == 0;

    public Office1Skills? Data { get; set; }
    public IDictionary<int, string> Positions { get; set; } = new Dictionary<int, string>();

    protected override async Task OnParametersSetAsync()
    {
        if (IsModeCreate)
        {
            Data = new Office1Skills();
        }
        else
            Data = await _Context.Office1Skills.FindAsync(Id);
        Positions = await _Context.Office1Positions
            .ToDictionaryAsync(s => s.Id, s => s.Name);
    }
    public async Task HandleValidSubmit()
    {
        if (IsModeCreate && Data is { })
        {
            _Context.Office1Skills.Add(Data);
        }
        await _Context.SaveChangesAsync();
        NavManager.NavigateTo("office1/all");
    }
    public string Mode => (Id == 0) ? "Добавление нового сотрудника" : "Редактирование данных сотрудника";
    public string Theme => IsModeCreate ? "success" : "info";
    public string TextColor => IsModeCreate ? "white" : "dark";
}