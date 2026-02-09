# ProgramInformationV2

A comprehensive Blazor-based management system for organizing and maintaining educational programs, credentials, courses, and requirement sets.

## Overview

ProgramInformationV2 is a modern web application designed to manage academic program information, course catalogs, credentials, and associated data. The system provides a complete solution for educational institutions to organize, search, and maintain their course and program offerings.

## Architecture

The solution consists of four main projects:

### **ProgramInformationV2** (Main Blazor Application)
- **Technology**: ASP.NET Core 8.0 Blazor Server
- **Purpose**: Primary web interface for managing programs, credentials, courses, and sections
- **Authentication**: Microsoft Identity (Azure AD/Entra ID)
- **Production URL**: https://course.wigg.illinois.edu
- **Key Features**:
  - Interactive Blazor components for data management
  - Role-based security and access control
  - Rich text editing capabilities
  - File upload and image management
  - Audit logging and reporting
  - Course import functionality
  - Advanced filtering and search

### **ProgramInformationV2.Function** (API Layer)
- **Technology**: Azure Functions v4 (.NET 8.0)
- **Purpose**: RESTful API for data access and integrations
- **Production URL**: https://courseapi.wigg.illinois.edu
- **Documentation**: Swagger UI available at https://courseapi.wigg.illinois.edu/api/swagger/ui
- **Features**:
  - OpenAPI/Swagger documentation
  - CORS-enabled endpoints
  - Integration with SQL Server and OpenSearch

### **ProgramInformationV2.Data** (Data Access Layer)
- **Technology**: .NET 8.0 Class Library
- **Purpose**: Entity Framework Core data access and business logic
- **Features**:
  - SQL Server database integration
  - Entity Framework Core migrations
  - Azure Blob Storage integration for file uploads
  - Course import helpers
  - Cache management
  - Field validation and management

### **ProgramInformationV2.Search** (Search Layer)
- **Technology**: .NET 8.0 Class Library
- **Purpose**: Search functionality using AWS OpenSearch Service
- **Features**:
  - OpenSearch client integration
  - AWS SigV4 authentication
  - Index management for courses, programs, credentials, requirement sets, and static codes
  - Search helpers and query builders

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server (Express or higher)
- AWS OpenSearch Service instance
- Azure Storage Account (for file uploads)
- Azure AD tenant (for authentication)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/web-illinois/data-course.git
   cd data-course
   ```

2. **Configure User Secrets**
   
   Set the ProgramInformationV2 project as the startup project, then configure your connection strings and secrets:
   
   ```bash
   dotnet user-secrets set "ConnectionStrings:AppConnection" "Server=YOUR_SERVER;Database=ProgramInfoV2;Trusted_Connection=True;TrustServerCertificate=True" --project ProgramInformationV2
   dotnet user-secrets set "SearchUrl" "YOUR_OPENSEARCH_URL" --project ProgramInformationV2
   dotnet user-secrets set "AzureStorage" "YOUR_STORAGE_URL" --project ProgramInformationV2
   dotnet user-secrets set "AzureAccountName" "YOUR_ACCOUNT_NAME" --project ProgramInformationV2
   dotnet user-secrets set "AzureAccountKey" "YOUR_ACCOUNT_KEY" --project ProgramInformationV2
   dotnet user-secrets set "AzureImageContainerName" "YOUR_CONTAINER_NAME" --project ProgramInformationV2
   ```

3. **Configure OpenSearch Access**
   
   Ensure your IP address is whitelisted in the AWS OpenSearch Service security group. You can check your IP at http://checkip.amazonaws.com/

4. **Initialize the Database**
   
   The application will automatically create database tables on first run. To manually manage migrations:
   
   ```bash
   # Create a new migration
   Add-Migration -Name YourMigrationName -Project ProgramInformationV2.Data
   
   # Apply migrations to database
   Update-Database -Project ProgramInformationV2.Data
   ```

5. **Run the Application**
   
   Set ProgramInformationV2 as the startup project and press F5 to run.

### Troubleshooting Common Setup Issues

- **Certificate Trust Error**: Add `TrustServerCertificate=True` to your SQL Server connection string
- **OpenSearch Connection**: Verify your IP is whitelisted and the SearchUrl is correct
- **Azure AD Authentication**: Ensure your AzureAd configuration in appsettings.json matches your tenant

## ?? Entity Framework Core Migrations

Always ensure **ProgramInformationV2** is set as the startup project when running EF Core commands:

```powershell
# Add a new migration
Add-Migration -Name MigrationName -Project ProgramInformationV2.Data

