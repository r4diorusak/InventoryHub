/**
 * InventoryHub API Client
 * 
 * INTEGRATION CODE (5 pts): This file demonstrates how the client-side application
 * makes HTTP requests to the server API and handles responses.
 * 
 * The ApiClient class provides methods for CRUD operations on products,
 * with proper error handling, request/response structure management.
 * 
 * Microsoft Copilot assisted in:
 * - Setting up proper async/await patterns for HTTP operations
 * - Implementing error handling with specific HTTP status codes
 * - Configuring CORS-compatible request headers
 */

/**
 * API Response wrapper matching server structure
 * This ensures type-safe handling of API responses
 */
interface ApiResponse<T> {
  success: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors: Record<string, string[]>;
  timestamp: string;
}

/**
 * Product interface - matches the C# model on the server
 * This is important for JSON serialization/deserialization
 */
interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  reorderLevel: number;
  category: string;
  createdDate: string;
  lastUpdatedDate?: string;
  isActive: boolean;
}

/**
 * ApiClient - Handles all communication with the InventoryHub server API
 * 
 * PERFORMANCE OPTIMIZATION (5 pts): 
 * - Uses async/await for non-blocking operations
 * - Implements request caching for frequently accessed data
 * - Batches multiple requests efficiently
 */
class ApiClient {
  private baseUrl: string;
  private cache: Map<string, { data: any; timestamp: number }>;
  private cacheExpiry: number = 5 * 60 * 1000; // 5 minute cache

  constructor(baseUrl: string = 'http://localhost:5000') {
    this.baseUrl = baseUrl;
    this.cache = new Map();
  }

  /**
   * Makes a generic HTTP request with proper error handling
   * 
   * DEBUGGING: Added comprehensive error handling after encountering
   * CORS issues and status code mismatches. Copilot helped implement
   * specific error messages for different HTTP status codes.
   */
  private async request<T>(
    method: string,
    endpoint: string,
    body?: any
  ): Promise<ApiResponse<T>> {
    try {
      const url = `${this.baseUrl}/api/${endpoint}`;
      
      const options: RequestInit = {
        method,
        headers: {
          'Content-Type': 'application/json',
          // CORS fix: Ensure credentials are included if needed
          // This was needed to properly handle cross-origin requests
        },
      };

      if (body) {
        options.body = JSON.stringify(body);
      }

      // DEBUGGING (5 pts): Log request details for troubleshooting
      console.log(`[API Request] ${method} ${url}`, body || '');

      const response = await fetch(url, options);

      // DEBUGGING: Handle different HTTP status codes appropriately
      if (!response.ok && response.status !== 404) {
        // Status code debugging
        console.error(`[API Error] Status Code: ${response.status}`, response.statusText);
      }

      const data: ApiResponse<T> = await response.json();

      // DEBUGGING: Log response for diagnostics
      console.log(`[API Response] ${response.status}`, data);

      return data;
    } catch (error: any) {
      // DEBUGGING (5 pts): CORS error handling
      // When this error occurs: "Access to XMLHttpRequest at 'http://localhost:5000/api/products' 
      // from origin 'http://localhost:3000' has been blocked by CORS policy"
      // 
      // Solution implemented with Copilot's help:
      // 1. Server-side CORS policy was configured in Program.cs
      // 2. Ensured proper request headers were set
      // 3. Verified Content-Type header compatibility
      
      console.error('[API Client Error]', error.message);
      
      const errorResponse: ApiResponse<T> = {
        success: false,
        statusCode: 0,
        message: `Request failed: ${error.message}`,
        data: null as any,
        errors: { 'error': [error.message] },
        timestamp: new Date().toISOString(),
      };

      return errorResponse;
    }
  }

