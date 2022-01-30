using DigitalJournal.Services;
using DigitalJournal.Services.Interfaces;
using DigitalJournalTests.Common;

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
        userManagerFake
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(expectedUser));
        userManagerFake
            .Setup(_ => _.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult((IList<string>) expectedRoleQuery.Select(x => x.Name).ToList()));
        var roleManagerFake = new Mock<RoleManagerMock>();
        roleManagerFake
            .Setup(_ => _.Roles)
            .Returns(expectedRoleQuery);
        var signInManagerFake = Mock.Of<SignInManagerMock>();
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
}

