using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.DataAccess.Models
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
        [Range(1,100, ErrorMessage ="Number must be in range of 1 - 100.")]
        public int DisplayOrder { get; set; }
    }
}
