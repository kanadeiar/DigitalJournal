namespace DigitalJournal.Services.Interfaces;

public interface IAccountService
{
    /// <summary> Получить данные по аккаунту </summary>
    /// <returns>Данные аккаунта</returns>
    public Task<IndexWebModel> GetIndexWebModel(string userName);
}

/// <summary> Вебмодель сведения о пользователе </summary>
public class IndexWebModel
{
    /// <summary> Сведения о профиле пользователя </summary>
    public Profile? Profile { get; set; }
    /// <summary> Сведения о пользовате </summary>
    public User User { get; set; }
    /// <summary> Роли пользователя </summary>
    public IEnumerable<string> UserRoleNames { get; set; } = Enumerable.Empty<string>();
}
