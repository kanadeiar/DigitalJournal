namespace DigitalJournalTests.Controllers;

[TestClass]
public class UserControllerTests
{
    #region Просмотр пользователей

    [TestMethod]
    public void Index_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedName = "test";
        var expectedRoleNale = "roleName";
        var expectedSurName = "TestSurName";
        var expectedDescription = "description";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        var users = Enumerable.Range(1, 1)
            .Select(r => user).AsQueryable();
        userManagerMock
            .Setup(_ => _.Users)
            .Returns(users);
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>) new List<string> { expectedRoleNale }));
        var roleManagerMock = new Mock<RoleManagerMock>();
        var roles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedRoleNale, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(roles);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var controller = new UserController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IEnumerable<UserController.UserWebModel>));
        var userModels = (IEnumerable<UserController.UserWebModel>)view.Model;
        Assert
            .AreEqual(1, userModels.Count());
        Assert
            .AreEqual(expectedSurName, userModels.First().SurName);
        Assert
            .AreEqual(1, userModels.First().RolesNames.Count());
        Assert
            .AreEqual(expectedDescription, userModels.First().RolesNames.First());
        userManagerMock
            .Verify(_ => _.Users, Times.Once);
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()), Times.Once);
        userManagerMock
            .Verify();
        roleManagerMock
            .Verify(_ => _.Roles, Times.Once);
        roleManagerMock
            .Verify();
    }

    [TestMethod]
    public void Index_SendCorrectNoRolesRequest_ShouldCorrectView()
    {
        var expectedName = "test";
        var expectedRoleNale = "roleName";
        var expectedSurName = "TestSurName";
        var expectedDescription = "description";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        var users = Enumerable.Range(1, 1)
            .Select(r => user).AsQueryable();
        userManagerMock
            .Setup(_ => _.Users)
            .Returns(users);
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>)new List<string> ()));
        var roleManagerMock = new Mock<RoleManagerMock>();
        var roles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedRoleNale, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(roles);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoRolesRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var controller = new UserController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IEnumerable<UserController.UserWebModel>));
        var userModels = (IEnumerable<UserController.UserWebModel>)view.Model;
        Assert
            .AreEqual(1, userModels.Count());
        Assert
            .AreEqual(expectedSurName, userModels.First().SurName);
        Assert
            .AreEqual(0, userModels.First().RolesNames.Count());
        userManagerMock
            .Verify(_ => _.Users, Times.Once);
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()), Times.Once);
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Trashes_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedName = "test";
        var expectedSurName = "TestSurName";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
            IsDelete = true,
        };
        var users = Enumerable.Range(1, 1)
            .Select(r => user).AsQueryable();
        userManagerMock
            .Setup(_ => _.Users)
            .Returns(users);
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Trashes_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Trashes().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IEnumerable<UserController.UserWebModel>));
        var userModels = (IEnumerable<UserController.UserWebModel>)view.Model;
        Assert
            .AreEqual(1, userModels.Count());
        Assert
            .AreEqual(expectedSurName, userModels.First().SurName);
        Assert
            .AreEqual(0, userModels.First().RolesNames.Count());
        userManagerMock
            .Verify(_ => _.Users, Times.Once);
        userManagerMock
            .Verify();
    }

    #endregion

    #region Созрание и редактирование пользователей

    [TestMethod]
    public void Create_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedName = "test";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var roles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedName, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(roles);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Create_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Create();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserEditWebModel));
        var userModels = (UserController.UserEditWebModel)view.Model;
        roleManagerMock
            .Verify(_ => _.Roles, Times.Once);
        roleManagerMock
            .Verify();
        Assert
            .AreEqual(1, userModels.AllRoles.Count());
    }

    [TestMethod]
    public void Edit_SendNullIdRequest_ShouldView()
    {
        var expectedName = "test";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNullIdRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Edit(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserEditWebModel));
    }

    [TestMethod]
    public void Edit_SendCorrectRequest_ShouldCorrectRequest()
    {
        var expectedName = "TestName";
        var expectedSurName = "TestSurName";
        var expectedDescription = "description";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        var roles = new List<string> { expectedName };
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>)roles));
        var roleManagerMock = new Mock<RoleManagerMock>();
        var objRoles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedName, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(objRoles);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendCorrectRequest_ShouldCorrectRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var controller = new UserController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Edit(user.Id).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserEditWebModel));
        var userModel = (UserController.UserEditWebModel)view.Model;
        Assert
            .AreEqual(expectedSurName, userModel.SurName);
        Assert
            .AreEqual(1, userModel.AllRoles.Count());
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once);
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()));
        userManagerMock
            .Verify();
        roleManagerMock
            .Verify(_ => _.Roles);
        roleManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendNotFoundRequest_ShouldNotFound()
    {
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Edit("notcontains").Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundResult));
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostInvalidModelRequest_ShouldView()
    {
        var expectedName = "name";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostInvalidModelRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new UserController.UserEditWebModel
        {
            UserName = expectedName,
        };
        var roles = new List<string>();
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Edit(model, roles).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserEditWebModel));
        var userModel = (UserController.UserEditWebModel)view.Model;
        Assert
            .AreEqual(expectedName, userModel.UserName);
    }

    [TestMethod]
    public void Edit_SendPostNewUserNonePassword_ShouldView()
    {
        var expectedName = "name";
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostNewUserNonePassword_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new UserController.UserEditWebModel
        {
            UserName = expectedName,
        };
        var roles = new List<string>();
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Edit(model, roles).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState["Password"].Errors;
        Assert
            .AreEqual("Нужно обязательно ввести новый пароль пользователя", returnErrors.FirstOrDefault().ErrorMessage);
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserEditWebModel));
        var userModel = (UserController.UserEditWebModel)view.Model;
        Assert
            .AreEqual(expectedName, userModel.UserName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once);
        userManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostNewUserCorrect_ShouldRedirect()
    {
        var expectedName = "name";
        var expectedSurName = "testSurname";
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        User callbackUser = null;
        userManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((User u, string s) => { callbackUser = u; });
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()));
        userManagerMock
            .Setup(_ => _.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostNewUserCorrect_ShouldRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new UserController.UserEditWebModel
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            Birthday = new DateTime(2021, 1, 1),
            UserName = expectedName,
            Email = "test@example.com",
            Password = "123",
        };
        var roles = new List<string>();
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Edit(model, roles).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("User", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        userManagerMock
            .Verify(_ => _.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        userManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackUser);
        Assert
            .AreEqual(expectedName, callbackUser.UserName);
    }

    [TestMethod]
    public void Edit_SendPostOldUserCorrect_ShouldRedirect()
    {
        var expectedName = "name";
        var expectedSurName = "testSurname";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        User callbackUser = null;
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((User u) => { callbackUser = u; });
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>) new List<string>()));
        userManagerMock
            .Setup(_ => _.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        userManagerMock
            .Setup(_ => _.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostOldUserCorrect_ShouldRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;

        var model = new UserController.UserEditWebModel
        {
            SurName = expectedSurName,
            FirstName = "test",
            Patronymic = "test",
            Birthday = new DateTime(2021, 1, 1),
            UserName = expectedName,
            Email = "test@example.com",
            Password = "123",
        };
        var roles = new List<string>();
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Edit(model, roles).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("User", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()));
        userManagerMock
            .Verify(_ => _.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        userManagerMock
            .Verify(_ => _.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()));
        userManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackUser);
        Assert
            .AreEqual(expectedName, callbackUser.UserName);
    }

    [TestMethod]
    public void Totrash_SendNullIdRequest_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Totrash_SendNullIdRequest_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Totrash(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Totrash_SendCorrectRequest_ShouldRedirect()
    {
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            IsDelete = false,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        User callbackUser = null;
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Callback((User u) => { callbackUser = u; });
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Totrash_SendNullIdRequest_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Totrash("id").Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = (RedirectToActionResult)result;
        Assert
            .AreEqual("User", redirect.ControllerName);
        Assert
            .AreEqual("Index", redirect.ActionName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        Assert
            .IsNotNull(callbackUser);
        Assert
            .IsTrue(callbackUser.IsDelete);
    }

    [TestMethod]
    public void Fromtrash_SendNullIdRequest_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Fromtrash_SendNullIdRequest_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Fromtrash(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Fromtrash_SendCorrectRequest_ShouldRedirect()
    {
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            IsDelete = true,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        User callbackUser = null;
        userManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<User>()))
            .Callback((User u) => { callbackUser = u; });
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Fromtrash_SendCorrectRequest_ShouldRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Fromtrash("id").Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = (RedirectToActionResult)result;
        Assert
            .AreEqual("User", redirect.ControllerName);
        Assert
            .AreEqual("Trashes", redirect.ActionName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<User>()));
        Assert
            .IsNotNull(callbackUser);
        Assert
            .IsFalse(callbackUser.IsDelete);
    }

    #endregion

    #region Удаление пользователей

    [TestMethod]
    public void Delete_SendEmptyId_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Delete_SendEmptyId_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Delete(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_SendNotFoundRequest_ShouldNotFound()
    {
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Delete_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.Delete("notfound").Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void Delete_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedUserName = "username";
        var expectedSurName = "testSurName";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedUserName,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        userManagerMock
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>)new List<string>()));
        var roleManagerMock = new Mock<RoleManagerMock>();
        IQueryable<Role> roles = Enumerable.Range(1, 1)
            .Select(i => new Role { }).AsQueryable();
        roleManagerMock.Setup(_ => _.Roles)
            .Returns(roles);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Delete_SendCorrectRequest_ShouldCorrectView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var profile = new Profile
        {
            SurName = expectedSurName,
            FirstName = "Test",
            Patronymic = "Test",
            UserId = user.Id,
        };
        journalContext.Profiles.Add(profile);
        journalContext.SaveChanges();
        user.ProfileId = profile.Id;
        var controller = new UserController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Delete(user.Id).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(UserController.UserWebModel));
        var userModel = (UserController.UserWebModel)view.Model;
        Assert
            .AreEqual(expectedSurName, userModel.SurName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.GetRolesAsync(It.IsAny<User>()));
        userManagerMock
            .Verify();
        roleManagerMock
            .Verify(_ => _.Roles, Times.Never);
        roleManagerMock
            .Verify();
    }

    [TestMethod]
    public void DeleteConfirmed_SendEmptyId_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendEmptyId_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.DeleteConfirmed(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void DeleteConfirmed_SendAdminRequest_ShouldBadRequest()
    {
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = "admin",
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendAdminRequest_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.DeleteConfirmed("admin").Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void DeleteConfirmed_SendCorrectRequest_ShouldCorrectRedirect()
    {
        var expectedName = "testName";
        var userManagerMock = new Mock<UserManagerMock>();
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(user));
        User callbackUser = null;
        userManagerMock
            .Setup(_ => _.DeleteAsync(It.IsAny<User>()))
            .Callback((User u) => { callbackUser = u; });
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendCorrectRequest_ShouldCorrectRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.DeleteConfirmed("userId").Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirect = (RedirectToActionResult)result;
        Assert
            .AreEqual("User", redirect.ControllerName);
        Assert
            .AreEqual("Trashes", redirect.ActionName);
        userManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once);
        userManagerMock
            .Verify(_ => _.DeleteAsync(It.IsAny<User>()), Times.Once);
        userManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackUser);
        Assert
            .AreEqual(expectedName, callbackUser.UserName);
    }

    #endregion

    #region WebAPI

    [TestMethod]
    public void IsNameFree_SendCorrectRequest_ShouldCorrectJson()
    {
        var expectedName = "TestName";
        var userManagerMock = new Mock<UserManagerMock>();
        userManagerMock
            .Setup(_ => _.FindByNameAsync(It.IsAny<string>()));
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(IsNameFree_SendCorrectRequest_ShouldCorrectJson)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new UserController(userManagerMock.Object, roleManagerStub, journalContext);

        var result = controller.IsNameFree(expectedName, string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(JsonResult));
        var json = (JsonResult)result;
        Assert
            .AreEqual("true", json.Value);
        userManagerMock
            .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
        userManagerMock
            .Verify();
    }

    #endregion

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

    #endregion
}

