namespace Client.Models
{
    public class ProductEntryDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime EntryDateTime { get; set; }
        public string ProductModel { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}