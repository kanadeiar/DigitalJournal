namespace DigitalJournalTests.Controllers;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void Index_SendCorrectRequest_ShouldCorrectView()
    {
        var controller = new HomeController();

        var result = controller.Index();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void About_SendCorrectRequest_ShouldCorrectView()
    {
        var controller = new HomeController();

        var result = controller.About();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void Error_SendRequest404_ShouldCorrectView()
    {
        var controller = new HomeController();

        var result = controller.Error("404");

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void Error_SendRequest1_ShouldCorrectView()
    {
        var expectedString = "Status --- 1";
        var controller = new HomeController();

        var result = controller.Error("1");

        Assert
            .IsInstanceOfType(result, typeof(ContentResult));
        var contentResult = ((ContentResult)result).Content;
        Assert
            .AreEqual(expectedString, contentResult);
    }
}

