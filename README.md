# BlanchisserieBackend

This is the backend API for a laundry management application, built with ASP.NET Core and Entity Framework Core.

## Features

- JWT authentication with HTTP-only cookie support
- Role-based authorization (Admin/User)
- CRUD operations for Users, Articles, Client Orders
- Automatic mapping with AutoMapper
- PostgreSQL database support
- CORS configuration for Angular frontend
- Swagger documentation

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Node.js](https://nodejs.org/) (if you want to run the Angular frontend)
- [Git](https://git-scm.com/)

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/blanchisseriebackend.git
   cd blanchisseriebackend
   ```

2. **Configure environment variables**

   Create a `.env` file at the root of the project with the following content:
   ```
   DB_HOST=localhost
   DB_PORT=5432
   DB_NAME=blanchisserie
   DB_USER=your_db_user
   DB_PASSWORD=your_db_password
   JWT_SECRET=your_jwt_secret
   JWT_ISSUER=your_jwt_issuer
   ```

3. **Install dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the backend**
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5150`.

## Usage

- **Swagger UI**: Visit `http://localhost:5150/swagger` for API documentation and testing.
- **Authentication**: Real authentication uses an HTTP-only cookie named `access_token`. Swagger uses the Authorization header only for local tests.
- **CORS**: The backend is configured to accept requests from `http://localhost:4200` (Angular frontend).

## Development

- To add new entities or features, update the models, services, controllers, and DTOs as needed.
- Use AutoMapper for automatic mapping between entities and DTOs.
- For role-based access, use `[Authorize(Roles = "Admin")]` on controllers or actions.

## Troubleshooting

- If you change the order of properties in your models and want to reflect this in the database, you must recreate the table (drop and migrate).
- Make sure your database connection and JWT settings are correct in the `.env` file.

## Testing data
To test the application, check the file `/Data/ApplicationDbContext.cs`. The test data is located at the end of the file, especially for users you can use to log in.