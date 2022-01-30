namespace DigitalJournalTests.Common;

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

