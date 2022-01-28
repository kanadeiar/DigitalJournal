// dotnet ef --startup-project ../DigitalJournal/ migrations add init --context DigitalJournalContext

namespace DigitalJournal.Dal.Data;

public class DigitalJournalContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }

    public DbSet<Factory1Shift> Factory1Shifts { get; set; }
    public DbSet<Factory1ProductType> Factory1ProductTypes { get; set; }

    public DbSet<Factory1Warehouse1ShiftData> Factory1Warehouse1ShiftData { get; set; }
    public DbSet<Factory1Press1ShiftData> Factory1Press1ShiftData { get;set; }
    public DbSet<Factory1Autoclave1ShiftData> Factory1Autoclave1ShiftDatas { get; set; }
    public DbSet<Factory1Pack1ShiftData> Factory1Pack1ShiftDatas { get; set; }
    public DbSet<Factory1Warehouse2ShiftData> Factory1Warehouse2ShiftData { get; set; }
    public DbSet<Factory1GeneralShiftData> Factory1GeneralShiftData { get; set; }

    public DigitalJournalContext(DbContextOptions<DigitalJournalContext> options) : base(options)
    { }
}

