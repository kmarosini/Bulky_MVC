using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_temp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Category Order")]
        [Range(1, 100, ErrorMessage = "Number must be in range of 1 - 100.")]
        public int DisplayOrder { get; set; }
    }
}
