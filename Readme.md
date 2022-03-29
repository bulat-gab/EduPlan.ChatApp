This is study project


# Initialize development environment
## dotnet secrets

Initializing the secret storage (it's already has been initialized, no need to run):  
```dotnet user-secrets init```

Set the ClientId and Secret:  
```dotnet user-secrets set "Authentication:Google:ClientId" "Your_ClientId"```

```dotnet user-secrets set "Authentication:Google:ClientSecret" "Your_Secret"```