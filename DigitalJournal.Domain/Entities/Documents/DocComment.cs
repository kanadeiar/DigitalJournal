using DigitalJournal.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Documents
{
    public class DocComment : Entity
    {
        [StringLength(200, ErrorMessage = "Комментарий документа должен быть длинной до 200 символов")]
        public string? Description { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран документ")]
        public int DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public DocDocument Document { get; set; }
    }
}
