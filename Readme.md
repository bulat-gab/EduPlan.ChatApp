This is study project

# Create project in Google Cloud Provider (GCP)

## Get the credentials
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
