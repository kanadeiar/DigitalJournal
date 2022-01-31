namespace DigitalJournalTests.Services;

[TestClass]
public class AccountServiceTests
{
    readonly Random random = new Random();

    [TestMethod]
    public void GetIndexWebModel_GetCorrect_ShouldModel()
    {
        var expectedNameRole = "expRole";
        var expectedRoleDescription = "descriptionRole";
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var expProfile = new Profile
        {
            SurName = "testSurName",
        };
        IQueryable<Role> expectedRoleQuery = Enumerable.Range(1, 1).Select(r =>
            new Role
            {
                Name = expectedNameRole,
                Description = expectedRoleDescription,
            }).AsQueryable();
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = new Mock<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        userManagerFake
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        userManagerFake
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>) expectedRoleQuery.Select(x => x.Name).ToList()));
        roleManagerFake
            .Setup(_ => _.Roles)
            .Returns(expectedRoleQuery);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        expProfile.UserId = expectedUser.Id;
        journalContext.Profiles.Add(expProfile);
        journalContext.SaveChanges();
        expectedUser.ProfileId = expProfile.Id;
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake.Object, signInManagerFake, journalContext);

        var webModel = service.GetIndexWebModel(expectedUser.UserName).Result;

        Assert
            .IsInstanceOfType(webModel, typeof(IndexWebModel));
        var model = (IndexWebModel)webModel;
        Assert
            .AreEqual(expectedUser.Id, model.User.Id);
        Assert
            .AreEqual(expectedUser.UserName, model.User.UserName);
        Assert
            .AreEqual(expProfile.SurName, model.Profile.SurName);
        Assert
            .AreEqual(1, model.UserRoleNames.Count());
        Assert
            .AreEqual(expectedRoleDescription, model.UserRoleNames.First());
    }

    [TestMethod]
    public void RequestRegisterUser_CreateSuccess_ShouldSuccess()
    {
        var expectedModel = new RegisterWebModel
        {
            SurName = "testSurName",
            FirstName = "test",
            Patronymic = "test",
            Email = "test@example.com",
            Birthday = new DateTime(2021, 1, 1),
            UserName = "expectedName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        userManagerFake
            .Setup(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
        signInManagerFake
            .Setup(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake.Object, journalContext);

        var (result, errors) = service.RequestRegisterUser(expectedModel).Result;

        Assert
            .IsTrue(result);
        Assert
            .AreEqual(0, errors.Count());
        userManagerFake
            .Verify(_ => _.AddToRoleAsync(It.IsAny<User>(), "users"), Times.Once);
        signInManagerFake
            .Verify(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
    }

    [TestMethod]
    public void RequestRegisterUser_CreateFail_ShouldErrors()
    {
        var expectedModel = new RegisterWebModel
        {
            SurName = "testSurName",
            FirstName = "test",
            Patronymic = "test",
            Email = "test@example.com",
            Birthday = new DateTime(2021, 1, 1),
            UserName = "expectedName",
            Password = "123",
            PasswordConfirm = "123",
        };
        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedErrorDescription = "TestDescription";
        var expErrors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Failed(expErrors)));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.RequestRegisterUser(expectedModel).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
        Assert
            .AreEqual(expectedErrorCode, errors.FirstOrDefault());
    }

    [TestMethod]
    public void PasswordSignInAsync_LoginCorrect_ShouldCorrectResult()
    {
        var expectedModel = new LoginWebModel 
        { 
            UserName = "Test", 
            Password = "123", 
            ReturnUrl = "Index" 
        };
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        string callbackUsername = null;
        signInManagerFake
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success))
            .Callback((string u, string p, bool b, bool b2) => { callbackUsername = u; });
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake.Object, journalContext);

        var result = service.PasswordSignInAsync(expectedModel).Result;

        Assert
            .IsTrue(result);
        Assert
            .AreEqual(expectedModel.UserName, callbackUsername);
    }

    [TestMethod]
    public void PasswordSignInAsync_LoginIncorrect_ShouldFalseResult()
    {
        var expectedModel = new LoginWebModel
        {
            UserName = "Test",
            Password = "123",
            ReturnUrl = "Index"
        };
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        signInManagerFake
            .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake.Object, journalContext);

        var result = service.PasswordSignInAsync(expectedModel).Result;

        Assert
            .IsFalse(result);
    }
}

