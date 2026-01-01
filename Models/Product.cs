using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcDatabaseApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Name is Required")]
        [StringLength(100,MinimumLength =3,ErrorMessage ="The name should have atleast 3 characters with less than 100 characters")]
        public required string Name { get; set; }
        [Required(ErrorMessage ="Price is required")]
        [Column(TypeName="decimal(18,2)")]
        [Range(0.01,1000,ErrorMessage ="The Price should range from 0.01 to 1000 only!")]
        public decimal Price { get; set; }

        [StringLength(500,MinimumLength =3,ErrorMessage ="Description can not exceed 500")]
        public string Description { get; set; }
        [Required]
        public DateTime CreateDate {  get; set; }= DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        public bool isActive { get; set; } =true;
        [StringLength(50)]
        public string Category { get; set; }
    }
}
