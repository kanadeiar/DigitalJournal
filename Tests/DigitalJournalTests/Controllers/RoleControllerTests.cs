namespace DigitalJournalTests.Controllers;

[TestClass]
public class RoleControllerTests
{
    #region Просмотр ролей

    [TestMethod]
    public void Index_SendCorrectRequest_ShouldView()
    {
        var expectedName = "test";
        var expectedSurName = "TestSurName";
        var expectedDescription = "description";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var roles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedName, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(roles);
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IList<User>) new List<User> { user }));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectRequest_ShouldView)).Options;
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
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IEnumerable<RoleController.RoleWebModel>));
        var roleModels = (IEnumerable<RoleController.RoleWebModel>)view.Model;
        Assert
            .AreEqual(1, roleModels.Count());
        Assert
            .AreEqual(expectedName, roleModels.First().Name);
        Assert
            .AreEqual(1, roleModels.First().UsersCount);
        Assert
            .AreEqual(expectedDescription, roleModels.First().Description);
        roleManagerMock
            .Verify(_ => _.Roles, Times.Once);
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public void Index_SendCorrectNoUsersRequest_ShouldView()
    {
        var expectedName = "test";
        var expectedDescription = "description";
        var expectedUserNames = "Нет пользователей";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var roles = Enumerable.Range(1, 1)
            .Select(r => new Role { Name = expectedName, Description = expectedDescription }).AsQueryable();
        roleManagerMock
            .Setup(_ => _.Roles)
            .Returns(roles);
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IList<User>)Enumerable.Empty<User>().ToList()));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoUsersRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Index().Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(IEnumerable<RoleController.RoleWebModel>));
        var roleModels = (IEnumerable<RoleController.RoleWebModel>)view.Model;
        Assert
            .AreEqual(1, roleModels.Count());
        Assert
            .AreEqual(expectedName, roleModels.First().Name);
        Assert
            .AreEqual(0, roleModels.First().UsersCount);
        Assert
            .AreEqual(expectedUserNames, roleModels.First().UsersNames);
        Assert
            .AreEqual(expectedDescription, roleModels.First().Description);
        roleManagerMock
            .Verify(_ => _.Roles, Times.Once);
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()), Times.Once);
    }

    #endregion

    #region Создание и редактирование ролей

    [TestMethod]
    public void Create_SendRequest_ShouldView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Create_SendRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Create();

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(RoleController.RoleEditWebModel));
    }

    [TestMethod]
    public void Edit_SendNullIdRequest_ShouldView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNullIdRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Edit(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(RoleController.RoleEditWebModel));
    }

    [TestMethod]
    public void Edit_SendCorrectRequest_ShouldView()
    {
        var expectedName = "name";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendCorrectRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Edit(role.Id).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(RoleController.RoleEditWebModel));
        var model = (RoleController.RoleEditWebModel)view.Model;
        Assert.AreEqual(expectedName, model.Name);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        roleManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendNotFoundRequest_ShouldNotFound()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Edit("notcontains").Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundResult));
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        roleManagerMock
            .Verify();
    }

    [TestMethod]
    public void Edit_SendPostInvalidModelRequest_ShouldView()
    {
        var expectedName = "name";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostInvalidModelRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new RoleController.RoleEditWebModel
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        var controller = new RoleController(userManagerStub, roleManagerStub, journalContext);
        controller.ModelState.AddModelError("error", "InvalidError");

        var result = controller.Edit(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var viewResult = (ViewResult)result;
        Assert
            .IsInstanceOfType(viewResult.Model, typeof(RoleController.RoleEditWebModel));
    }

    [TestMethod]
    public void Edit_SendPostNewRole_ShouldRedirect()
    {
        var expectedName = "name";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        Role callbackRole = null;
        roleManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((Role r) => { callbackRole = r; });
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostNewRole_ShouldRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new RoleController.RoleEditWebModel
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Edit(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Role", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        roleManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<Role>()));
        roleManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackRole);
        Assert
            .AreEqual(expectedName, callbackRole.Name);
    }

    [TestMethod]
    public void Edit_SendPostOldRole_ShouldResirect()
    {
        var oldName = "old";
        var oldDescription = "old";
        var expectedName = "name";
        var expectedDescription = "description";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role
        {
            Name = oldName,
            Description = oldDescription,
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        Role callbackRole = null;
        roleManagerMock
            .Setup(_ => _.UpdateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult(IdentityResult.Success))
            .Callback((Role r) => { callbackRole = r; });
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostOldRole_ShouldResirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new RoleController.RoleEditWebModel
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Edit(model).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Role", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once);
        roleManagerMock
            .Verify(_ => _.UpdateAsync(It.IsAny<Role>()), Times.Once);
        roleManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackRole);
        Assert
            .AreEqual(expectedName, callbackRole.Name);
        Assert
            .AreEqual(expectedName, role.Name);
    }

    [TestMethod]
    public void Edit_SendPostWithErrorRequest_ShouldView()
    {
        var expectedName = "name";
        var expectedDescription = "description";
        var expectedErrorCode = "Произошла неизвестная ошибка";
        var expectedErrorDescription = "TestDescription";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        Role callbackRole = null;
        var errors = new[]
        {
            new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
        };
        roleManagerMock
            .Setup(_ => _.CreateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult(IdentityResult.Failed(errors)))
            .Callback((Role r) => { callbackRole = r; });
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Edit_SendPostWithErrorRequest_ShouldView)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var model = new RoleController.RoleEditWebModel
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Edit(model).Result;

        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert
            .IsFalse(controller.ModelState.IsValid);
        var returnErrors = controller.ModelState[string.Empty].Errors;
        Assert
            .AreEqual(expectedErrorDescription, returnErrors.FirstOrDefault().ErrorMessage);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        roleManagerMock
            .Verify(_ => _.CreateAsync(It.IsAny<Role>()));
        roleManagerMock
            .Verify();
        Assert
            .IsNotNull(callbackRole);
        Assert
            .AreEqual(expectedName, callbackRole.Name);
    }

    #endregion

    #region Удаление ролей

    [TestMethod]
    public void Delete_SendEmptyId_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Delete_SendEmptyId_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.Delete(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void Delete_SendNotFoundRequest_ShouldNotFound()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Delete_SendNotFoundRequest_ShouldNotFound)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.Delete("notfound").Result;

        Assert
            .IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public void Delete_SendCorrectRequest_ShouldCorrectView()
    {
        var expectedName = "test";
        var expectedSurName = "TestSurName";
        var expectedDescription = "description";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role
        {
            Name = expectedName,
            Description = expectedDescription,
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        var user = new User
        {
            UserName = expectedName,
        };
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((IList<User>)new List<User> { user }));
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
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.Delete(role.Id).Result;

        Assert
            .IsInstanceOfType(result, typeof(ViewResult));
        var view = (ViewResult)result;
        Assert
            .IsInstanceOfType(view.Model, typeof(RoleController.RoleWebModel));
        var roleModels = (RoleController.RoleWebModel)view.Model;
        Assert
            .AreEqual(expectedName, roleModels.Name);
        Assert
            .AreEqual(1, roleModels.UsersCount);
        Assert
            .AreEqual(expectedDescription, roleModels.Description);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()));
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()));
    }

    [TestMethod]
    public void DeleteConfirmed_SendEmptyId_ShouldBadRequest()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendEmptyId_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerStub, journalContext);

        var result = controller.DeleteConfirmed(string.Empty).Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
    }

    [TestMethod]
    public void DeleteConfirmed_SendAnyUsers_ShouldBadRequest()
    {
        var expectedName = "test";
        var expectedRoleName = "roletest";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role 
        { 
            Name = expectedRoleName
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        var user = new User 
        { 
            UserName = expectedName 
        };
        IList<User> users = new List<User> { user };
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(users));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendAnyUsers_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.DeleteConfirmed("anyusers").Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once());
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()));
    }

    [TestMethod]
    public void DeleteConfirmed_SendAdminsRequest_ShouldBadRequest()
    {
        var expectedRoleName = "admins";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role
        {
            Name = expectedRoleName
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        IList<User> users = new List<User> { };
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(users));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendAdminsRequest_ShouldBadRequest)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.DeleteConfirmed("adminsrole").Result;

        Assert
            .IsInstanceOfType(result, typeof(BadRequestResult));
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once());
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()));
    }

    [TestMethod]
    public void DeleteConfirmed_SendCorrectRequest_ShouldRedirect()
    {
        var expectedRoleName = "testName";
        var userManagerMock = new Mock<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        var role = new Role
        {
            Name = expectedRoleName
        };
        roleManagerMock
            .Setup(_ => _.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(role));
        IList<User> users = new List<User> { };
        userManagerMock
            .Setup(_ => _.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(users));
        Role callbackRole = null;
        roleManagerMock
            .Setup(_ => _.DeleteAsync(It.IsAny<Role>()))
            .Callback((Role r) => callbackRole = r);
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(DeleteConfirmed_SendCorrectRequest_ShouldRedirect)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerMock.Object, roleManagerMock.Object, journalContext);

        var result = controller.DeleteConfirmed(expectedRoleName).Result;

        Assert
            .IsInstanceOfType(result, typeof(RedirectToActionResult));
        var redirectResult = (RedirectToActionResult)result;
        Assert
            .AreEqual("Role", redirectResult.ControllerName);
        Assert
            .AreEqual("Index", redirectResult.ActionName);
        roleManagerMock
            .Verify(_ => _.FindByIdAsync(It.IsAny<string>()), Times.Once());
        userManagerMock
            .Verify(_ => _.GetUsersInRoleAsync(It.IsAny<string>()));
        Assert
            .IsNotNull(callbackRole);
        Assert
            .AreEqual(expectedRoleName, callbackRole.Name);
    }

    #endregion

    #region WebAPI

    [TestMethod]
    public void IsNameFree_SendRequest_ShouldCorrectJson()
    {
        var expectedName = "TestName";
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerMock = new Mock<RoleManagerMock>();
        roleManagerMock
            .Setup(_ => _.FindByNameAsync(expectedName));
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(IsNameFree_SendRequest_ShouldCorrectJson)).Options;
        using var journalContext = new DigitalJournalContext(journalContextOptions);
        var controller = new RoleController(userManagerStub, roleManagerMock.Object, journalContext);

        var result = controller.IsNameFree(expectedName, "").Result;

        Assert
            .IsInstanceOfType(result, typeof(JsonResult));
        var json = (JsonResult)result;
        Assert
            .AreEqual("true", json.Value);
        roleManagerMock
            .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
        roleManagerMock
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

