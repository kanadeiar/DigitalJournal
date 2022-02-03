namespace DigitalJournalTests.Services;

[TestClass]
public class AccountServiceTests
{
    readonly Random random = new Random();

    #region Получение данных

    [TestMethod]
    public void GetIndexWebModel_GetEmptyUserName_ShouldModel()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake, journalContext);

        var webModel = service.GetIndexWebModel(string.Empty).Result;

        Assert
            .IsInstanceOfType(webModel, typeof(UserIndexWebModel));
        var model = (UserIndexWebModel)webModel;
        Assert
            .AreEqual(null, model.User.UserName);
        Assert
            .AreEqual(string.Empty, model.Profile.SurName);
    }

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
            .Returns(Task.FromResult((IList<string>)expectedRoleQuery.Select(x => x.Name).ToList()));
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
            .IsInstanceOfType(webModel, typeof(UserIndexWebModel));
        var model = (UserIndexWebModel)webModel;
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

    #endregion

    #region Регистрация

    [TestMethod]
    public void RequestRegisterUser_CreateSuccess_ShouldSuccess()
    {
        var expectedModel = new UserRegisterWebModel
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
        Assert
            .AreEqual(1, journalContext.Profiles.Count());
        Assert
            .AreEqual(expectedModel.SurName, journalContext.Profiles.FirstOrDefault().SurName);
        userManagerFake
            .Verify(_ => _.AddToRoleAsync(It.IsAny<User>(), "users"), Times.Once);
        signInManagerFake
            .Verify(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
    }

    [TestMethod]
    public void RequestRegisterUser_CreateFail_ShouldErrors()
    {
        var expectedModel = new UserRegisterWebModel
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

    #endregion

    #region Вход

    [TestMethod]
    public void LoginPasswordSignInAsync_LoginCorrect_ShouldCorrectResult()
    {
        var expectedModel = new UserLoginWebModel
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

        var result = service.LoginPasswordSignIn(expectedModel).Result;

        Assert
            .IsTrue(result);
        Assert
            .AreEqual(expectedModel.UserName, callbackUsername);
    }

    [TestMethod]
    public void LoginPasswordSignInAsync_LoginIncorrect_ShouldFalseResult()
    {
        var expectedModel = new UserLoginWebModel
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

        var result = service.LoginPasswordSignIn(expectedModel).Result;

        Assert
            .IsFalse(result);
    }

    #endregion

    #region Редактирование профиля

    [TestMethod]
    public void GetEditModelByName_NullName_ShouldFalse()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake, journalContext);

        var (result, model) = service.GetEditModelByName(null).Result;

        Assert
            .IsFalse(result);
        Assert
            .IsNull(model);
    }

    [TestMethod]
    public void GetEditModelByName_NoneUser_ShouldFalse()
    {
        var userManagerFake = new Mock<UserManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()));
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, model) = service.GetEditModelByName("noneuser").Result;

        Assert
            .IsFalse(result);
        Assert
            .IsNull(model);
    }

    [TestMethod]
    public void GetEditModelByName_NoneProfile_ShouldFalse()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, model) = service.GetEditModelByName("noneprofile").Result;

        Assert
            .IsFalse(result);
        Assert
            .IsNull(model);
    }

    [TestMethod]
    public void GetEditMoselByName_CorrectRequest_ShouldCorrect()
    {
        var expectedUser = new User
        {
            UserName = "testName",
            Email = "test@example.com",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(expectedUser.UserName))
            .Returns(Task.FromResult(expectedUser));
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var expectedProfile = new Profile
        {
            SurName = "surname",
            FirstName = "test",
            Patronymic = "test",
            UserId = expectedUser.Id,
        };
        journalContext.Profiles.Add(expectedProfile);
        journalContext.SaveChanges();
        expectedUser.ProfileId = expectedProfile.Id;
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, model) = service.GetEditModelByName(expectedUser.UserName).Result;

        Assert
            .IsTrue(result);
        Assert
            .IsInstanceOfType(model, typeof(UserEditWebModel));
        Assert
            .AreEqual(expectedProfile.SurName, model.SurName);
        Assert
            .AreEqual(expectedUser.Email, model.Email);
    }

    [TestMethod]
    public void RequestUpdateUserProfile_NullName_ShouldFalse()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.RequestUpdateUserProfile(null, new UserEditWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
    }

    [TestMethod]
    public void RequestUpdateUserProfile_NoneUser_ShouldFalse()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.RequestUpdateUserProfile("noneuser", new UserEditWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
    }

    [TestMethod]
    public void RequestUpdateUserProfile_NoneProfile_ShouldFalse()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.RequestUpdateUserProfile("noneprofile", new UserEditWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
    }

    [TestMethod]
    public void RequestUpdateUserProfile_CorrectRequest_ShouldCorrect()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var expectedModel = new UserEditWebModel
        {
            SurName = "testNewSurName",
            FirstName = "test",
            Patronymic = "test",
            Email = "test@example.com",
            Birthday = new DateTime(2021, 1, 1),
        };
        var userManagerFake = new Mock<UserManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        User callbackUser = null;
        userManagerFake
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((User u) => { callbackUser = u; });
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var expectedProfile = new Profile
        {
            SurName = "surname",
            FirstName = "test",
            Patronymic = "test",
            UserId = expectedUser.Id,
        };
        journalContext.Profiles.Add(expectedProfile);
        journalContext.SaveChanges();
        expectedUser.ProfileId = expectedProfile.Id;
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.RequestUpdateUserProfile(expectedUser.UserName, expectedModel).Result;

        Assert
            .IsTrue(result);
        Assert
            .AreEqual(0, errors.Count());
        Assert
            .AreEqual(1, journalContext.Profiles.Count());
        Assert
            .AreEqual(expectedModel.SurName, journalContext.Profiles.FirstOrDefault().SurName);
        userManagerFake
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
    }

    #endregion

    #region Смена пароля

    [TestMethod]
    public void CheckAndChangePassword_NullName_ShouldFalse()
    {
        var userManagerFake = Mock.Of<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.CheckAndChangePassword(null, new UserPasswordWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
    }

    [TestMethod]
    public void CheckAndChangePassword_NoneUser_ShouldFalse()
    {
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = Mock.Of<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake, journalContext);

        var (result, errors) = service.CheckAndChangePassword("noneuser", new UserPasswordWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
        Assert
            .AreEqual("Не удалось найти сущность в базе данных", errors.First());
    }

    [TestMethod]
    public void CheckAndChangePassword_ErrorPassword_ShouldFalse()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        signInManagerFake
            .Setup(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake.Object, journalContext);

        var (result, errors) = service.CheckAndChangePassword("nonepassword", new UserPasswordWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
        Assert
            .AreEqual("Неправильный старый пароль", errors.First());
    }

    [TestMethod]
    public void CheckAndChangePassword_ErrorRemovePassword_ShouldFalse()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var expectedErrorCode = "DefaultError";
        var expectedErrorDescription = "TestDescription";
        var expErrors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        signInManagerFake
            .Setup(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
        userManagerFake
            .Setup(_ => _.RemovePasswordAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Failed(expErrors)));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake.Object, journalContext);

        var (result, errors) = service.CheckAndChangePassword("errorpassword", new UserPasswordWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
        Assert
            .AreEqual("Произошла неизвестная ошибка", errors.First());
    }

    [TestMethod]
    public void CheckAndChangePassword_ErrorAddPassword_ShouldFalse()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var expectedErrorCode = "DefaultError";
        var expectedErrorDescription = "TestDescription";
        var expErrors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        signInManagerFake
            .Setup(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
        userManagerFake
            .Setup(_ => _.RemovePasswordAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        userManagerFake
            .Setup(_ => _.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Failed(expErrors)));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake.Object, journalContext);

        var (result, errors) = service.CheckAndChangePassword("errorpassword", new UserPasswordWebModel()).Result;

        Assert
            .IsFalse(result);
        Assert
            .AreEqual(1, errors.Count());
        Assert
            .AreEqual("Произошла неизвестная ошибка", errors.First());
    }

    [TestMethod]
    public void CheckAndChangePassword_CorrectRequest_ShouldCorrect()
    {
        var expectedUser = new User
        {
            UserName = "testName",
        };
        var expectedErrorCode = "DefaultError";
        var expectedErrorDescription = "TestDescription";
        var expErrors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        var expectedModel = new UserPasswordWebModel
        {
            OldPassword = "old",
            Password = "new",
            PasswordConfirm = "new",
        };
        var userManagerFake = new Mock<UserManagerMock>();
        var roleManagerFake = Mock.Of<RoleManagerMock>();
        var signInManagerFake = new Mock<SignInManagerMock>();
        userManagerFake
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        signInManagerFake
            .Setup(_ => _.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
        userManagerFake
            .Setup(_ => _.RemovePasswordAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Success));
        User callbackUser = null;
        userManagerFake
            .Setup(_ => _.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((User u, string p) => { callbackUser = u; });
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(random.Next(int.MaxValue).ToString()).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        IAccountService service = new AccountService(userManagerFake.Object, roleManagerFake, signInManagerFake.Object, journalContext);

        var (result, errors) = service.CheckAndChangePassword(expectedUser.UserName, expectedModel).Result;

        Assert
            .IsTrue(result);
        Assert
            .AreEqual(0, errors.Count());
        Assert
            .AreEqual(expectedUser.UserName, callbackUser.UserName);
        userManagerFake
            .Verify(_ => _.AddPasswordAsync(It.IsAny<User>(), expectedModel.Password));
    }

    #endregion
}

