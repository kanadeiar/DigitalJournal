namespace DigitalJournalTests.Common;

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

