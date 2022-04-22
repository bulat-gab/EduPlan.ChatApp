Rem Start docker
docker compose -f ..\docker\docker-compose.yml up -d

Rem Start backend
dotnet run --project ..\src\EduPlan.ChatApp.Api\EduPlan.ChatApp.Api.csproj

Rem Start frontend
npm start --prefix ..\src\eduplan.chatapp.react