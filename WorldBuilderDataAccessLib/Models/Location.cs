using System.ComponentModel.DataAnnotations;

namespace WorldBuilderDataAccessLib.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Region { get; set; }

        // Add other properties as needed
    }
}
