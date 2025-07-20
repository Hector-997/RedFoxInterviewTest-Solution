# RedFoxInterviewTest

This project is a RESTful API developed in .NET, built to manage user entities and their associated data. It was designed with clean architecture principles in mind, leveraging CQRS with MediatR, FluentValidation for request validation, and a layered project structure to ensure separation of concerns and scalability.

---

## 🚀 Features Implemented

### 🔍 Get All Users
Retrieve a list of all registered users. (Enhanced)

### 🔍 Get User by ID
Get a single user by their unique identifier.

### 🆕 Create User
Add a new user including nested Address and Company details.

### ✏️ Update User
Fully update an existing user by ID.

### ❌ Delete User
Remove a user record by ID.

### 🧪 HTTP Request Samples
All requests can be tested using the `RedFox.Api/Requests/users.http` file.

---

## ✅ Architectural Decisions

- **📦 Address & Geolocation as Value Objects**
  - These do not have identities and exist only as part of User or another entity in the future.
  - Two identical values are treated as equal.
  
- **📥 DTOs for Address & Geolocation**
  - To ensure proper separation, DTOs are used in request/response models while the actual Value Objects are exclusive to the domain layer.

- **📚 CQRS Pattern**
  - Commands and Queries are separated under the `Features` folder, grouped by action and entity.
  - Example: `Create`, `GetSingle`, `Update`, `Delete`, and `Seed` for the `User` entity.

- **🌱 Database Seeding & Migration Strategy**
  - Upon application startup, if there are any pending migrations, they are automatically applied.
  - A seed operation is triggered only if the `Users` table is empty.
  - This approach ensures a clean and version-controlled initial migration and allows further migrations to evolve the schema.

- **🧭 Custom Middleware for Exception Handling**
  - A global middleware intercepts exceptions across the application.
  - Validation exceptions are caught and returned as structured `400 Bad Request` responses.
  
- **🧪 ValidationBehavior**
  - All commands and queries are validated using FluentValidation via a custom MediatR pipeline behavior.

- **✋ Manual Mapping over AutoMapper (Suggested Improvement)**
  - While AutoMapper is currently used, switching to manual mapping is recommended for improved control and debugging.

---

## ⚠️ Development Notes

- The API can be tested directly within **Visual Studio** by opening and executing the `users.http` file using the built-in REST Client.
- There were difficulties encountered when using `record` types with AutoMapper. Adjustments were required to ensure compatibility while preserving the desired structure.
- Challenges were also faced with the initial database version. The seed data did not anticipate additional columns planned for the schema, causing incompatibilities with migrations. These were resolved by establishing a base migration structure first and applying further changes incrementally.

---

## 🛡️ CORS Policy

A CORS policy has been implemented to allow unrestricted access during development. This policy (`DevCorsPolicy`) enables requests from any origin, using any HTTP method and headers. It is applied conditionally based on the environment using helper extensions (`AddCustomCors`, `UseCustomCors`) defined in the `Config` folder.

## ✅ Input Validations

FluentValidation is used to enforce rules during user creation and update. The following validations are applied through the `AddUserWithRelatedRequestValidator`:

- `Name`: Required.
- `Username`: Required.
- `Email`: Required and must be a valid email address.
- `Phone`: Required.
- `Website`: Required and must be a valid URI.

These validations ensure that incoming requests adhere to the expected structure and data quality.

## ⚙️ How to Run

```bash
# Apply migrations and seed data (if DB is empty)
dotnet run
```

Make sure the project has access to a valid SQLite connection string as defined in `appsettings.Development.json`.



## 🧑‍💻 Author

Developed as part of a technical interview challenge. All architectural decisions were made to reflect best practices in maintainability and readability.