namespace DigitalJournalTests.Controllers;

[TestClass]
public class ApiAccountControllerTests
{
    [TestMethod]
    public void Login_SendCorrectRequest_ShouldResultOk()
    {
        var expectedCred = new Credentials
        {
            UserName = "test",
            Password = "123",
        };
        var userManagerFake = Mock.Of<UserManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        var configurationFake = Mock.Of<IConfiguration>();
        signInManagerFake
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
        var controller = new ApiAccountController(signInManagerFake.Object, userManagerFake, configurationFake);

        var model = controller.Login(expectedCred).Result;

        Assert
            .IsInstanceOfType(model, typeof(OkResult));
        signInManagerFake
            .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once());
    }
    [TestMethod]
    public void Login_SendNoSuccessRequest_ShouldReturnUnauthorized()
    {
        var expectedCred = new Credentials
        {
            UserName = "test",
            Password = "123",
        };
        var userManagerFake = Mock.Of<UserManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        var configurationFake = Mock.Of<IConfiguration>();
        signInManagerFake
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));
        var controller = new ApiAccountController(signInManagerFake.Object, userManagerFake, configurationFake);

        var model = controller.Login(expectedCred).Result;

        Assert
            .IsInstanceOfType(model, typeof(UnauthorizedResult));
        signInManagerFake
            .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once());
    }
    [TestMethod]
    public void SignOut_SendCorrectRequest_ShouldReturnOk()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        var configurationFake = Mock.Of<IConfiguration>();
        signInManagerFake
            .Setup(x => x.SignOutAsync());
        var controller = new ApiAccountController(signInManagerFake.Object, userManagerFake, configurationFake);

        var model = controller.Logout().Result;

        Assert
            .IsInstanceOfType(model, typeof(OkResult));
        signInManagerFake
            .Verify(_ => _.SignOutAsync());
    }
}
