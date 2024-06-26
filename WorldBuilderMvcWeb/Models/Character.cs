using System.ComponentModel.DataAnnotations;

namespace WorldBuilderWeb.Models
{
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Race { get; set; }

        [MaxLength(50)]
        public string Class { get; set; }

        public int Level { get; set; }

        [MaxLength(100)]
        public string Background { get; set; }

        [MaxLength(500)]
        public string DmNotes { get; set; }
    }
}
