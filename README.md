# RedFox Technical Test

## Table of Contents

1. [Introduction & Overview](#1-introduction--overview)
2. [Quick Start Guide](#2-quick-start-guide)
3. [Understanding the Requirements](#3-understanding-the-requirements)
4. [Technical Implementation Guide](#4-technical-implementation-guide)
5. [Testing & Verification](#5-testing--verification)
6. [Submission & Evaluation](#6-submission--evaluation)
7. [Reference & Troubleshooting](#7-reference--troubleshooting)

---

## 1. Introduction & Overview

This solution is a skeleton for managing user data from a remote API endpoint. Your task is to complete the implementation to fully support the data structure returned by the external API.

**⏰ Time Allocation: 10-12 hours of focused development time**

### Data Source
This project fetches user data from **JSONPlaceholder API**: `https://jsonplaceholder.typicode.com/users`

The external API returns rich user data including address and geographic coordinates, but **the skeleton currently only processes basic user and company data - address/geo data is ignored.**

### Project Architecture
```
RedFoxInterviewTest/
├── RedFox.Api          # Web API & Controllers
├── RedFox.Application  # CQRS, DTOs, MediatR
├── RedFox.Domain       # Entities, Value Objects
└── RedFox.Infrastructure # DbContext, Services
```

**Architecture:** Clean Architecture with CQRS pattern using MediatR, Entity Framework Core with SQLite, and AutoMapper.

---

## 2. Quick Start Guide

### Prerequisites
- .NET 8.0 SDK
- Your preferred IDE (Visual Studio, VS Code, Rider)
- Internet connection (required for external API calls)

### Setup & Run
```bash
# 1. Clone and navigate to project
git clone [repository-url]
cd RedFoxInterviewTest

# 2. Restore dependencies and build
dotnet restore
dotnet build

# 3. Run the API
dotnet run --project RedFox.Api

# 4. Verify setup
# Navigate to: http://localhost:5105/swagger
# Or test directly: GET http://localhost:5105/users
```

### Database Setup
- **Database:** SQLite with automatic creation (`ExtendedYankee2.db`)
- **Seeding:** Initial data fetched from JSONPlaceholder API on startup
- **Migrations:** EF Core handles schema updates

### Verification Checklist
✅ API starts successfully on http://localhost:5105  
✅ Database file `ExtendedYankee2.db` is created automatically  
✅ Initial data seeding fetches 10 users from external API  
✅ `/users` endpoint returns partial user data (missing address information)  
✅ Swagger UI accessible at http://localhost:5105/swagger  

---

## 3. Understanding the Requirements

### Current vs Expected Implementation

**Currently Working:**
- ✅ Basic User/Company entities with proper relationship
- ✅ EF Core DbContext configuration with SQLite
- ✅ Background service (`DbInitWorker`) for initial data seeding
- ✅ Base CQRS pattern with MediatR
- ✅ API endpoint returning partial user data
- ✅ AutoMapper configuration for basic mappings

**What You Need to Implement:**
- ❌ **Missing entities**: Address and Geo
- ❌ **Missing relationships**: User → Address (1:1), Address → Geo (1:1)  
- ❌ **Incomplete data extraction**: DbInitWorker ignores address/geo from API
- ❌ **Missing DTOs**: AddressDto, GeoDto
- ❌ **Incomplete mappings**: AutoMapper missing address/geo mappings
- ❌ **Missing query includes**: Handlers don't load address/geo relationships
- ❌ **Missing CRUD operations**: Only GET /users implemented
- ❌ **Missing CORS**: No cross-origin configuration

### API Endpoints to Implement
| Endpoint | Method | Status | Description |
|----------|--------|--------|-------------|
| `/users` | GET | ✅ Partial | Get all users (missing address data) |
| `/users/{id}` | GET | ❌ Missing | Get single user by ID |
| `/users` | POST | ❌ Missing | Create new user |
| `/users/{id}` | PUT | ❌ Missing | Update existing user |
| `/users/{id}` | DELETE | ❌ Missing | Delete user |

---

## 4. Technical Implementation Guide

### Step 1: Complete Domain Model

**Required Entity Relationships:**
- **User** (1) ↔ (1) **Address** (one-to-one)
- **Address** (1) ↔ (1) **Geo** (one-to-one)  
- **User** (*) ↔ (1) **Company** (many-to-one, already implemented)

**Entity Design Considerations:**
- Address and Geo can be implemented as entities or value objects
- Consider which approach fits best with EF Core and your design
- Ensure proper foreign key relationships

### Step 2: Database Configuration

**Tasks:**
1. Create EF Core entity configurations for Address and Geo
2. Configure one-to-one relationships with proper foreign keys
3. Create and apply EF Core migration:
   ```bash
   dotnet ef migrations add AddAddressAndGeo -p RedFox.Infrastructure -s RedFox.Api
   dotnet ef database update -p RedFox.Infrastructure -s RedFox.Api
   ```
4. Update DbInitWorker to extract and save address/geo data from API

### Step 3: CQRS & Data Layer Updates

**Required Changes:**
- Update existing queries/handlers to include all relationships using `.Include()`
- Create DTOs for Address and Geo entities
- Update UserDto to include address property
- Update AutoMapper profiles for complete object mapping
- Ensure UserCreationDto handles nested address data from API

### Step 4: Complete CRUD API Implementation

**Required Endpoints:**
- `GET /users` - Get all users with nested data (✅ enhance existing)
- `GET /users/{id}` - Get single user by ID with full nested structure
- `POST /users` - Create new user with address and company information
- `PUT /users/{id}` - Update existing user (full update)
- `DELETE /users/{id}` - Delete user and associated address/geo data

**CQRS Implementation Requirements:**
- Create individual commands: `CreateUserCommand`, `UpdateUserCommand`, `DeleteUserCommand`
- Create corresponding handlers following existing pattern
- Implement proper validation using FluentValidation or built-in validation
- Create request/response DTOs for each operation
- Handle cascade operations (address/geo relationships)

**Implementation Guidelines:**
- Each endpoint should follow CQRS pattern with separate commands/queries
- Use proper Entity Framework Include() statements for nested data
- Implement validation at both DTO and entity level
- Consider using FluentValidation for complex validation rules
- Ensure proper transaction handling for create/update operations
- Handle foreign key relationships correctly (User-Address-Geo cascade)

### Step 5: API & Infrastructure

**Required Updates:**
- Enhance `/users` endpoint to return complete nested structure
- Add CORS configuration to allow `localhost` origins for development
- Update query handlers to include address/geo relationships
- Maintain Clean Architecture principles throughout

**API Requirements:**
- Follow RESTful conventions with proper HTTP verbs
- Return appropriate HTTP status codes (200, 201, 204, 404, 400, 500)
- Implement proper error handling and validation responses
- Ensure all endpoints return consistent JSON structure
- Add request validation for required fields and business rules

### Step 6: Documentation

**Deliverables:**
- Brief documentation about your architectural decisions
- Explanation of entity vs value object choice for Address/Geo
- CRUD API design decisions and validation strategy
- Any trade-offs or assumptions made
- Instructions for running and testing your solution

---

## 5. Testing & Verification

### Verification Steps

1. **Complete CRUD API Testing:**
   ```bash
   # Test GET all users
   GET http://localhost:5105/users
   
   # Test GET single user
   GET http://localhost:5105/users/1
   
   # Test CREATE user
   POST http://localhost:5105/users
   Content-Type: application/json
   { /* User creation JSON */ }
   
   # Test UPDATE user
   PUT http://localhost:5105/users/1
   Content-Type: application/json
   { /* Updated user JSON */ }
   
   # Test DELETE user
   DELETE http://localhost:5105/users/1
   ```

2. **API Response Verification:**
   - All endpoints return complete nested JSON with address and geo data
   - Proper HTTP status codes (200, 201, 204, 404, 400, 500)
   - Consistent JSON structure across all endpoints
   - Error responses include meaningful validation messages

3. **Database Verification:**
   - Check SQLite database contains Address and Geo tables
   - Verify all 10 users have associated address and geo records
   - Confirm proper foreign key relationships
   - Test cascade operations (deleting user removes address/geo)

4. **Validation Testing:**
   - Test required field validation (empty name, email, etc.)
   - Test email format validation
   - Test duplicate username/email handling
   - Test invalid ID handling (404 responses)

5. **Code Quality Check:**
   - Solution builds without warnings
   - All layers maintain Clean Architecture separation
   - CQRS pattern consistently applied for all operations
   - Proper error handling and logging

### Definition of "Complete"
- ✅ All 10 users from JSONPlaceholder API stored with complete data
- ✅ All 5 CRUD endpoints implemented and working correctly
- ✅ API returns exact format shown in "Expected API Response" (see Reference section)
- ✅ Database schema includes all required relationships
- ✅ Proper validation and error handling implemented
- ✅ CORS allows localhost requests
- ✅ Code follows existing architectural patterns

---

## 6. Submission & Evaluation

### Git Workflow Requirements

1. **Create Feature Branch:**
   ```bash
   git checkout -b firstName_lastName
   ```
   Replace `firstName_lastName` with your actual name (e.g., `john_doe`)

2. **Development Process:**
   - Make regular commits with clear, descriptive messages
   - Follow conventional commit format when possible
   - Commit frequently to show development progression

3. **Pull Request Submission:**
   ```bash
   git push origin firstName_lastName
   ```
   - Create a Pull Request from your branch to `main`
   - **Important:** A well-described PR earns additional points

### PR Description Requirements

Your Pull Request description should include:
- **Summary:** Brief overview of what you implemented
- **Architecture Decisions:** Explain your entity design choices (entities vs value objects)
- **CRUD Implementation:** Describe your API design and validation strategy
- **Trade-offs:** Document any assumptions or compromises made
- **Testing Instructions:** How to verify your implementation works
- **Time Breakdown:** Actual time spent on each major component

### Evaluation Criteria

| Category | Weight | Focus Areas |
|----------|--------|-------------|
| **Architecture Compliance** | 25% | Clean Architecture separation, dependency direction, CQRS adherence |
| **Data Modeling & EF Core** | 25% | Entity relationships, EF Core configuration, efficient queries |
| **CRUD API Implementation** | 30% | Complete operations, JSON format, HTTP codes, validation |
| **Code Quality & Patterns** | 10% | Clean code, proper patterns, existing conventions |
| **Documentation & Testing** | 10% | Design explanations, verification ability, commit history |

### Evaluation Bonus
- **Clear PR description:** +5 bonus points
- **Comprehensive documentation:** +3 bonus points  
- **Clean commit history:** +2 bonus points

### Time Allocation Guidance

**Recommended breakdown for 10-12 hour challenge:**
- Domain modeling (Address/Geo entities): 1-2 hours
- EF Core configuration and migrations: 1-2 hours  
- Data extraction (DbInitWorker updates): 1-2 hours
- DTOs and AutoMapper updates: 1-2 hours
- CRUD API implementation (POST, PUT, DELETE): 3-4 hours
- Query handler updates and testing: 1-2 hours
- CORS, documentation, final testing: 1 hour

**Important:** Document any incomplete parts with explanations if you run out of time.

---

## 7. Reference & Troubleshooting

### Expected API Response Examples

#### Current API Response (Incomplete)
```json
[
  {
    "id": 1,
    "name": "Leanne Graham",
    "username": "Bret", 
    "email": "Sincere@april.biz",
    "phone": "1-770-736-8031 x56442",
    "website": "hildegard.org",
    "company": {
      "name": "Romaguera-Crona",
      "catchPhrase": "Multi-layered client-server neural-net",
      "bs": "harness real-time e-markets"
    }
  }
]
```

#### GET /users - Expected Complete Response
```json
[
  {
    "id": 1,
    "name": "Leanne Graham",
    "username": "Bret",
    "email": "Sincere@april.biz",
    "address": {
      "street": "Kulas Light",
      "suite": "Apt. 556", 
      "city": "Gwenborough",
      "zipcode": "92998-3874",
      "geo": {
        "lat": "-37.3159",
        "lng": "81.1496"
      }
    },
    "phone": "1-770-736-8031 x56442",
    "website": "hildegard.org",
    "company": {
      "name": "Romaguera-Crona",
      "catchPhrase": "Multi-layered client-server neural-net",
      "bs": "harness real-time e-markets"
    }
  }
]
```

#### GET /users/{id} - Expected Single User Response
```json
{
  "id": 1,
  "name": "Leanne Graham",
  "username": "Bret",
  "email": "Sincere@april.biz",
  "address": {
    "street": "Kulas Light",
    "suite": "Apt. 556",
    "city": "Gwenborough", 
    "zipcode": "92998-3874",
    "geo": {
      "lat": "-37.3159",
      "lng": "81.1496"
    }
  },
  "phone": "1-770-736-8031 x56442",
  "website": "hildegard.org",
  "company": {
    "name": "Romaguera-Crona",
    "catchPhrase": "Multi-layered client-server neural-net",
    "bs": "harness real-time e-markets"
  }
}
```

#### POST /users - Create User Request/Response
**Request Body:**
```json
{
  "name": "John Doe",
  "username": "johndoe",
  "email": "john.doe@example.com", 
  "address": {
    "street": "123 Main St",
    "suite": "Apt 1",
    "city": "Anytown",
    "zipcode": "12345",
    "geo": {
      "lat": "40.7128",
      "lng": "-74.0060"
    }
  },
  "phone": "555-0123",
  "website": "johndoe.com",
  "company": {
    "name": "ACME Corp",
    "catchPhrase": "Innovation at its finest",
    "bs": "synergize cutting-edge solutions"
  }
}
```

**Response (201 Created):**
```json
{
  "id": 11,
  "name": "John Doe",
  "username": "johndoe",
  "email": "john.doe@example.com",
  "address": {
    "street": "123 Main St",
    "suite": "Apt 1", 
    "city": "Anytown",
    "zipcode": "12345",
    "geo": {
      "lat": "40.7128",
      "lng": "-74.0060"
    }
  },
  "phone": "555-0123",
  "website": "johndoe.com",
  "company": {
    "name": "ACME Corp",
    "catchPhrase": "Innovation at its finest",
    "bs": "synergize cutting-edge solutions"
  }
}
```

#### PUT /users/{id} - Update User
**Request Body:** Same format as POST  
**Response (200 OK):** Updated user object with same structure

#### DELETE /users/{id} - Delete User
**Response (204 No Content):** Empty response body

### EF Core Migration Commands
```bash
# Create migration for new entities
dotnet ef migrations add MigrationName -p RedFox.Infrastructure -s RedFox.Api

# Apply migrations to database  
dotnet ef database update -p RedFox.Infrastructure -s RedFox.Api

# Remove database to start fresh (if needed)
rm ExtendedYankee2.db*
```

### Common Issues & Solutions

**Port Already in Use:**
```bash
dotnet run --project RedFox.Api --urls="http://localhost:5106"
```

**Database Lock Issues:**
- Stop the application completely
- Delete `ExtendedYankee2.db*` files to reset database

**External API Connection:**
- Ensure internet connectivity
- Check if JSONPlaceholder API is accessible: `https://jsonplaceholder.typicode.com/users`

**Migration Issues:**
- Ensure you're in the solution root directory
- Use the full project paths as shown in commands above

### Getting Help
- Check Swagger UI for API documentation: `http://localhost:5105/swagger`
- Review existing working code patterns (User/Company implementation)
- Ensure all NuGet packages are restored: `dotnet restore`

---

**Good luck!** Remember that communication through your PR description is as important as the code itself.