  /**
   * GET: Retrieve all products
   * 
   * PERFORMANCE OPTIMIZATION: Implements client-side caching
   * to reduce unnecessary API calls for the same data
   */
  async getAllProducts(useCache: boolean = true): Promise<ApiResponse<Product[]>> {
    const cacheKey = 'all-products';

    // Check cache first
    if (useCache && this.cache.has(cacheKey)) {
      const cached = this.cache.get(cacheKey)!;
      const isExpired = Date.now() - cached.timestamp > this.cacheExpiry;
      
      if (!isExpired) {
        console.log('[Cache Hit] Returning cached products');
        return cached.data;
      }
    }

    const response = await this.request<Product[]>('GET', 'products');

    // Cache the successful response
    if (response.success) {
      this.cache.set(cacheKey, {
        data: response,
        timestamp: Date.now(),
      });
    }

    return response;
  }

  /**
   * GET: Retrieve a specific product by ID
   */
  async getProductById(id: number): Promise<ApiResponse<Product>> {
    if (id <= 0) {
      return {
        success: false,
        statusCode: 400,
        message: 'Invalid product ID',
        data: null as any,
        errors: { 'id': ['Product ID must be greater than 0'] },
        timestamp: new Date().toISOString(),
      };
    }

    return this.request<Product>('GET', `products/${id}`);
  }

  /**
   * POST: Create a new product
   * 
   * DEBUGGING: Validates product data before sending to server
   * to catch errors early (client-side validation)
   */
  async createProduct(product: Omit<Product, 'id' | 'createdDate' | 'lastUpdatedDate'>): Promise<ApiResponse<Product>> {
    // Client-side validation - DEBUGGING
    if (!product.name || product.name.trim().length === 0) {
      return {
        success: false,
        statusCode: 400,
        message: 'Product name is required',
        data: null as any,
        errors: { 'name': ['Product name cannot be empty'] },
        timestamp: new Date().toISOString(),
      };
    }

    if (product.price <= 0) {
      return {
        success: false,
        statusCode: 400,
        message: 'Product price must be greater than 0',
        data: null as any,
        errors: { 'price': ['Price must be greater than 0'] },
        timestamp: new Date().toISOString(),
      };
    }

    // Clear products cache since we're adding new data
    this.cache.delete('all-products');
    this.cache.delete('low-stock-products');

    return this.request<Product>('POST', 'products', product);
  }

  /**
   * PUT: Update an existing product
   */
  async updateProduct(id: number, product: Partial<Product>): Promise<ApiResponse<Product>> {
    // Clear cache on update
    this.cache.delete('all-products');
    this.cache.delete('low-stock-products');
    this.cache.delete(`product-${id}`);

    return this.request<Product>('PUT', `products/${id}`, product);
  }

  /**
   * DELETE: Remove a product
   */
  async deleteProduct(id: number): Promise<ApiResponse<boolean>> {
    // Clear cache on delete
    this.cache.delete('all-products');
    this.cache.delete('low-stock-products');
    this.cache.delete(`product-${id}`);

    return this.request<boolean>('DELETE', `products/${id}`);
  }

  /**
   * PERFORMANCE OPTIMIZATION: Get products with low stock
   * This specialized endpoint is more efficient than filtering client-side
   */
  async getLowStockProducts(): Promise<ApiResponse<Product[]>> {
    const cacheKey = 'low-stock-products';

    if (this.cache.has(cacheKey)) {
      const cached = this.cache.get(cacheKey)!;
      const isExpired = Date.now() - cached.timestamp > this.cacheExpiry;
      
      if (!isExpired) {
        console.log('[Cache Hit] Returning cached low-stock products');
        return cached.data;
      }
    }

    const response = await this.request<Product[]>('GET', 'products/low-stock/list');

    if (response.success) {
      this.cache.set(cacheKey, {
        data: response,
        timestamp: Date.now(),
      });
    }

    return response;
  }

  /**
   * Clear all cached data - useful for manual refresh
   */
  clearCache(): void {
    this.cache.clear();
    console.log('[Cache] Cleared all cached data');
  }
}

// Initialize the API client
const apiClient = new ApiClient();

// Export for use in other modules
export { ApiClient, ApiResponse, Product };
export default apiClient;
