# E-Commerce Application

A complete e-commerce solution with a C# ASP.NET Core API backend and a modern HTML/CSS/JavaScript front-end.

## Architecture

This application follows Clean Architecture principles with the following layers:

- **Domain**: Core business logic and entities
- **Application**: Use cases, commands, queries, and DTOs
- **Infrastructure**: External dependencies (database, authentication, etc.)
- **API**: RESTful web API controllers
- **Front-end**: Modern web interface

## Features

### Backend (C# ASP.NET Core)
- **User Management**: Registration, login, email verification
- **Product Management**: CRUD operations for products
- **Shopping Cart**: Add/remove items, guest and authenticated carts
- **Order Processing**: Create orders from cart
- **JWT Authentication**: Secure API endpoints
- **MongoDB Integration**: NoSQL database for flexibility
- **CQRS Pattern**: Commands and queries separation
- **Domain-Driven Design**: Rich domain models with business rules

### Frontend (HTML5/CSS3/JavaScript)
- **Responsive Design**: Works on all devices
- **Product Catalog**: Browse and search products
- **Shopping Cart**: Persistent cart with local storage
- **User Authentication**: Login/register forms
- **Modern UI**: Clean, professional interface
- **API Integration**: Full backend connectivity

## Tech Stack

### Backend
- **C# 12** / **.NET 9**
- **ASP.NET Core Web API**
- **MongoDB** (via MongoDB.Driver)
- **JWT Authentication**
- **MediatR** (CQRS implementation)
- **FluentValidation**
- **Serilog** (logging)

### Frontend
- **HTML5**
- **CSS3** (Flexbox, Grid, Animations)
- **JavaScript (ES6+)**
- **Font Awesome** (icons)

## Getting Started

### Prerequisites
- **.NET 9 SDK**
- **MongoDB** (running locally or cloud instance)
- **Python 3** (for serving front-end, or use any web server)

### Setup

1. **Clone/Download the project**

2. **Database Setup**:
   - Ensure MongoDB is running on `localhost:27017`
   - Database name: `ECommerceDb` (configured in `appsettings.json`)

3. **Backend Setup**:
   ```bash
   cd src
   dotnet build
   cd E-Commerce.Api
   dotnet run
   ```
   - API will be available at: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`

4. **Frontend Setup**:
   ```bash
   cd frontend
   python -m http.server 8080
   ```
   - Front-end will be available at: `http://localhost:8080`

### Configuration

Update `appsettings.json` in the API project:
```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  },
  "DatabaseName": "ECommerceDb",
  "JwtSettings": {
    "Secret": "Your_Super_Secret_Key_That_Must_Be_At_Least_32_Characters!",
    "Issuer": "E-Commerce-Api",
    "Audience": "E-Commerce-Users",
    "ExpiryInMinutes": 60
  }
}
```

## API Endpoints

### Products
- `GET /api/products` - Get all products (anonymous)
- `GET /api/products/{id}` - Get product by ID (anonymous)
- `POST /api/products` - Create product (authenticated)

### Users
- `POST /api/user/register` - Register new user
- `POST /api/user/login` - Login user
- `POST /api/user/verify-email` - Verify email

### Cart
- `POST /api/cart/add` - Add item to cart (anonymous/authenticated)
- `GET /api/cart` - Get cart contents (anonymous/authenticated)
- `PUT /api/cart/items/{id}` - Update cart item (anonymous/authenticated)
- `DELETE /api/cart` - Clear cart (anonymous/authenticated)

### Orders
- `POST /api/orders` - Create order (authenticated)
- `GET /api/orders` - Get user orders (authenticated)
- `GET /api/orders/{id}` - Get order by ID (authenticated)
- `PUT /api/orders/{id}/status` - Update order status (admin only)

## Project Structure

```
E-Commerce/
├── src/
│   ├── E-Commerce.Api/           # Web API controllers
│   ├── E-Commerce.Application/   # Use cases, DTOs, interfaces
│   ├── E-Commerce.Domain/        # Entities, value objects, domain logic
│   └── E-Commerce.Infrastructure/# External dependencies, repositories
├── frontend/                     # HTML/CSS/JS front-end
│   ├── index.html
│   ├── styles.css
│   ├── app.js
│   └── README.md
├── E-Commerce.sln               # Solution file
└── README.md                    # This file
```

## Development

### Running Tests
```bash
cd src
dotnet test
```

### Building for Production
```bash
cd src
dotnet publish E-Commerce.Api -c Release -o ./publish
```

### Database Seeding
The application will create collections automatically. For initial data, you can use the API endpoints or add seed data in the infrastructure layer.

## Security Features

- **JWT Authentication**: Bearer token-based auth
- **Password Hashing**: Secure password storage
- **Input Validation**: Comprehensive validation on all inputs
- **CORS**: Configured for cross-origin requests
- **HTTPS**: Redirects HTTP to HTTPS in production

## Contributing

1. Follow the existing code structure and naming conventions
2. Add tests for new features
3. Update documentation as needed
4. Ensure all builds pass before committing

## License

This project is developed for educational and demonstration purposes.

## Demo

- **API**: Visit `http://localhost:5000/swagger` for interactive API documentation
- **Front-end**: Visit `http://localhost:8080` for the web interface
- **Sample Data**: The front-end includes fallback sample products if the API is unavailable

## Troubleshooting

### Common Issues

1. **MongoDB Connection**: Ensure MongoDB is running and connection string is correct
2. **Port Conflicts**: Change ports in code if 5000/8080 are in use
3. **CORS Issues**: Update CORS policy in `Program.cs` for different front-end URLs
4. **Authentication**: Check JWT settings and ensure secret key is properly configured

### Logs
Check the console output and logs for detailed error information.