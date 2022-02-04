namespace DigitalJournalTests.Blazor.Components.Factory1;

[TestClass]
public class Factory1ShowPAPComponentTests
{
    [TestMethod]
    public void Init_Initialized_ShouldCorrect()
    {
        var expectedPressCount = 100;
        var expectedAutoclaveNumber = 1;
        var expectedPackCount = 100;
        using var context = new Bunit.TestContext();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Init_Initialized_ShouldCorrect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile { SurName = "Testov", FirstName = "Test", Patronymic = "Testovich", UserId = "test" };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        var productType = new Factory1ProductType { Number = 1, Name = "Test" };
        journalContext.Factory1ProductTypes.Add(productType);
        journalContext.SaveChanges();
        journalContext.Factory1Press1ShiftData.AddRange(Enumerable.Range(1, 10).Select(i => new Factory1Press1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = productType,
            ProductCount = expectedPressCount,
            Loose1RawValue = 10.0,
            Loose2RawValue = 20.0,
            Loose3RawValue = 30.0,
            Profile = profile,
        }));
        journalContext.SaveChanges();
        journalContext.Factory1Autoclave1ShiftDatas.AddRange(Enumerable.Range(1, 10).Select(i => new Factory1Autoclave1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = productType,
            AutoclaveNumber = expectedAutoclaveNumber,
            TimeStart = new DateTime(2021, 1, 1),
            AutoclavedTime = new TimeSpan(1, 2, 0),
            Profile = profile,
        }));
        journalContext.SaveChanges();
        journalContext.Factory1Pack1ShiftDatas.AddRange(Enumerable.Range(1, 10).Select(i => new Factory1Pack1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = productType,
            ProductCount = expectedPackCount,
            Profile = profile,
        }));
        journalContext.SaveChanges();

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.Factory1.Factory1ShowPAPComponent>(
            builder =>
            {
                builder.Add(c => c.QueryPress, journalContext.Factory1Press1ShiftData);
                builder.Add(c => c.QueryAutoclave, journalContext.Factory1Autoclave1ShiftDatas);
                builder.Add(c => c.QueryPack, journalContext.Factory1Pack1ShiftDatas);
            });

        Assert
            .IsTrue(component.Markup.Contains("<h6 class=\"mt-4\">Последние данные по производству:</h6>"));
        Assert.
            AreEqual(1, component.RenderCount);
        var pressValues = component.Find("#pressValues");
        Assert
            .IsTrue(pressValues.ToMarkup().Contains(expectedPressCount.ToString()));
        var autoclaveValues = component.Find("#autoclaveValues");
        Assert
            .IsTrue(autoclaveValues.ToMarkup().Contains(expectedAutoclaveNumber.ToString()));
        var packValues = component.Find("#packValues");
        Assert
            .IsTrue(packValues.ToMarkup().Contains(expectedPackCount.ToString()));
    }
}

