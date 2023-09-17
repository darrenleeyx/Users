# Users API

## Description

A web api to create, delete and get users in memory. This project is built using .NET 7.

[API Documentation (deployed on heroku)](https://users-web-api-28d77317a8a1.herokuapp.com/swagger/index.html)

![Screenshot of Swagger UI](https://github.com/darrenleeyx/Users/assets/59044608/10d4ceee-16ae-417e-b573-8996fb502d2c)

## Projects
1. src/Users.Api -> An ASP.NET Core Web API Project
2. src/Users.Api.Contracts -> A class library project which contains all request and response contracts for all API endpoints
3. src/Users.Application -> A class library project which contains logic to get, create and delete users 
4. tests/Users.Api.Tests.Unit -> A xUnit project which contains unit tests for Users.Api
5. tests/Users.Application.Tests.Unit -> A xUnit project which contains unit tests for Users.Application

## Features
1. API endpoints to get, create and delete users in memory which is loaded from a static json file during applcation startup.
2. FluentValidation to validate all incoming requests and integrated with Swagger UI for clarity.
3. Serilog as the main logging provider to log (Enriched with correlation id for traceability):
    * Requests and responses using HttpLoggingMiddleware
    * Validation and unexpected exceptions thrown during runtime
6. Global exception handling middleware to handle all exceptions appropriately by returning 400/500 status codes and its problem details.
7. Unit Testing using xUnit, NSubstitute, Bogus, FluentAssertions.
