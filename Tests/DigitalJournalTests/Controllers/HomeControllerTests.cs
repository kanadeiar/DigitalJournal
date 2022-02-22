namespace DigitalJournalTests.Controllers;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void Index_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedMon = new SimpleReportWebModel
        {
            DimensionOne = "test",
            Quantity = 1,
        };
        var homeInfoServiceFake = new Mock<IHomeInfoService>();
        homeInfoServiceFake
            .Setup(_ => _.GetMoneyInfoForFactory1())
            .Returns(Task.FromResult((IList<SimpleReportWebModel>)new List<SimpleReportWebModel> { expectedMon }));
        var controller = new HomeController(homeInfoServiceFake.Object);

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var models = (result as ViewResult).Model as IEnumerable<SimpleReportWebModel>;
        Assert
            .AreEqual(expectedMon.DimensionOne, models.First().DimensionOne);
    }

    [TestMethod]
    public void About_SendCorrectRequest_ShouldCorrectView()
    {
        var homeInfoServiceFake = Mock.Of<IHomeInfoService>();
        var controller = new HomeController(homeInfoServiceFake);

        var result = controller.About();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void Error_SendRequest404_ShouldCorrectView()
    {
        var homeInfoServiceFake = Mock.Of<IHomeInfoService>();
        var controller = new HomeController(homeInfoServiceFake);

        var result = controller.Error("404");

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public void Error_SendRequest1_ShouldCorrectView()
    {
        var expectedString = "Status --- 1";
        var homeInfoServiceFake = Mock.Of<IHomeInfoService>();
        var controller = new HomeController(homeInfoServiceFake);

        var result = controller.Error("1");

        Assert
            .IsInstanceOfType(result, typeof(ContentResult));
        var contentResult = ((ContentResult)result).Content;
        Assert
            .AreEqual(expectedString, contentResult);
    }
}

