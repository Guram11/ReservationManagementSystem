# Stage 1: Build the tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY *.sln .
COPY ./ReservationManagementSystem.API/*.csproj ./ReservationManagementSystem.API/
COPY ./ReservationManagementSystem.FunctionalTests/*.csproj ./ReservationManagementSystem.FunctionalTests/

# Restore dependencies
RUN dotnet restore

# Copy all files and build tests
COPY . .
WORKDIR /src/ReservationManagementSystem.FunctionalTests
RUN dotnet build --no-restore -c Release

# Stage 2: Run tests
FROM build AS testrunner
WORKDIR /src/ReservationManagementSystem.FunctionalTests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]