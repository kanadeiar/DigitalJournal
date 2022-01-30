namespace DigitalJournalTests.Blazor.Components;

[TestClass]
public class IndexComponentTests
{
    [TestMethod]
    public void Init_Initialized_ShouldCorrect()
    {
        using var context = new Bunit.TestContext();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Init_Initialized_ShouldCorrect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile { SurName = "Testov", FirstName = "Test", Patronymic = "Testovich", UserId = "test" };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        journalContext.Factory1Warehouse1ShiftData.AddRange(Enumerable.Range(1, 10).Select(i => new Factory1Warehouse1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Tank1LooseRawValue = 100,
            Profile = profile,
        }));
        journalContext.SaveChanges();

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.IndexComponent>(
            builder =>
            {
                builder.Add(c => c.Query, journalContext.Factory1Warehouse1ShiftData);
            });

        Assert
            .AreEqual(1, component.RenderCount);
        var paraElms = component.FindAll(".list-group-item");
        Assert
            .AreEqual(10, paraElms.Count());
        Assert
            .IsTrue(paraElms.First().ToMarkup().Contains("01.01.2021"));
        Assert
            .IsTrue(paraElms.First().ToMarkup().Contains("100"));
    }
}

