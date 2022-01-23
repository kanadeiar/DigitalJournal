using DigitalJournal.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DigitalJournal.Domain.Entities.Factory1
{
    public class Factory1Shift : Entity
    {
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название смены обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название смены должно быть от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }
    }
}
