using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcDatabaseApp.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [DisplayName("Product Name")]
        [Required(ErrorMessage="Name must be present")]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        public string Price {  get; set; }

        [DisplayName("Product Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayName("Active")]
        public bool isActive { get; set; }
        [DisplayName("Product Category")]
        [StringLength(50)]
        public string Category {  get; set; }
    }
}
