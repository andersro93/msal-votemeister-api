FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS restore
WORKDIR /app

COPY MsalDemo.Backend.sln .
COPY MsalDemo.Backend.csproj .
RUN ["dotnet", "restore"]

FROM restore AS build
COPY . ./
RUN ["dotnet", "build", "--configuration", "Release"]

FROM build AS test
RUN ["dotnet", "test"]

FROM build AS publish
RUN ["dotnet", "publish", "--configuration", "Release", "--output", "artifacts", "MsalDemo.Backend.csproj"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /app

COPY --from=publish /app/artifacts /app
ENTRYPOINT ["dotnet", "MsalDemo.Backend.dll"]