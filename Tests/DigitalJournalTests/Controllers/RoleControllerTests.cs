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
    }

    #endregion

    #region Создание и редактирование ролей

    [TestMethod]
    public void Create_SendRequest_ShouldView()
    {
        var userManagerStub = Mock.Of<UserManagerMock>();
        var roleManagerStub = Mock.Of<RoleManagerMock>();
        var journalContextOptions = new DbContextOptionsBuilder<DigitalJournalContext>()
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoUsersRequest_ShouldView)).Options;
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
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoUsersRequest_ShouldView)).Options;
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
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoUsersRequest_ShouldView)).Options;
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
            .UseInMemoryDatabase(nameof(Index_SendCorrectNoUsersRequest_ShouldView)).Options;
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

