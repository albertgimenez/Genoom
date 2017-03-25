# Genoom Simpsons Tree Sample

## Requisites
* Net core 1.0
* Sql Server or MongoDb servers
* IIS express or IIS to host and launch the website.

## Where to download the code
https://github.com/albertgimenez/Genoom

## Full documentation
You can locate it in the doc folder: https://github.com/albertgimenez/Genoom/tree/master/Genoom.Simpsons/doc

## Unsupported features (TODOs)
* MongoDb repository implementation
* Upload to Azure

## Application Settings and Configuration
In the Genoom.Simpsons.Web, open the file appSettings.json, some key points of it are:
* Decide wich strategy we want to use (database):
 *Sql: to use SQL Server, please check the SqlConnection settings as well.
 *MongoDb: to use MongoDb, please check the MongoDbConnection settings and you will need also to review the MongoDbConfig section as well.

```json
{
  "DbStrategy": "Sql",
  "ConnectionStrings": {
    "SqlConnection": "Server=.;Database=GenoomSimpsons;Trusted_Connection=True;MultipleActiveResultSets=true",
    "MongoDbConnection": "mongodb://localhost:27017"
  },
  "MongoDbConfig": {
    "Database": "GenoomSimpsons",
    "Collection": "SimpsonsFamilyTree"
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

 
## Startup and DI services registration
To change and configure the DI services injectedand the default api routes configuration check the Genoom.Simpsons.Web, open the file Startup.cs, some key points of it are:

*ConfigureServices: here we add and register the services in the DI, we rely on the new but very simple DI offered by Microsoft.
 *The Swagger Documentation service is added here
 *The repository (sql, mongodb) instance is added here.
* Configure: here we configure the services:
 *The Swagger path and endpoint
 *We set a default error controller for requests that are nt any of the valid controllers developed (for the 404 errors)

```c#
public void ConfigureServices(IServiceCollection services)
{
        ...
 
        // Swagger documentation API
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Info { Title = "Genoom Simpsons Tree", Version = "v1" });
        });
 
        // The database provider (strategy) to use to access the data.
        services.AddSingleton<IPeopleRepository>(Support.PeopleRepositoryFactory.Create(Configuration));
}
```
 
```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
        ...
 
        // Swagger API doc
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Genoom Simpsons Tree v1");
        });
 
        // The default routes, by default if does not exist (404) we want to provide a nice response.
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "Error404",
                template: "{*url}",
                defaults: new { controller = "Error", action = "Handle404" }
            );
        });
    }
}
```

## Routes
For this excersice is set that the api calls are direct to the controllers like /people instead of /api/v1/people
I’ve followed the route pattern, but if there is intention to continue evolving this project, we should change it to follow the Web Api good practice: /api/v{xx}/controller
This will make easier to version the api and handle changes on it while keeping compatibility backwards. 

There are two routes levels:
* Default routes: set in the Configure Method in Startup.cs
* Per controller routes: this allows clear and fine grained control over the routes.
For this exercise because there are few controllers I thought is the best option because it’s clear. You will see that the controller has this decorator in the class declaration:

```
[Route("[controller]")]
```

