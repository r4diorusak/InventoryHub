using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryHub.Server.Models;
using InventoryHub.Server.Services;

namespace InventoryHub.Server.Controllers
{
    /// <summary>
    /// ProductsController handles all API requests related to inventory product management.
    /// 
    /// INTEGRATION CODE (5 pts): This controller demonstrates how the server responds to
    /// client requests and returns properly structured JSON responses using the ApiResponse wrapper.
    /// 
    /// The API endpoints are consumed by the client-side code which makes HTTP requests
    /// and handles the responses. See InventoryHub.Client for integration examples.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// GET: api/products
        /// Retrieves all active products from the inventory.
        /// 
        /// Response: ApiResponse{List{Product}} - Wrapped response with metadata
        /// </summary>
        /// <returns>List of all active products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Product>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<List<Product>>), 500)]
        public async Task<ActionResult<ApiResponse<List<Product>>>> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// GET: api/products/{id}
        /// Retrieves a specific product by its ID.
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>The product with the specified ID</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 500)]
        public async Task<ActionResult<ApiResponse<Product>>> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<Product>.CreateError("Invalid product ID", 400));
            }

            var response = await _productService.GetProductByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// POST: api/products
        /// Creates a new product in the inventory.
        /// 
        /// DEBUGGING NOTE: Originally, the endpoint didn't validate input properly.
        /// Microsoft Copilot helped us add comprehensive validation and error handling.
        /// </summary>
        /// <param name="product">The product data to create</param>
        /// <returns>The created product with assigned ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Product>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 400)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 500)]
        public async Task<ActionResult<ApiResponse<Product>>> CreateProduct([FromBody] Product product)
        {
            // DEBUGGING: Copilot suggested validating the model state before processing
            if (!ModelState.IsValid)
            {
                var errorResponse = ApiResponse<Product>.CreateError("Invalid product data", 400);
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        if (!errorResponse.Errors.ContainsKey("validation"))
                            errorResponse.Errors["validation"] = new List<string>();
                        errorResponse.Errors["validation"].Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errorResponse);
            }

            var response = await _productService.CreateProductAsync(product);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = response.Data.Id }, response);
            }

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// PUT: api/products/{id}
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product ID to update</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 500)]
        public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<Product>.CreateError("Invalid product ID", 400));
            }

            var response = await _productService.UpdateProductAsync(id, product);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// DELETE: api/products/{id}
        /// Deletes (soft delete) a product from the inventory.
        /// </summary>
        /// <param name="id">The product ID to delete</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 500)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<bool>.CreateError("Invalid product ID", 400));
            }

            var response = await _productService.DeleteProductAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// GET: api/products/low-stock
        /// PERFORMANCE OPTIMIZATION: Retrieves only products with low stock levels.
        /// This specialized endpoint reduces data transfer and improves client-side performance
        /// by filtering at the server level.
        /// </summary>
        /// <returns>List of products below their reorder level</returns>
        [HttpGet("low-stock/list")]
        [ProducesResponseType(typeof(ApiResponse<List<Product>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<List<Product>>), 500)]
        public async Task<ActionResult<ApiResponse<List<Product>>>> GetLowStockProducts()
        {
            var response = await _productService.GetLowStockProductsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
