namespace DigitalJournal.Services.Interfaces;

/// <summary> Сервис получения данных для построения графиков </summary>
public interface IHomeInfoService
{
    /// <summary> Общие данные по прибылям по цехам </summary>
    /// <returns>Данные для графиков</returns>
    Task<IList<SimpleReportWebModel>> GetMoneyInfoForFactory1();
    /// <summary> Получение данных по товарам на прессу 1 </summary>
    /// <returns>Данные для графиков</returns>
    Task<IList<SimpleReportWebModel>> GetDataForPress1ForProducts();
    /// <summary> Получить данные по скоростям упаковки завода №1 за последние десять смен </summary>
    /// <returns>Данные</returns>
    Task<IList<SimpleReportWebModel>> GetSpeedPackOfFacttory1();
    /// <summary> Получить данные по качеству продукции </summary>
    /// <returns>Данные</returns>
    public Task<IList<StackedWebModel>> GetQualityDataOfFactory1();
}
/// <summary> Веб модель данных для графиков </summary>
public class SimpleReportWebModel
{
    public string DimensionOne { get; set; }
    public int Quantity { get; set; }
}
/// <summary> Веб модель обобщения данных для графиков </summary>
public class StackedWebModel
{
    public string StackedDimensionOne { get; set; }
    public List<SimpleReportWebModel> LstData { get; set; }
}