Rem Start docker
docker compose -f %ChatAppDir%\docker\docker-compose.yml up -d

Rem Start backend
dotnet run --project %ChatAppDir%\src\EduPlan.ChatApp.Api\EduPlan.ChatApp.Api.csproj