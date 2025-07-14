# RedFox Technical Test

## Introduction

This solution is a skeleton for managing user data from a remote API endpoint. Your task is to complete the implementation to fully support the data structure returned by the external API.

## Project Structure

```
RedFoxInterviewTest/
├── RedFox.Api          # Web API & Controllers
├── RedFox.Application  # CQRS, DTOs, MediatR
├── RedFox.Domain       # Entities, Value Objects
└── RedFox.Infrastructure # DbContext, Services
```

## Current Implementation

- ✅ Basic User/Company entities
- ✅ EF Core DbContext configuration
- ✅ Background service for initial data seeding
- ✅ Base CQRS pattern with MediatR
- ✅ API endpoint returning partial user data

## Technical Test Requirements

### 1. Complete Domain Model

- Add missing entities to match the API response structure
- Establish proper relationships between entities
- Consider value objects where appropriate

### 2. Database Configuration

- Configure EF Core relationships for nested entities
- Ensure proper migration generation
- Update data seeding service to handle full dataset

### 3. API Development

- Enhance existing endpoint to return full nested structure
- Add proper CORS configuration
- Maintain Clean Architecture principles

### 4. CQRS Implementation

- Update existing queries/handlers to include all relationships
- Modify DTOs to reflect complete data structure
- Ensure proper mapping between entities and DTOs

### 5. Documentation

- Add brief documentation about your approach
- List any trade-offs or assumptions made
- Provide instructions for testing the solution

## Expected API Response

The `GET /users` endpoint should return data in this format:

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

## Evaluation Criteria

1. **Architecture Compliance**
    - Proper layer separation
    - Adherence to Clean Architecture principles

2. **Data Modeling**
    - Correct entity relationships
    - Proper EF Core configuration

3. **API Implementation**
    - Accurate response format
    - Proper CORS configuration
    - Error handling

4. **Code Quality**
    - Readability
    - Proper use of patterns (CQRS, Repository)
    - Efficient data access

5. **Documentation**
    - Clear explanation of decisions
    - Setup/usage instructions

## Time Expectation

We expect this technical test to be completed within **8 hours of focused development time**. Please document any
incomplete parts with explanations.

---

**Good luck!** Please submit your solution as a Git repository with clear commit history.