# Update database to latest migration
Update-Database -Project ProgramInformationV2.Data

# Rollback to a specific migration
Update-Database -Migration MigrationName -Project ProgramInformationV2.Data
```

## OpenSearch Management

### Index Structure
- `pcr2_courses` - Course catalog data
- `pcr2_programs` - Academic programs
- `pcr2_requirementsets` - Requirement sets and course groupings
- `pcr2_staticcode` - Static code lookups
- `pcr2_credentials` - Credential information

### Delete Test Data

Use these OpenSearch queries to remove test data:

```json
POST /pcr2_courses/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_requirementsets/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_programs/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_staticcode/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_credentials/_delete_by_query
{ "query": { "match": { "source": "test" } } }
```

## Database Maintenance

### Delete Sources Marked for Deletion

```sql
DELETE FROM dbo.FieldSources WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
DELETE FROM dbo.Logs WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
DELETE FROM dbo.SecurityEntries WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
DELETE FROM dbo.TagSources WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
DELETE FROM Sources WHERE RequestDeletion = 1;
```

## Deployment

### Production Deployment
- **Method**: CI/CD pipeline (automated)
- **Blazor App**: Deployed to Azure App Service
- **Azure Functions**: Deployed to Azure Functions hosting
- **Database**: Azure SQL Database or SQL Server
- **Search**: AWS OpenSearch Service
- **Storage**: Azure Blob Storage

### Development/Staging
Development is primarily done on local machines. Temporary staging sites can be deployed when end users need to review work in progress.

## Key Features

- **Program Management**: Create and manage academic programs with credentials and courses
- **Credential Management**: Define credentials, requirements, and course associations
- **Course Catalog**: Comprehensive course information including sections, faculty, schedules
- **Requirement Sets**: Build and manage curriculum requirements and course groupings
- **Search Functionality**: Advanced search across programs, courses, credentials using OpenSearch
- **Security & Access Control**: Role-based security with source-level permissions
- **Audit Trail**: Complete logging of changes and user actions
- **Course Import**: Import course data from external systems
- **Filters**: Advanced filtering capabilities for courses and programs
- **Configuration Management**: JSON import/export for backup and migration

## Technology Stack

- **Frontend**: Blazor Server (ASP.NET Core 8.0)
- **Backend**: Azure Functions v4
- **Database**: SQL Server with Entity Framework Core 8.0
- **Search**: AWS OpenSearch Service (with OpenSearch.Client 1.8.0)
- **Authentication**: Microsoft Identity Web (Azure AD)
- **Storage**: Azure Blob Storage
- **UI Components**: Illinois Web Components, Blazored TextEditor
- **Bundling**: WebOptimizer

## Project Structure

```
ProgramInformationV2/
??? Components/
?   ??? Controls/          # Reusable Blazor components
?   ??? Layout/            # Layout components (MainLayout, Sidebar, etc.)
?   ??? Pages/             # Page components organized by feature
?       ??? Course/        # Course management pages
?       ??? Program/       # Program management pages
?       ??? Credential/    # Credential management pages
?       ??? RequirementSet/# Requirement set pages
?       ??? Configuration/ # Admin configuration
?       ??? Audit/         # Audit and reporting
?       ??? FieldsUsed/    # Field usage tracking
??? Controllers/           # MVC controllers
??? Helpers/              # Utility helpers

ProgramInformationV2.Data/
??? DataContext/          # EF Core DbContext
??? DataHelpers/          # Data access helpers
??? Migrations/           # EF Core migrations
??? Models/               # Entity models

ProgramInformationV2.Search/
??? Models/               # Search model definitions
??? Getters/              # Search query helpers
??? Setters/              # Index update helpers

ProgramInformationV2.Function/
??? Functions/            # Azure Function endpoints
```

## Contributing

This is a project by IT Partners at the University of Illinois. For contribution guidelines or questions, please contact the development team.

## Support

For issues or questions:
- Check the Swagger API documentation at https://courseapi.wigg.illinois.edu/api/swagger/ui
- Contact the IT Partners development team
- Review audit logs in the application for troubleshooting