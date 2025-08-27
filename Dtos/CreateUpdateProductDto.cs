using System.ComponentModel.DataAnnotations;

namespace ProductInventory.Api.Dtos
{
    public class CreateUpdateProductDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;


        [Range(0, 1000000)]
        public decimal Price { get; set; }


        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        [Required]
        public int CategoryId { get; set; }
    }
}
