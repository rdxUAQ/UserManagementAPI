.env data example
API_KEY=a1f6ec07-4cef-............


User Management REST API
Tech Stack: ASP.NET Core 8 | MongoDB | Serilog | JWT Auth

Features:

✅ Full CRUD operations for user management
✅ Global exception handling with standardized JSON responses
✅ Request/Response logging for auditing
✅ API Key authentication (X-API-Key header)
✅ Model validation with DataAnnotations
✅ Clean architecture (Controllers → Services → Repository)
✅ Swagger/OpenAPI documentation
✅ Environment-based configuration (.env support)


Endpoints:


GET /api/v1/users/all - List all users
GET /api/v1/users/{id} - Get user by ID
POST /api/v1/users/create - Create new user
PUT /api/v1/users/update/{id} - Update user
DELETE /api/v1/users/delete/{id} - Delete user
Response Format:
{
  "data": {...},
  "error": { "code": "ERR001", "description": "..." }
}

Security: API Key authentication, input validation, error masking in production

📝 One-liner caption options:
"Production-ready ASP.NET Core REST API with MongoDB, API Key auth, and enterprise-level error handling 🔐"

"Clean architecture User Management API: CRUD + logging + standardized responses + secure authentication ✨"

"ASP.NET Core 8 API with MongoDB persistence, Serilog auditing, and middleware-based auth layer
