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
        var pressData = new Factory1Press1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = new Factory1ProductType { Name = "Test" },
            ProductCount = expectedPressCount,
            Loose1RawValue = 10.0,
            Loose2RawValue = 20.0,
            Loose3RawValue = 30.0,
            Profile = new Profile(),
        };
        var autoclaveData = new Factory1Autoclave1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = new Factory1ProductType { Name = "Test" },
            AutoclaveNumber = expectedAutoclaveNumber,
            TimeStart = new DateTime(2021, 1, 1),
            AutoclavedTime = new TimeSpan(1, 2, 0),
            Profile = new Profile(),
        };
        var packData = new Factory1Pack1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Factory1ProductType = new Factory1ProductType { Name = "Test" },
            ProductCount = expectedPackCount,
            Profile = new Profile(),
        };

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.Factory1.Factory1ShowPAPComponent>(
            builder =>
            {
                builder.Add(c => c.Factory1Press1ShiftData, pressData);
                builder.Add(c => c.Factory1Autoclave1ShiftData, autoclaveData);
                builder.Add(c => c.Factory1Pack1ShiftData, packData);
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

