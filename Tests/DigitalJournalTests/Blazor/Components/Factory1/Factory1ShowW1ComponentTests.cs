namespace DigitalJournalTests.Blazor.Components.Factory1;

[TestClass]
public class Factory1ShowW1ComponentTests
{
    [TestMethod]
    public void Init_Initialized_ShouldCorrect()
    {
        using var context = new Bunit.TestContext();
        var data = new Factory1Warehouse1ShiftData
        {
            Time = new DateTime(2021, 1, 1),
            Tank1LooseRawValue = 100,
            Profile = new Profile(),
        };

        var component = context.RenderComponent<DigitalJournal.Blazor.Components.Factory1.Factory1ShowW1Component>(
            builder =>
            {
                builder.Add(c => c.Data, new Factory1Warehouse1ShiftData());
            });

        Assert
            .IsTrue(component.Markup.Contains("<p class=\"mb-0 mt-2\">Содержимое склада №1 сырья:</p>"));
        Assert.
            AreEqual(1, component.RenderCount);
        var tank1Value = component.Find("#tank1value");
    }
}

