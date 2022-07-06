# Description
This is study project. The primary goal is to learn how to create a backend .NET application with a separate SPA frontend so that they are completely decoupled and secure them with the external OAuth Provider.  

There are many tutorials on the Web on how to connect the .NET backend and SPA frontend. However, most of them are when either the frontend application located inside of the backend project and has some dependencies or the backend contains some cshtml pages. Even in the [latest Microsoft's tutorial](https://docs.microsoft.com/en-us/visualstudio/javascript/tutorial-asp-net-core-with-react?view=vs-2022) the backend and the frontend are not 100% separate.

Morever, there are even less guides on how to add external authentication provider to such system. I wanted to separate the backend and the frontend so different people can develop them independtly although the project has been done solely by myself. 

## Technologies used
### Backend
- .NET 6.0
- MS SQL Server running in Docker
- Google Authentication Provider
- Entity Framework Core
- Serilog for logging
- AspNetCore Authentication
- Swagger

### Frontend
- React v.17
- React Bootstrap
- React Hooks, functional components
-  Axios

## Architecture

![image](https://user-images.githubusercontent.com/14952031/177486475-f3b86b2d-4409-4f01-82a0-8877a3f1e99f.png)

- Domain - core of the system, where the domain entities reside. Other projects depend on the Domain. Domain does not depend on anything*.
- Common - a storage for common classes, such as the exceptions, etc. that can be used in both API and Infrastructure.
- Infrastructure - the code to interact with the Database.
- API - backend application
- React SPA - frontend application

![image](https://user-images.githubusercontent.com/14952031/177483777-30040002-fd3e-4b2b-9760-361eaf5078c0.png)




## Google OAuth
To start using google authentication first create project in [Google Cloud Provider (GCP)](https://console.cloud.google.com/)

and get the credentials
![image](https://user-images.githubusercontent.com/14952031/177108480-650550da-a354-4f3b-8e95-28f0dc888532.png)


# Initialize development environment
## dotnet secrets

Initializing the secret storage (it's already has been initialized, no need to run):  
```dotnet user-secrets init```

Set the ClientId and Secret:  
```dotnet user-secrets set "Authentication:Google:ClientId" "Your_ClientId"```

```dotnet user-secrets set "Authentication:Google:ClientSecret" "Your_Secret"```

## Database
Install packages:  
`dotnet tool install --global dotnet-ef`  
`dotnet add package Microsoft.EntityFrameworkCore.Design`

Create Migrations:  
`cd src/EduPlan.ChatApp.Infrastructure` & `dotnet ef migrations add InitialCreate -s ..\EduPlan.ChatApp.Api\EduPlan.ChatApp.Api.csproj --verbose`

Update database:  
`dotnet ef database update -s ..\EduPlan.ChatApp.Api\EduPlan.ChatApp.Api.csproj`
