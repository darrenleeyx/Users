# Users API

## Description

An api to create, delete and get users from a static json file. This project is built using .NET 7.

[API Documentation (deployed on heroku)](https://users-web-api-28d77317a8a1.herokuapp.com/swagger/index.html)

![Screenshot of Swagger UI](https://github.com/darrenleeyx/Users/assets/59044608/10d4ceee-16ae-417e-b573-8996fb502d2c)

## Projects
- src/Users.Api -> An ASP.NET Core Web API Project
- src/Users.Api.Contracts -> A class library which contains all request and response contracts for all API endpoints
- src/Users.Application -> A class library which contains logic to get, create and delete users 
- tests/Users.Api.Tests.Unit -> A xUnit project which contains unit tests for Users.Api
- tests/Users.Application.Tests.Unit -> A xUnit project which contains unit tests for Users.Application

## Features
1. API endpoints to get, create and delete users from memory (loaded from static json file when application starts)
2. FluentValidation to validate all incoming requests and integrated with Swagger UI
3. Serilog as logging provider to log all requests and responses through UseHttpLogging middleware to console and file
4. Custom global exception handling middleware to handle validation and unexpected exceptions
5. Unit Testing using xUnit, NSubstitute, Bogus, FluentAssertions

## Installation

Provide step-by-step instructions on how to set up and run your .NET Web API project locally. Include any prerequisites and dependencies that need to be installed.

```bash
# Clone the repository
git clone https://github.com/darrenleeyx/Users.git

# Change to the project directory
cd Users

# Install dependencies
dotnet restore

# Build the project
dotnet build

# Run the API
dotnet run
