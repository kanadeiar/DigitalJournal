namespace DigitalJournal.Dal.Data;

public static class DigitalJournalSeedTestData
{
    private static readonly Random _rnd = new Random();

    public static async Task SeedTestData(IServiceProvider provider, IConfiguration configuration)
    {
        provider = provider.CreateScope().ServiceProvider;
        var logger = provider.GetRequiredService<ILogger<DigitalJournalContext>>();
        using var context = new DigitalJournalContext(provider.GetRequiredService<DbContextOptions<DigitalJournalContext>>());

        if (context == null || context.Factory1ProductTypes == null)
        {
            logger.LogError("Null DigitalJournalContext");
            throw new ArgumentNullException("Null DigitalJournalContext");
        }
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Applying migrations: {string.Join(",", pendingMigrations)}");
            await context.Database.MigrateAsync();
        }
        if (context.Factory1ProductTypes.Any())
        {
            logger.LogInformation("Factory1 database contains data - database init with test data is not required");
            return;
        }
        logger.LogInformation("Begin writing test data to database Factory1 ...");

        UserManager<User> userManager = provider.GetRequiredService<UserManager<User>>();

        var pt1 = new Factory1ProductType { Name = "Кирпич полуторный 250x120x65", Number = 1, Units = 360 };
        var pt2 = new Factory1ProductType { Name = "Кирпич двойной 250x120x138", Number = 2, Units = 240 };
        var pt3 = new Factory1ProductType { Name = "Кирпич евро пустотелый 250x120x120", Number = 3, Units = 180 };
        var pt4 = new Factory1ProductType { Name = "Кирпич полнотелый 250x120x88", Number = 4, Units = 300 };
        context.Factory1ProductTypes.AddRange(pt1, pt2, pt3, pt4);
        await context.SaveChangesAsync();

        var shA = new Factory1Shift { Name = "Смена А" };
        var shB = new Factory1Shift { Name = "Смена Б" };
        var shC = new Factory1Shift { Name = "Смена В" };
        var shD = new Factory1Shift { Name = "Смена Г" };
        context.Factory1Shifts.AddRange(shA, shB, shC, shD);
        await context.SaveChangesAsync();
        var shifts = await context.Factory1Shifts.ToDictionaryAsync(s => s.Id, s => s);

        var w1sds = Enumerable.Range(1, 120).Select(i => new Factory1Warehouse1ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Tank1LooseRawValue = _rnd.NextDouble() * 10.0 + 100.0,
            Tank2LooseRawValue = _rnd.NextDouble() * 10.0 + 200.0,
            Tank3LooseRawValue = _rnd.NextDouble() * 10.0 + 300.0,
            Factory1Shift = shifts[i % 4 + 1],
            UserId = (userManager.FindByNameAsync(TestData.User.Username).Result).Id,
        });
        context.Factory1Warehouse1ShiftData.AddRange(w1sds);
        await context.SaveChangesAsync();            



        logger.LogInformation("Complete writing test data to database Factory1 ...");
    }
}

