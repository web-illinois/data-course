# IT Partners README.MD file

## Summary: 

This is the new Program Information for IT Partners, used to organize programs, credentials, and courses.

## Production location: 

This consists of five applications:
* **ProgramInformationV2**: The Blazor server application to handle back-end functions. https://course.wigg.illinois.edu
* **ProgramInformationV2.Function**: The Function Application for the API. https://courseapi.wigg.illinois.edu.
* **ProgramInformationV2.Data**: The data access for all the processes
* **ProgramInformationV2.Search**: The data access for searching (accessing AWS OpenSearch Service)

Swagger API documentation is available at https://courseapi.wigg.illinois.edu/api/swagger/ui

## Development location: 

Currently, none. We do development on local machines, and will host temporary sites when end users need to see development work. 

## How to deploy to production/development: 

CI/CD 

## How to set up locally: 

Download and point to an empty AWS OpenSearch Service. The function code will install the necessary indicies, and the blazor app will install the database tables.  

You can get your IP address by using http://checkip.amazonaws.com/.

### Information about EF Core Tools:

Make sure the ProgramInformationV2 project is set up as the startup project before running the commands below:

``Add-Migration -Name {migration name} -Project ProgramInformationV2.Data``

``Update-Database -Project ProgramInformationV2.Data``

If you run into the issue "The certificate chain was issued by an authority that is not trusted.", then add **TrustServerCertificate=True** to the connection string.

## Code to delete the test items in the OpenSearch Service

``
POST /pcr2_courses/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_requirementsets/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_programs/_delete_by_query
{ "query": { "match": { "source": "test" } } }

POST /pcr2_staticcode/_delete_by_query
{ "query": { "match": { "source": "test" } } }
``

## SQL statement to delete items marked for deletion

``
  DELETE FROM dbo.FieldSources WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
  DELETE FROM dbo.Logs WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
  DELETE FROM dbo.SecurityEntries WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
  DELETE FROM dbo.TagSources WHERE SourceId IN (SELECT ID FROM Sources WHERE RequestDeletion = 1);
  DELETE FROM Sources WHERE RequestDeletion = 1;
``