namespace DigitalJournal.Domain.Interfaces
{
    public interface IMapper
    {
        /// <summary> Преобразование типа </summary>
        /// <typeparam name="TIn">Исходный</typeparam>
        /// <typeparam name="TOut">Требуемый</typeparam>
        /// <param name="source">Источник</param>
        /// <returns>Результат</returns>
        TOut Map<TIn, TOut>(TIn source) where TIn : class where TOut : class, new();
    }
}
