# WayOfMilk

WayOfMilk is a distributed farm management system developed as part of the SEP3 semester project.  
The system supports dairy farm operations such as cow management, department transfers, milk collection, storage, sales, and traceability.

The solution was designed to improve food safety, operational control, and milk traceability from cow to customer.

---

## Project Overview

The system supports the following core use cases:

- user login with role-based access
- cow registration and management
- transfer of cows between departments
- veterinarian approval for quarantine release
- milk collection and storage registration
- customer and sales registration
- history and traceability-related operations

The project was developed as a **heterogeneous distributed system** using a **3-tier architecture**.

---

## Architecture

### Tier 1 – Presentation Layer
- **Blazor Server**
- Responsible for UI, navigation, role-based views, and client-side interaction

### Tier 2 – Application Layer
- **ASP.NET Core WebAPI**
- Exposes REST endpoints
- Handles authentication and authorization
- Maps REST DTOs to gRPC contracts
- Acts as a gateway between the frontend and the Java backend

### Tier 3 – Domain & Data Layer
- **Java Spring Boot**
- Contains core business logic
- Exposes gRPC services
- Handles persistence with JPA/Hibernate

### Database
- **PostgreSQL**
- Stores users, cows, departments, milk records, containers, customers, sales, and transfer records

---

## Technologies Used

### Frontend
- Blazor Server
- C#
- HTML / CSS

### Backend
- ASP.NET Core WebAPI
- Java Spring Boot
- gRPC
- Protocol Buffers

### Persistence
- PostgreSQL
- Spring Data JPA / Hibernate

### Security
- JWT authentication
- Role-based authorization
- BCrypt password hashing

### Testing
- JUnit
- Mockito
- Swagger
- BloomRPC
- Black-box testing

---

## Repository Structure

Example structure:

```text
WayOfMilk_SEP3/
│
├── C_Sharp/
│   ├── Client/
│   │   └── WoM_BlazorApp/          # Tier 1 - Blazor frontend
│   ├── Server/
│   │   └── WoM_WebApi/             # Tier 2 - REST API and gRPC gateway
│   ├── Shared/
│   │   ├── ApiContracts/           # Shared REST DTOs
│   │   └── WoM_Grpc/               # Shared gRPC-related code/contracts for C#
│   └── WayOfMilk.sln
│
├── Java/
│   ├── WoM_T3/                     # Tier 3 - Spring Boot backend with JPA and gRPC server
│   └── wom-grpc-contract/          # Shared Protocol Buffer contract module
│
└── README.md
```

---

## Main Features

- Role-based login and access control
- Owner, Worker, and Veterinarian roles
- Cow registration and updates
- Transfer of cows between departments
- Veterinarian approval for quarantine release
- Milk collection and container assignment
- Customer and sales management
- Basic history and traceability support
- Distributed communication between services

---

## Development Process

The project followed a hybrid development process combining:

- **Unified Process (UP)** for analysis, design, implementation, and testing
- **Kanban** for flexible task management during execution

Collaboration and coordination were supported through:

- **Discord** for communication and decision logging
- **GitHub** for version control and branching
- **Kanban board** for task tracking and progress visibility

---

## Setup Instructions

### Prerequisites

Make sure the following are installed:

- .NET SDK
- Java JDK
- Maven
- PostgreSQL
- Git

Optional tools:

- Swagger for REST testing
- BloomRPC for gRPC testing

---

## 1. Clone the repository

```bash
git clone https://github.com/Waqarahmedkhan96/WayOfMilk_Sep3.git
cd WayOfMilk_Sep3
```

If you already have the repository:

```bash
git pull
```

---

## 2. Build the shared gRPC contract and backend projects

First, build the Java gRPC contract module:

```bash
cd Java/wom-grpc-contract
mvn clean install
```

Then build the Java backend:

```bash
cd ../WoM_T3
mvn clean install
```

Then build the shared C# gRPC project:

```bash
cd ../../C_Sharp/Shared/WoM_Grpc
dotnet build
```

---

## 3. Configure the database

Open the Java backend configuration file:

```text
Java/WoM_T3/src/main/resources/application.properties
```

Update the PostgreSQL connection settings to match your local setup.

What to update:
- `spring.datasource.url` → keep this format, but change it if your PostgreSQL host, port, or database is different
- `spring.datasource.username` → your local PostgreSQL username
- `spring.datasource.password` → your local PostgreSQL password

Example configuration:

```properties
spring.datasource.url=jdbc:postgresql://localhost:5432/postgres?currentSchema=wayofmilk
spring.datasource.username=username
spring.datasource.password=password
```

If needed, you can keep the schema part exactly as:

```text
currentSchema=wayofmilk
```

---

## 4. Create the database schema

Open PostgreSQL and create the schema manually:

```sql
CREATE SCHEMA wayofmilk;
```

#### !!! This schema must exist before starting the backend. !!!

After that, when the application runs, the tables will be created automatically by Hibernate/JPA.

---

## 5. Start Tier 3 – Java backend

Go to the Java backend folder:

```bash
cd Java/WoM_T3
mvn spring-boot:run
```

This starts the Java Spring Boot backend and the gRPC server.

---

## 6. Start Tier 2 – ASP.NET Core WebAPI

Open a new terminal and run:

```bash
cd C_Sharp/Server/WoM_WebApi
dotnet run
```

This starts the WebAPI layer.

---

## 7. Start Tier 1 – Blazor application

Open another terminal and run:

```bash
cd C_Sharp/Client/WoM_BlazorApp
dotnet run
```

Then open the local address shown in the terminal in your browser. Most likely at: 

```text
https://localhost:5075
```

---

## Setup Summary

The recommended startup order is:

1. Clone or pull the repository
2. Build `Java/wom-grpc-contract`
3. Build `Java/WoM_T3`
4. Build `C_Sharp/Shared/WoM_Grpc`
5. Configure PostgreSQL in `application.properties`
6. Create schema `wayofmilk`
7. Start Java backend
8. Start WebAPI
9. Start Blazor app

---

## Communication Flow

The system uses two communication styles:

### Blazor → WebAPI
- REST
- JSON DTOs
- JWT-based authentication

### WebAPI → Java Backend
- gRPC
- Protocol Buffers
- Shared strongly typed contracts

---

## Security Notes

The current implementation includes:

- JWT authentication
- role-based authorization
- password hashing using BCrypt

Known limitations:

- HTTPS/TLS was considered in the architecture, but not all secure communication paths were fully finalized in the final development phase

---

## Testing

The project was tested using multiple approaches:

### Unit Testing
- Java service layer tested with JUnit and Mockito

### REST Testing
- Swagger

### gRPC Testing
- BloomRPC

### UI / Functional Testing
- Black-box test cases based on use cases

---

## Known Limitations

The current version reflects the final academic project submission and includes some limitations:

- not all initially planned use cases were fully completed
- some traceability functionality remains partial or mock-based
- some history-related functionality is limited
- security hardening can be improved further
- HTTPS/TLS is not fully completed across all communication paths

---

## Future Improvements

Possible future work includes:

- full end-to-end milk traceability
- stronger HTTPS/TLS enforcement
- improved audit/history functionality
- better observability and logging
- improved filtering and reporting
- additional validation and automated testing

---

## Authors

SEP3 Group 5

- Ana Maria Patriche
- Mara-Ioana Statie
- Piotr Gala
- Waqar Ahmed Khan

Supervisors:

- Jakob Trigger Knop
- Joseph Chukwudi Okika

---

## Academic Context

This project was developed for the **Software Technology Engineering** programme as part of the **SEP3 semester project**.

---

## License

This repository was created for academic purposes.
