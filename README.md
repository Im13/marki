# 🛍️ Marki - Full-Stack E-Commerce Platform

A modern, scalable e-commerce platform built with .NET 7 and Angular 18, featuring real-time order notifications, comprehensive admin dashboard, and integration with third-party marketplaces.

[![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18-DD0031?logo=angular)](https://angular.io/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 📋 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [API Documentation](#-api-documentation)
- [Screenshots](#-screenshots)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### Customer Portal
- 🔐 **User Authentication & Authorization** - Secure JWT-based authentication
- 🛒 **Shopping Cart** - Real-time cart management with Redis caching
- 📦 **Product Catalog** - Advanced product browsing with SKU variants
- 🔍 **Product Search & Filtering** - Fast and efficient product discovery
- 💳 **Checkout Process** - Streamlined multi-step checkout
- 📍 **Address Management** - Support for Vietnamese provinces/districts/wards
- 🔔 **Real-time Notifications** - SignalR-powered order status updates
- 📱 **Responsive Design** - Mobile-first approach

### Admin Dashboard
- 📊 **Dashboard Analytics** - Revenue tracking and business insights
- 👥 **Customer Management** - Complete customer lifecycle management
- 📦 **Order Processing** - Order fulfillment and tracking
- 🏷️ **Product Management** - CRUD operations with image uploads
- 🎨 **Website Customization** - Manage slides and promotional content
- 📈 **Statistics & Reports** - Sales analytics and performance metrics
- 🛍️ **Shopee Integration** - Third-party marketplace synchronization
- 🤖 **Recommendation System** - AI-powered product recommendations

### Technical Features
- ⚡ **Clean Architecture** - Separation of concerns with Core, Infrastructure, and API layers
- 🔄 **Real-time Communication** - SignalR hubs for instant updates
- 💾 **Redis Caching** - Improved performance with distributed caching
- 🎯 **Repository Pattern** - Abstracted data access layer
- 📝 **Specification Pattern** - Flexible and reusable query logic
- 🧪 **Unit Testing** - Comprehensive test coverage
- 🐳 **Docker Support** - Containerized Redis services
- 📚 **Swagger/OpenAPI** - Interactive API documentation

## 🛠️ Tech Stack

### Backend
- **Framework:** ASP.NET Core 7.0
- **Database:** SQLite (Development), SQL Server ready (Production)
- **ORM:** Entity Framework Core 7.0
- **Authentication:** JWT Bearer Tokens, ASP.NET Core Identity
- **Caching:** Redis
- **Real-time:** SignalR
- **Mapping:** AutoMapper
- **API Documentation:** Swagger/Swashbuckle

### Frontend (Client)
- **Framework:** Angular 18
- **UI Library:** Ng-Zorro (Ant Design)
- **Styling:** Bootstrap 5, Font Awesome
- **State Management:** RxJS
- **Notifications:** ngx-toastr
- **Gallery:** @daelmaak/ngx-gallery
- **Carousel:** Swiper

### Frontend (Admin)
- **Framework:** Angular 18
- **UI Components:** Ng-Zorro (Ant Design)
- **Rich Features:** Statistics, Analytics, Content Management

### DevOps
- **Containerization:** Docker, Docker Compose
- **Version Control:** Git

## 🏗️ Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────┐
│                    API Layer                    │
│  (Controllers, Middleware, DTOs, Extensions)    │
├─────────────────────────────────────────────────┤
│                   Core Layer                    │
│     (Entities, Interfaces, Specifications)      │
├─────────────────────────────────────────────────┤
│              Infrastructure Layer               │
│  (Data, Identity, Services, Repositories)       │
└─────────────────────────────────────────────────┘

┌──────────────┐         ┌──────────────┐
│    Client    │ ←────→  │    Admin     │
│  (Angular)   │         │  (Angular)   │
└──────────────┘         └──────────────┘
```

### Key Architectural Patterns
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Specification Pattern** - Query composition
- **Dependency Injection** - Loose coupling
- **Middleware Pattern** - Request/response pipeline

## 🚀 Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for Redis)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/marki.git
cd marki
```

2. **Start Redis (using Docker)**
```bash
docker-compose up -d
```

3. **Setup Backend API**
```bash
cd API/API
dotnet restore
dotnet ef database update
dotnet run
```
The API will be available at `https://localhost:5001` (or configured port)

4. **Setup Client Application**
```bash
cd Client
npm install
ng serve
```
Access at `http://localhost:4200`

5. **Setup Admin Dashboard**
```bash
cd Admin
npm install
ng serve
```
Access at `http://localhost:4201`

### Default Credentials

**Admin Account:**
- Email: `admin@marki.com`
- Password: `Pa$$w0rd`

**Test Customer Account:**
- Email: `customer@test.com`
- Password: `Pa$$w0rd`

### Configuration

Update `appsettings.json` in the API project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=marki.db",
    "IdentityConnection": "Data Source=identity.db",
    "Redis": "localhost:6379"
  },
  "Token": {
    "Key": "your-super-secret-key-min-32-chars",
    "Issuer": "https://localhost:5001"
  }
}
```

## 📁 Project Structure

```
marki/
├── API/                          # Backend .NET Solution
│   ├── API/                      # Web API Project
│   │   ├── Controllers/          # API Endpoints
│   │   ├── DTOs/                 # Data Transfer Objects
│   │   ├── Extensions/           # Service Extensions
│   │   ├── Middleware/           # Custom Middleware
│   │   └── Program.cs            # Application Entry Point
│   ├── Core/                     # Domain Layer
│   │   ├── Entities/             # Domain Entities
│   │   ├── Interfaces/           # Repository Interfaces
│   │   ├── Specifications/       # Query Specifications
│   │   └── Services/             # Domain Services
│   ├── Infrastructure/           # Infrastructure Layer
│   │   ├── Data/                 # DbContext, Repositories
│   │   ├── Identity/             # Authentication & Authorization
│   │   ├── Services/             # External Services
│   │   └── Hubs/                 # SignalR Hubs
│   └── API.Tests/                # Unit & Integration Tests
├── Client/                       # Customer-facing Angular App
│   ├── src/
│   │   ├── app/
│   │   │   ├── account/          # Authentication
│   │   │   ├── basket/           # Shopping Cart
│   │   │   ├── checkout/         # Checkout Process
│   │   │   ├── products/         # Product Catalog
│   │   │   ├── home/             # Landing Page
│   │   │   └── shared/           # Shared Components
│   │   └── assets/               # Static Assets
│   └── package.json
├── Admin/                        # Admin Dashboard Angular App
│   ├── src/
│   │   ├── app/
│   │   │   ├── dashboard/        # Analytics Dashboard
│   │   │   ├── customer/         # Customer Management
│   │   │   ├── order/            # Order Management
│   │   │   ├── product/          # Product Management
│   │   │   ├── statistics/       # Reports & Statistics
│   │   │   └── settings/         # System Settings
│   │   └── assets/
│   └── package.json
├── ClientUI/                     # Additional UI Components
├── docker-compose.yml            # Docker Services Configuration
└── README.md
```

## 📚 API Documentation

Once the API is running, access the interactive Swagger documentation at:
```
https://localhost:5001/swagger
```

### Main Endpoints

#### Authentication
- `POST /api/account/login` - User login
- `POST /api/account/register` - User registration
- `GET /api/account` - Get current user

#### Products
- `GET /api/products` - Get products with pagination
- `GET /api/products/{id}` - Get product details
- `POST /api/products` - Create product (Admin)
- `PUT /api/products/{id}` - Update product (Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

#### Basket
- `GET /api/basket` - Get user's basket
- `POST /api/basket` - Update basket
- `DELETE /api/basket/{id}` - Delete basket item

#### Orders
- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order details
- `POST /api/orders` - Create order

#### Admin
- `GET /api/admin/dashboard` - Dashboard statistics
- `GET /api/admin/orders` - All orders management
- `GET /api/admin/customers` - Customer management

## 📸 Screenshots

### Customer Portal

#### Home Page
![Home Page](docs/screenshots/home.png)

#### Product Catalog
![Product Catalog](docs/screenshots/products.png)

#### Shopping Cart
![Shopping Cart](docs/screenshots/cart.png)

#### Checkout
![Checkout](docs/screenshots/checkout.png)

### Admin Dashboard

#### Analytics Dashboard
![Dashboard](docs/screenshots/admin-dashboard.png)

#### Order Management
![Orders](docs/screenshots/admin-orders.png)

#### Product Management
![Products](docs/screenshots/admin-products.png)

> **Note:** Add actual screenshots to the `docs/screenshots/` directory

## 🧪 Testing

Run backend tests:
```bash
cd API
dotnet test
```

Run frontend tests:
```bash
cd Client
ng test
```

## 🔐 Security Features

- JWT token-based authentication
- Password hashing with ASP.NET Core Identity
- CORS policy configuration
- HTTPS enforcement
- SQL injection prevention via Entity Framework
- XSS protection
- CSRF token validation

## 🚀 Deployment

### Backend Deployment
The API can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
- IIS

### Frontend Deployment
The Angular apps can be deployed to:
- Vercel
- Netlify
- Azure Static Web Apps
- AWS S3 + CloudFront

### Database Migration
For production, replace SQLite with SQL Server or PostgreSQL:

1. Update connection string in `appsettings.json`
2. Install appropriate EF Core provider
3. Run migrations:
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 👨‍💻 Author

**Bach Nguyen Luong**

- GitHub: [@Im13](https://github.com/Im13)
- LinkedIn: (https://www.linkedin.com/in/nguy%E1%BB%85n-l%C6%B0%C6%A1ng-b%C3%A1ch-483863220/)
- Email: bach.nguyenluongmsh@gmail.com

## 🙏 Acknowledgments

- [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Angular](https://angular.io/)
- [Ng-Zorro](https://ng.ant.design/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [SignalR](https://docs.microsoft.com/aspnet/core/signalr)

## 📞 Support

If you have any questions or need help, please open an issue or contact me directly.

---

⭐️ If you find this project useful, please consider giving it a star!

**Built with ❤️ using .NET and Angular**
