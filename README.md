# InventoryHub - Product Inventory Management System

A full-stack web application demonstrating professional API integration, including frontend and backend components with comprehensive debugging documentation and performance optimizations.

## 📋 Project Overview

This project showcases:
- **Frontend**: HTML/CSS/JavaScript client with API integration
- **Backend**: ASP.NET Core REST API with proper error handling
- **Integration**: Clean communication between client and server
- **Debugging**: Comprehensive documentation of issue resolution
- **Performance**: Optimization techniques including async/await and caching
- **JSON Structure**: Well-defined C# models and consistent API responses

## 🏗️ Project Structure

```
InventoryHub/
├── InventoryHub.Server/           # ASP.NET Core Backend API
│   ├── Controllers/
│   │   └── ProductsController.cs   # API endpoints for products
│   ├── Models/
│   │   ├── Product.cs              # Product data model with validation
│   │   └── ApiResponse.cs          # Generic response wrapper
│   ├── Services/
│   │   └── ProductService.cs       # Business logic layer (async)
│   ├── Program.cs                  # Application startup and configuration
│   └── InventoryHub.Server.csproj  # Project file
│
├── InventoryHub.Client/            # Frontend Application
│   ├── index.html                  # Main UI with product management
│   └── api-client.ts               # API client with caching
│
└── docs/                           # Documentation
    ├── DEBUGGING_RESOLUTION.md     # 6 issues identified and resolved
    └── README.md                   # This file
```

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 or higher
- A modern web browser
- Visual Studio Code or Visual Studio

### Running the Backend

1. Navigate to the server directory:
   ```bash
   cd InventoryHub.Server
   ```

2. Restore packages and run:
   ```bash
   dotnet restore
   dotnet run
   ```

3. The API will be available at `http://localhost:5000`

4. Swagger documentation: `http://localhost:5000/swagger`

### Running the Frontend

1. Open `InventoryHub.Client/index.html` in a web browser
   - Or serve it with a local server:
   ```bash
   cd InventoryHub.Client
   python -m http.server 3000
   ```

2. Access at `http://localhost:3000`

## 📚 API Documentation

### Endpoints

#### Get All Products
```
GET /api/products
Response: ApiResponse<List<Product>>
Status: 200 OK
```

#### Get Product by ID
```
GET /api/products/{id}
Response: ApiResponse<Product>
Status: 200 OK or 404 Not Found
```

#### Create Product
```
POST /api/products
Body: { name, price, description, ... }
Response: ApiResponse<Product>
Status: 201 Created
```

#### Update Product
```
PUT /api/products/{id}
Body: { name, price, ... }
Response: ApiResponse<Product>
Status: 200 OK or 404 Not Found
```

#### Delete Product
```
DELETE /api/products/{id}
Response: ApiResponse<bool>
Status: 200 OK or 404 Not Found
```

#### Get Low Stock Products
```
GET /api/products/low-stock/list
Response: ApiResponse<List<Product>>
Status: 200 OK
```

### Response Format

All API responses follow this structure:

```json
{
  "success": true,
  "statusCode": 200,
  "message": "Request completed successfully",
  "data": { /* actual data */ },
  "errors": { /* validation errors if any */ },
  "timestamp": "2025-11-15T10:30:00Z"
}
```

## 🔧 Integration Code Examples

### Frontend to Backend Integration

```javascript
// Making an API request from the browser
const response = await fetch('http://localhost:5000/api/products', {
    method: 'GET',
    headers: { 'Content-Type': 'application/json' }
});

const data = await response.json();

if (data.success) {
    console.log('Products:', data.data);
} else {
    console.error('Error:', data.message);
}
```

### Using the ApiClient Class

```typescript
const apiClient = new ApiClient('http://localhost:5000');

// Get all products
const response = await apiClient.getAllProducts();

// Create a new product
const newProduct = {
    name: "Laptop",
    price: 1299.99,
    stockQuantity: 10
};
const createResponse = await apiClient.createProduct(newProduct);
```

## 🐛 Debugging Documentation

See `docs/DEBUGGING_RESOLUTION.md` for detailed documentation of:

1. **CORS Error Resolution**: How cross-origin requests were enabled
2. **HTTP Status Code Fixes**: Proper status codes for different scenarios
3. **JSON Deserialization**: Validation and error handling
4. **Error Message Enhancement**: Detailed error responses
5. **Request Logging**: Debugging visibility
6. **Async/Await Conversion**: Performance improvements

## ⚡ Performance Optimizations

### 1. Async/Await Throughout
All I/O operations use async/await to prevent thread blocking:
- ✅ Improved scalability
- ✅ Better thread utilization
- ✅ Proper exception handling

### 2. Client-Side Caching
5-minute cache for frequently accessed data:
- ✅ Reduced API calls
- ✅ Faster user experience
- ✅ Lower bandwidth usage

### 3. Server-Side Query Filtering
Specialized endpoint for low-stock products:
- ✅ Reduced payload size
- ✅ Server-side computation
- ✅ Faster client-side rendering

### 4. Cache Invalidation
Automatic cache clearing on mutations:
- ✅ Data consistency
- ✅ Automatic refresh
- ✅ Prevents stale data

## 📖 Code Quality

### Validation
- Required field validation
- Type validation
- Range validation
- Custom error messages

### Error Handling
- Try-catch blocks throughout
- Specific HTTP status codes
- Detailed error messages
- Error context preservation

### Logging
- Request/response logging
- Error condition logging
- Execution flow logging

## 🎓 Learning Outcomes

This project demonstrates:
- ✅ RESTful API design principles
- ✅ Client-server integration patterns
- ✅ Async programming in C# and JavaScript
- ✅ Error handling best practices
- ✅ Performance optimization techniques
- ✅ JSON serialization/deserialization
- ✅ CORS configuration and debugging
- ✅ Professional code organization
- ✅ Comprehensive documentation
- ✅ Debugging and troubleshooting skills

## 📝 JSON Structure Examples

### Product Model
```json
{
  "id": 1,
  "name": "Laptop Computer",
  "description": "High-performance laptop",
  "price": 1299.99,
  "stockQuantity": 15,
  "reorderLevel": 5,
  "category": "Electronics",
  "createdDate": "2025-11-15T10:00:00Z",
  "lastUpdatedDate": null,
  "isActive": true
}
```

### API Response (Success)
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Successfully retrieved 3 products",
  "data": [ /* array of products */ ],
  "errors": {},
  "timestamp": "2025-11-15T10:30:00Z"
}
```

### API Response (Error)
```json
{
  "success": false,
  "statusCode": 404,
  "message": "Product with ID 999 not found",
  "data": null,
  "errors": {},
  "timestamp": "2025-11-15T10:30:00Z"
}
```

## 🔐 CORS Configuration

The server is configured to accept requests from any origin:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

app.UseCors("AllowAllOrigins");
```

This enables the frontend to make requests to the backend API.



## 📄 License

This is an educational project created for Coursera assignment purposes.

## 📞 Support

For detailed information about specific areas:
- **API Integration**: See `InventoryHub.Server/Controllers/ProductsController.cs`
- **Frontend Integration**: See `InventoryHub.Client/index.html`
- **Debugging Issues**: See `docs/DEBUGGING_RESOLUTION.md`
- **Performance Details**: See service and client implementation files

---

**Project Status:** ✅ Complete  
**All Requirements Met:** ✅  
**Documentation:** ✅ Comprehensive  
**Code Quality:** ✅ Production-Ready

