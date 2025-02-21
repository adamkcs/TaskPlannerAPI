# Task Planner API

PLEASE USE DEVELOP BRANCH.

## Overview
The **Task Planner API** is a .NET 8 task management system backend that allows users to create, assign, and track tasks in a structured board format, similar to a Kanban system.

## Project Status
In development stage.

## Features
- **User Authentication & Authorization** using .NET Identity and JWT
- **Task Management** with CRUD operations
- **Board Management** for organizing tasks
- **Labels & Comments** for additional task details
- **SQLite Database Support** with Entity Framework Core
- **Dockerized Deployment** for scalable and portable usage
- **Swagger API Documentation** for easy API testing

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core
- **Database:** SQLite (can be swapped with SQL Server/PostgreSQL)
- **Authentication:** .NET Identity with JWT authentication
- **Containerization:** Docker
- **Frontend:** Angular (later)

## API Endpoints
See Swagger

### Prerequisites
Ensure you have the following installed:
- .NET 8 SDK
- Docker
- SQLite

### Running Locally
1. Clone the repository:
   ```sh
   git clone https://github.com/adamkcs/TaskPlannerAPI.git
   cd task-planner-api
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Run database migrations:
   ```sh
   dotnet ef database update
   ```
4. Start the API:
   ```sh
   dotnet run
   ```
5. Access Swagger UI at:
   ```sh
   http://localhost:8080/swagger
   ```

### Running with Docker
1. Build and start the container:
   ```sh
   docker-compose up --build
   ```
2. Access the API at `http://localhost:8080`

## Database Structure

The API follows a relational database structure with key models:

- **User** – Manages authentication and user roles
- **Board** – A collection of tasks
- **Task** – Assignable work items
- **Task List** – Assignable work items
- **Label** – Categorization tags
- **Comment** – Task discussions

## License
MIT License © 2025 Task Planner API
