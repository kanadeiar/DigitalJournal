using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Данные по прессу 1 за смену </summary>
    public class Factory1Press1ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Вид товара </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара")]
        public int Factory1ProductTypeId { get; set; }
        /// <summary> Вид товара </summary>
        [ForeignKey(nameof(Factory1ProductTypeId))]
        public Factory1ProductType Factory1ProductType { get; set; }
        /// <summary> Количество изготовленного товара </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество вида товаров должно быть положительным числом")]
        public int ProductCount { get; set; }

        /// <summary> Использовано сырья - песка </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Количество использованного сырья песка должно быть положительным числом")]
        public double Loose1RawValue { get; set; }

        /// <summary> Использовано сырья - цемента </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Количество использованного сырья цемента должно быть положительным числом")]
        public double Loose2RawValue { get; set; }

        /// <summary> Использовано сырья - извести </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Количество использованного сырья извести должно быть положительным числом")]
        public double Loose3RawValue { get; set; }

        /// <summary> Смена </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Factory1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Factory1ShiftId))]
        public Factory1Shift Factory1Shift { get; set; }

        /// <summary> Пресовщик </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран пресовщик")]
        public int ProfileId { get; set; }
        /// <summary> Пресовщик </summary>
        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; }
    }
}
