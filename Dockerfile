# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY ReservationManagementSystem.sln . 
COPY ./ReservationManagementSystem.API/*.csproj ./ReservationManagementSystem.API/
COPY ./ReservationManagementSystem.Domain/*.csproj ./ReservationManagementSystem.Domain/
COPY ./ReservationManagementSystem.Application/*.csproj ./ReservationManagementSystem.Application/
COPY ./ReservationManagementSystem.Infrastructure/*.csproj ./ReservationManagementSystem.Infrastructure/
COPY ./ReservationManagementSystem.Application.Tests/*.csproj ./ReservationManagementSystem.Application.Tests/
COPY ./ReservationManagementSystem.Infrastructure.Tests/*.csproj ./ReservationManagementSystem.Infrastructure.Tests/
COPY ./ReservationManagementSystem.Api.Tests/*.csproj ./ReservationManagementSystem.Api.Tests/
COPY ./ReservationManagementSystem.FunctionalTests/*.csproj ./ReservationManagementSystem.FunctionalTests/

COPY Nuget.config ./

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source files
COPY . .

# Build the projects
RUN dotnet build --no-restore -c Release

# Stage 2: Publish the project
RUN dotnet publish --no-restore -c Release -o /app/publish

# Stage 3: Set up runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet", "ReservationManagementSystem.API.dll"]