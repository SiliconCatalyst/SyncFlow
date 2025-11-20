using System.ComponentModel.DataAnnotations;

namespace Server.Dtos
{
    public class ProductEntryCreateDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string ProductModel { get; set; } =
        string.Empty;

        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}