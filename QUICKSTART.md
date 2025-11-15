# InventoryHub - Quick Start Guide

## 5-Minute Setup

### Prerequisites
- .NET 6.0 SDK installed
- A web browser
- Visual Studio Code (optional but recommended)

### Step 1: Start the Backend Server

```bash
# Navigate to the server directory
cd InventoryHub.Server

# Restore NuGet packages
dotnet restore

# Run the server
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

### Step 2: Open the Frontend

1. Navigate to `InventoryHub.Client` folder
2. Open `index.html` in your web browser
   - Simply double-click the file, OR
   - Use a local server:
     ```bash
     cd InventoryHub.Client
     python -m http.server 8000
     # Then visit http://localhost:8000
     ```

### Step 3: Test the Application

1. Click "Refresh All" to load products
2. Fill out the "Add New Product" form
3. Click "Add Product"
4. Watch it appear in the products list!

## Features to Try

### View All Products
- Click "Refresh All" button
- Products load from server with caching

### Add a New Product
- Fill in the product form
- Click "Add Product"
- Product appears in the list immediately

### View Low Stock Items
- Click "Low Stock Only" button
- Shows only products below reorder level

### Clear Cache
- Click "Clear Cache" button
- Next refresh will fetch fresh data from server

## API Endpoints

Test these directly in your browser console or with Postman:

### Get All Products
```javascript
fetch('http://localhost:5000/api/products')
  .then(r => r.json())
  .then(d => console.log(d))
```

### Create a Product
```javascript
fetch('http://localhost:5000/api/products', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    name: 'Test Product',
    price: 99.99,
    stockQuantity: 10,
    category: 'Electronics'
  })
})
  .then(r => r.json())
  .then(d => console.log(d))
```

## Troubleshooting

### CORS Error
**Error:** "Access to XMLHttpRequest ... has been blocked by CORS policy"

**Solution:** Make sure the backend server is running on `http://localhost:5000`

### Port Already in Use
**Error:** "The port 5000 is already in use"

**Solution:** Either:
1. Stop the process using port 5000
2. Or change the port in Program.cs and rebuild

### Products Not Loading
**Error:** UI shows empty product list

**Solution:**
1. Check that backend is running (http://localhost:5000)
2. Open browser console (F12) to see error messages
3. Click "Clear Cache" and refresh

## File Structure

```
InventoryHub/
├── InventoryHub.Server/          ← Run this first (dotnet run)
├── InventoryHub.Client/          ← Open index.html in browser
├── README.md                      ← Project overview
├── REFLECTION.md                  ← Copilot assistance details
└── docs/
    └── DEBUGGING_RESOLUTION.md    ← Issue resolution guide
```

## Key Files

### Backend
- `Program.cs` - Server configuration (including CORS)
- `Controllers/ProductsController.cs` - API endpoints
- `Services/ProductService.cs` - Business logic
- `Models/Product.cs` - Data model
- `Models/ApiResponse.cs` - Response wrapper

### Frontend
- `index.html` - UI and JavaScript integration code
- `api-client.ts` - API client class (TypeScript)

## Performance Features

✅ **Caching**: Products cached for 5 minutes  
✅ **Async Operations**: Non-blocking server operations  
✅ **Optimized Queries**: Low-stock endpoint filters on server  
✅ **Cache Invalidation**: Automatic cache clearing on mutations  

## Next Steps

1. **Explore the Code**: Review the implementation in the files
2. **Read Documentation**: Check `DEBUGGING_RESOLUTION.md` for issue resolution
3. **Review Integration**: See how frontend calls backend API
4. **Study Performance**: Learn about caching and async patterns
5. **Test More**: Add more products, refresh, modify, delete

## Common Tasks

### Add Multiple Products
```javascript
const products = [
  { name: 'Mouse', price: 25.99, stockQuantity: 50 },
  { name: 'Keyboard', price: 79.99, stockQuantity: 30 },
  { name: 'Monitor', price: 299.99, stockQuantity: 5 }
];

products.forEach(p => {
  fetch('http://localhost:5000/api/products', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(p)
  })
  .then(r => r.json())
  .then(d => console.log('Created:', d.data.name))
});
```

### Check Server Health
```javascript
fetch('http://localhost:5000/api/products')
  .then(r => console.log('Server Status:', r.status))
  .catch(e => console.error('Server Error:', e.message))
```

### Monitor Cache
The browser console shows cache hits:
```
[Cache] Using cached products
```

## Support

For detailed information:
- **Integration Examples**: See `InventoryHub.Client/index.html`
- **API Details**: See `InventoryHub.Server/Controllers/ProductsController.cs`
- **Debugging Help**: See `docs/DEBUGGING_RESOLUTION.md`
- **Learning Guide**: See `REFLECTION.md`

---

**Ready to test?** Start with `dotnet run` in the Server folder! 🚀
