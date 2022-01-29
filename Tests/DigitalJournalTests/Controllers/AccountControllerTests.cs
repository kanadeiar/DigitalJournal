namespace DigitalJournalTests.Controllers;

[TestClass]
public class AccountControllerTests
{
    #region Данные аккаунта

    [TestMethod]
    public void Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequestNotIsAuthenticated_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerMock.Object, roleManagerMock.Object, signInManagerMock.Object, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                }
            }
        };

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.IndexWebModel));
    }

    [TestMethod]
    public void Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView()
    {
        var profileId = 1;
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedRole = "TestRole";
        var expectedDescription = "TestDescriprionRole";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
            ProfileId = profileId
        };
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        IList<string> roles = new List<string> { expectedRole };
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(roles));
        var roleManagerMock = new Mock<RoleManagerMock>();
        IQueryable<Role> rolesQuery = Enumerable.Range(1, 1).Select(r => 
            new Role 
            {
                Name = expectedRole,
                Description = expectedDescription
            }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(rolesQuery);
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequestIsAuthenticated_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        journalContext.Profiles.Add(new Profile
        {
            Id = profileId,
            SurName = expectedSurName,
            UserId = user.Id,
        });
        journalContext.SaveChanges();
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, expectedName, ClaimValueTypes.String),
        }, "Custom");
        var controller = new AccountController(userManagerMock.Object, roleManagerMock.Object, signInManagerMock.Object, journalContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity),
                }
            }
        };

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.IndexWebModel));
        var model = view.Model as AccountController.IndexWebModel;
        Assert
            .AreEqual(user.Id, model.User.Id);
        Assert
            .AreEqual(expectedName, model.User.UserName);
        Assert
            .AreEqual(expectedSurName, model.Profile.SurName);
        Assert
            .AreEqual(1, model.UserRoleNames.Count());
        Assert
            .AreEqual(expectedDescription, model.UserRoleNames.First());
        userManagerMock
            .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()), Times.Once);
    }

    #endregion

    #region Регистрация

    [TestMethod]
    public void Register_SendCorrectRequest_ShouldCorrectView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);

        var result = controller.Register();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = result as ViewResult;
        Assert
            .IsInstanceOfType(view.Model, typeof(AccountController.RegisterWebModel));
    }

    [TestMethod]
    public void Register_SendInvalidModel_ShouldCorrectView()
    {
        var expectedName = "TestName";
        var expectedPassword = "123";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerStub = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendInvalidModel_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.RegisterWebModel
        {
            UserName = expectedName,
            Password = expectedPassword,
            PasswordConfirm = expectedPassword,
        };
        var controller = new AccountController(userManagerStub, roleManagerStub, signInManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Register(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(AccountController.RegisterWebModel));
    }

    [TestMethod]
    public void Register_SendSuccessRequest_ShouldCorrectView()
    {
        var profileId = 1;
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedPassword = "123";
        var userManagerMock = new Mock<UserManagerMock>();
        User callbackUser = default;
        string callbackPassword = default;
        userManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback((User u, string p) => 
            { 
                callbackUser = u; 
                callbackPassword = p; 
            });
        userManagerMock
            .Setup(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var signInManagerMock = new Mock<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Register_SendSuccessRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new AccountController.RegisterWebModel
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            UserName = expectedName,
            Password = expectedPassword,
            PasswordConfirm = expectedPassword,
        };
        var controller = new AccountController(userManagerMock.Object, roleManagerStub, signInManagerMock.Object, journalContext);

        var result = controller.Register(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Home", redirectResult.ControllerName);
        Assert
            .AreEqual(nameof(HomeController.Index), redirectResult.ActionName);
        userManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify();
        signInManagerMock
            .Verify(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
        signInManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackUser);
        Assert
            .AreEqual(expectedPassword, callbackPassword);
        var profile = journalContext.Profiles.SingleOrDefault(p => p.SurName == expectedSurName);
        Assert
            .IsInstanceOfType(profile, typeof(Profile));
        Assert
            .AreEqual(callbackUser.ProfileId, profile.Id);
        Assert
            .AreEqual(callbackUser.Id, profile.UserId);
    }

    #endregion

}

#region Вспомогательное

public class UserManagerMock : UserManager<User>
{
    /// <summary> Мок подменяющий сервис управления пользователями </summary>
    public UserManagerMock()
        : base(new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object)
    {
    }
}

public class RoleManagerMock : RoleManager<Role>
{
    /// <summary> Мок подменяющий сервис управления ролями </summary>
    public RoleManagerMock()
        : base(new Mock<IRoleStore<Role>>().Object,
            new IRoleValidator<Role>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object)
    {
    }
}

public class SignInManagerMock : SignInManager<User>
{
    /// <summary> Мок подменяющий вход/выход пользователей </summary>
    public SignInManagerMock()
        : base(new Mock<UserManagerMock>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object)
    {
    }
}

#endregion

