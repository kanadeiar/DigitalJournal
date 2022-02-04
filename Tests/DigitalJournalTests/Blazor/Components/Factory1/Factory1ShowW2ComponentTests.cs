namespace DigitalJournalTests.Blazor.Components.Factory1;

[TestClass]
public class Factory1ShowW2ComponentTests
{
    [TestMethod]
    public void Init_Initialized_ShouldCorrect()
    {
        var expectedCount = 200;
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
        journalContext.Factory1Warehouse2ShiftData.AddRange(Enumerable.Range(1, 10).Select(i => new Factory1Warehouse2ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Place1ProductType = productType,
            Place1ProductsCount = expectedCount,
            Place2ProductType = productType,
            Place2ProductsCount = 100,
            Place3ProductType = productType,
            Place3ProductsCount = 100,
            Profile = profile,
        }));
        journalContext.SaveChanges();

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.Factory1.Factory1ShowW2Component>(
            builder =>
            {
                builder.Add(c => c.Query, journalContext.Factory1Warehouse2ShiftData);
            });

        Assert
            .IsTrue(component.Markup.Contains("<p class=\"mb-0 mt-2\">Содержимое склада №2 готовой продукции:</p>"));
        Assert.
            AreEqual(1, component.RenderCount);
        var place1value = component.Find("#place1value");
        Assert
            .IsTrue(place1value.ToMarkup().Contains(expectedCount.ToString()));
    }
}

