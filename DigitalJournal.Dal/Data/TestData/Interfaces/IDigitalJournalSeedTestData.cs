namespace DigitalJournal.Dal.Data;

public interface IDigitalJournalSeedTestData
{
    /// <summary> Заполнение базы данных журнала тестовыми данными </summary>
    /// <param name="provider">Провайдер</param>
    /// <param name="configuration">Корфигуратор</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Task<IDigitalJournalSeedTestData> SeedTestData(IServiceProvider provider, IConfiguration configuration);
}
