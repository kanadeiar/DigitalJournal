using DigitalJournal.Domain.Entities.Documents;

namespace DigitalJournal.Dal.Data;

public class DigitalJournalSeedTestData : IDigitalJournalSeedTestData
{
    private static readonly Random _rnd = new Random();

    public async Task<IDigitalJournalSeedTestData> SeedTestData(IServiceProvider provider, IConfiguration configuration)
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
            return this;
        }
        logger.LogInformation("Begin writing test data to database Factory1 ...");

        UserManager<User> userManager = provider.GetRequiredService<UserManager<User>>();

        var pt1 = new Factory1ProductType { Name = "Кирпич полуторный 250x120x65", Number = 1, Units = 360 };
        var pt2 = new Factory1ProductType { Name = "Кирпич двойной 250x120x138", Number = 2, Units = 240 };
        var pt3 = new Factory1ProductType { Name = "Кирпич евро пустотелый 250x120x120", Number = 3, Units = 180 };
        var pt4 = new Factory1ProductType { Name = "Кирпич полнотелый 250x120x88", Number = 4, Units = 300 };
        context.Factory1ProductTypes.AddRange(pt1, pt2, pt3, pt4);
        await context.SaveChangesAsync();
        var products = await context.Factory1ProductTypes.ToDictionaryAsync(s => s.Id, s => s);

        var shA = new Factory1Shift { Name = "Смена А" };
        var shB = new Factory1Shift { Name = "Смена Б" };
        var shC = new Factory1Shift { Name = "Смена В" };
        var shD = new Factory1Shift { Name = "Смена Г" };
        context.Factory1Shifts.AddRange(shA, shB, shC, shD);
        await context.SaveChangesAsync();
        var shifts = await context.Factory1Shifts.ToDictionaryAsync(s => s.Id, s => s);

        var adminUser = await userManager.FindByNameAsync(TestData.Admin.Username);
        var adminProfile = new Profile
        {
            SurName = "Админов",
            FirstName = "Админ",
            Patronymic = "Админович",
            Birthday = DateTime.Today.AddYears(-20),
            UserId = adminUser.Id
        };
        var userUser = await userManager.FindByNameAsync(TestData.User.Username);
        var userProfile = new Profile
        {
            SurName = "Пользов",
            FirstName = "Пользователь",
            Patronymic = "Пользович",
            Birthday = DateTime.Today.AddYears(-18),
            UserId = userUser.Id
        };
        var masterUser = await userManager.FindByNameAsync(TestData.Master.Username);
        var masterProfile = new Profile
        {
            SurName = "Мастеров",
            FirstName = "Мастер",
            Patronymic = "Мастерович",
            Birthday = DateTime.Today.AddYears(-18),
            UserId = masterUser.Id
        };
        var operatorUser = await userManager.FindByNameAsync(TestData.Operator.Username);
        var operatorProfile = new Profile
        {
            SurName = "Операторов",
            FirstName = "Оператор",
            Patronymic = "Операторович",
            Birthday = DateTime.Today.AddYears(-18),
            UserId = masterUser.Id
        };
        context.Profiles.AddRange(adminProfile, userProfile, masterProfile, operatorProfile);
        await context.SaveChangesAsync();
        adminUser.ProfileId = adminProfile.Id;
        await userManager.UpdateAsync(adminUser);
        userUser.ProfileId = userProfile.Id;
        await userManager.UpdateAsync(userUser);
        masterUser.ProfileId = masterProfile.Id;
        await userManager.UpdateAsync(masterUser);
        operatorUser.ProfileId = operatorProfile.Id;
        await userManager.UpdateAsync(operatorUser);

        var w1sds = Enumerable.Range(1, 120).Select(i => new Factory1Warehouse1ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Tank1LooseRawValue = _rnd.NextDouble() * 10.0 + 100.0,
            Tank2LooseRawValue = _rnd.NextDouble() * 10.0 + 200.0,
            Tank3LooseRawValue = _rnd.NextDouble() * 10.0 + 300.0,
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1Warehouse1ShiftData.AddRange(w1sds);
        await context.SaveChangesAsync();

        var p1sds = Enumerable.Range(1, 120).Select(i => new Factory1Press1ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Factory1ProductType = products[i % 4 + 1],
            ProductCount = _rnd.Next(80, 120),
            Loose1RawValue = _rnd.NextDouble() * 10.0,
            Loose2RawValue = _rnd.NextDouble() * 10.0,
            Loose3RawValue = _rnd.NextDouble() * 10.0,
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1Press1ShiftData.AddRange(p1sds);
        await context.SaveChangesAsync();

        var a1sds = Enumerable.Range(1, 120).Select(i => new Factory1Autoclave1ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            AutoclaveNumber = _rnd.Next(1, 5),
            TimeStart = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i - 2),
            AutoclavedTime = new TimeSpan(1, _rnd.Next(59), 0),
            Factory1ProductType = products[i % 4 + 1],
            AutoclavedCount = _rnd.Next(80, 120),
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1Autoclave1ShiftDatas.AddRange(a1sds);
        await context.SaveChangesAsync();

        var pk1sds = Enumerable.Range(1, 120).Select(i => new Factory1Pack1ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Factory1ProductType = products[i % 4 + 1],
            ProductCount = _rnd.Next(80, 120),
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1Pack1ShiftDatas.AddRange(pk1sds);
        await context.SaveChangesAsync();

        var w2sds = Enumerable.Range(1, 120).Select(i => new Factory1Warehouse2ShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Place1ProductType = products[1],
            Place1ProductsCount = _rnd.Next(80, 120),
            Place2ProductType = products[2],
            Place2ProductsCount = _rnd.Next(80, 120),
            Place3ProductType = products[3],
            Place3ProductsCount = _rnd.Next(80, 120),
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1Warehouse2ShiftData.AddRange(w2sds);
        await context.SaveChangesAsync();

        var gsd = Enumerable.Range(1, 120).Select(i => new Factory1GeneralShiftData
        {
            Time = DateTime.Today.AddDays(-60).AddHours(8).AddHours(12 * i),
            Factory1ProductType = products[i % 4 + 1],
            ProductCount = _rnd.Next(80, 120),
            Loose1RawValue = _rnd.NextDouble() * 10.0,
            Loose2RawValue = _rnd.NextDouble() * 10.0,
            Loose3RawValue = _rnd.NextDouble() * 10.0,
            AutoclaveNumber = _rnd.Next(1, 5),
            Factory1PackProductType = products[i % 4 + 1],
            PackProductCount = _rnd.Next(80, 120),
            Factory1Shift = shifts[i % 4 + 1],
            Profile = userProfile,
        });
        context.Factory1GeneralShiftData.AddRange(gsd);
        await context.SaveChangesAsync();

        var c1 = new DocDirectory
        {
            Name = "Общая папка публичная",            
        };
        var c2 = new DocDirectory
        {
            Name = "Секретная папка администратора",
        };
        var c3 = new DocDirectory
        {
            Name = "Папка с документами",
        };
        context.DocDirectories.AddRange(c1, c2, c3);
        await context.SaveChangesAsync();

        var c3_1 = new DocDirectory
        {
            Name = "Внутренняя папка",
            BaseDirectory = c3,
        };
        var d1 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Общий документ 1",
            Description = "Обычный документ",
            Directory = c1,
        };
        var d2 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Секретный документ 1",
            Description = "Самый секретный документ",
            Directory = c2,
        };
        var d3 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Секретный документ 1",
            Description = "Самый секретный документ",
            Directory = c2,
        };
        var d4 = new DocDocument
        {
            Birthday= DateTime.Now,
            Name = "Обычный документ",
            Description = "Типовой текстовый документ",
            Directory = c3,
        };
        context.DocDirectories.Add(c3_1);
        context.DocDocuments.AddRange(d1, d2, d3, d4);
        await context.SaveChangesAsync();

        var d5_1 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Обычный платежный документ 1",
            Description = "Обычный платежный документ 1",
            Directory = c3_1,
        };
        var d5_2 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Обычный платежный документ 2",
            Description = "Обычный платежный документ 2",
            Directory = c3_1,
        };
        var d5_3 = new DocDocument
        {
            Birthday = DateTime.Now,
            Name = "Обычный платежный документ 3",
            Description = "Обычный платежный документ 3",
            Directory = c3_1,
        };
        context.DocDocuments.AddRange(d5_1, d5_2, d5_3);
        await context.SaveChangesAsync();

        var com1 = new DocComment
        {
            Description = "Тестовый комментарий 1",
            Document = d4,
        };
        var com2 = new DocComment
        {
            Description = "Тестовый комментарий 2",
            Document = d5_1,
        };
        var com3 = new DocComment
        {
            Description = "Тестовый комментарий 3",
            Document = d5_1,
        };
        context.DocComment.AddRange(com1, com2, com3);
        await context.SaveChangesAsync();

        logger.LogInformation("Complete writing test data to database Factory1 ...");

        return this;
    }
}

