namespace DigitalJournalTests.Common;

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

