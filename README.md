# Finance Tracker API

A RESTful API for tracking personal income and expenses, built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQLite**. Features JWT authentication, clean architecture (Controller → Service → Repository), and Swagger documentation.

## Tech Stack

- **Language:** C# (.NET 8)
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Auth:** JWT Bearer Tokens
- **Docs:** Swagger / Swashbuckle

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run the project

```bash
# Clone the repo
git clone https://github.com/Timothynyezi/FinanceTrackerAPI.git
cd FinanceTrackerAPI

# Update the JWT key in appsettings.json (see Configuration below)

# Install EF Core tools (first time only)
dotnet tool install --global dotnet-ef

# Run migrations (creates the SQLite database)
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the API
dotnet run
```

The API will be available at `http://localhost:5000`.  
Swagger UI: `http://localhost:5000/swagger`

### Configuration

In `appsettings.json`, replace the JWT key with a secure random string (32+ characters):

```json
"Jwt": {
  "Key": "YOUR_STRONG_SECRET_KEY_HERE_MIN_32_CHARS",
  "Issuer": "FinanceTrackerAPI",
  "Audience": "FinanceTrackerClient"
}
```

## API Endpoints

### Auth
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login and receive JWT token | No |

### Transactions
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/transactions` | Get all transactions for logged-in user | Yes |
| GET | `/api/transactions/{id}` | Get a specific transaction | Yes |
| POST | `/api/transactions` | Create a new transaction | Yes |
| PUT | `/api/transactions/{id}` | Update a transaction | Yes |
| DELETE | `/api/transactions/{id}` | Delete a transaction | Yes |

### Categories
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/categories` | Get all categories | Yes |
| POST | `/api/categories` | Create a new category | Yes |

## Example Requests

### Register
```json
POST /api/auth/register
{
  "username": "thami",
  "password": "securepassword123"
}
```

### Create Transaction
```json
POST /api/transactions
Authorization: Bearer {your_token}
{
  "description": "Monthly salary",
  "amount": 15000.00,
  "date": "2026-04-01T00:00:00",
  "type": 0,
  "categoryId": 1
}
```
*Note: `type` — 0 = Income, 1 = Expense*

## Architecture

```
Controllers/     → Handle HTTP requests and responses
Services/        → Business logic layer
Repositories/    → Database access layer
Models/          → Entity classes (User, Transaction, Category)
DTOs/            → Request and response data shapes
Data/            → EF Core DbContext and migrations
Middleware/      → Global error handling
```

## Default Categories (seeded on first run)

Salary, Food & Groceries, Transport, Utilities, Entertainment, Healthcare, Other

## Author

**Thamsanqa Timothy Nyezi**  
[github.com/Timothynyezi](https://github.com/Timothynyezi) | [linkedin.com/in/tt-nyezi](https://linkedin.com/in/tt-nyezi)
