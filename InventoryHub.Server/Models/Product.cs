using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Server.Models
{
    /// <summary>
    /// Represents a product in the inventory system.
    /// This model is used for JSON serialization/deserialization between client and server.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the product (required field)
        /// </summary>
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Unit price of the product
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// Current quantity in stock
        /// </summary>
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Reorder level - when stock falls below this, reordering is recommended
        /// </summary>
        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }

        /// <summary>
        /// Category of the product
        /// </summary>
        [StringLength(50)]
        public string Category { get; set; }

        /// <summary>
        /// Date when the product was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the product was last updated
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }

        /// <summary>
        /// Determines if the product is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
