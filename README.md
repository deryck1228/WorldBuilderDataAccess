# WorldBuilderMVC

WorldBuilderMVC is an ASP.NET MVC application designed to help you organize your worldbuilding for your tabletop role-playing game group. This project showcases various .NET skills including Entity Framework Core, Azure Functions, Azure API Management, Azure Service Bus, and ASP.NET MVC Websites.

## Features

- Create, view, edit, and delete characters
- Organize campaign locations
- Secure authentication using Azure AD
- API Management using Azure API Management
- Background processing using Azure Service Bus

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or later
- Azure subscription (for Azure SQL, Functions, API Management, and Service Bus)
- SQL Server Management Studio (SSMS) or Azure Data Studio (optional, for database management)

## Getting Started

### Setting Up the Database

1. Create an Azure SQL Server and a database.
2. Configure your SQL Server and database in Azure.
3. Update the connection string in `local.settings.json` for Azure Functions and in `appsettings.json` for the MVC project.

### Setting Up Azure Functions

1. Create an Azure Function App.
2. Deploy the `WorldBuilderFunctions` project to your Azure Function App.
3. Ensure that the connection string to your Azure SQL Database is set correctly in the Azure Function App's application settings.

### Setting Up the MVC Application

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/WorldBuilderMVC.git
