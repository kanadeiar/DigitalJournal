namespace DigitalJournal.Dal.Data;

public interface IIdentitySeedTestData
{
    /// <summary> Заполнение идентификационной базы данных тестовыми данными </summary>
    /// <param name="provider">Провайдер</param>
    /// <param name="configuration">Корфигурация</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task<IIdentitySeedTestData> SeedTestData(IServiceProvider provider, IConfiguration configuration);
}
