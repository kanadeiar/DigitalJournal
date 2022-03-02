using DigitalJournal.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Documents
{
    public class DocDocument : Entity
    {
        [Required(ErrorMessage = "Дата документа обязательна для документа")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-18);

        [Required(ErrorMessage = "Название документа обязательно нужно ввести")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название документа должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Описание документа должно быть длинной до 200 символов")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "Отметки документа должно быть длинной до 200 символов")]
        public string? Marks { get; set; }

        [StringLength(200, ErrorMessage = "Примечание документа должно быть длинной до 200 символов")]
        public string? Note { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран каталог документа")]
        public int DirectoryId { get; set; }
        [ForeignKey(nameof(DirectoryId))]
        public DocDirectory Directory { get; set; }

        public virtual IEnumerable<DocComment> Comments { get; set; } = new List<DocComment>();
    }
}
