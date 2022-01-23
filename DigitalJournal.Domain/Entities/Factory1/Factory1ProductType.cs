using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Вид товара </summary>
    public class Factory1ProductType : Entity
    {
        /// <summary> Название пачки товара, еденица </summary>
        [Required(ErrorMessage = "Название вида товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название вида товаров должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Номер пачки товаров </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер пачки продукта должен быть положительным числом")]
        public int Number { get; set; }

        /// <summary> Количество неделимых едениц в одной пачке </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество неделимых едениц в пачке продукта должно быть положительным числом")]
        public int Units { get; set; }

        /// <summary> Метка удаления вида упаковки продукта </summary>
        public bool IsDelete { get; set; }
    }
}
