namespace DigitalJournalTests.Blazor.Components.Factory1;

[TestClass]
public class Factory1ShowW2ComponentTests
{
    [TestMethod]
    public void Init_Initialized_ShouldCorrect()
    {
        var expectedCount = 200;
        using var context = new Bunit.TestContext();
        var data = new Factory1Warehouse2ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Place1ProductType = new Factory1ProductType { Name = "Test" },
            Place1ProductsCount = expectedCount,
            Place2ProductType = new Factory1ProductType { Name = "Test" },
            Place2ProductsCount = 100,
            Place3ProductType = new Factory1ProductType { Name = "Test" },
            Place3ProductsCount = 100,
            Profile = new Profile(),
        };

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.Factory1.Factory1ShowW2Component>(
            builder =>
            {
                builder.Add(c => c.Data, data);
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

