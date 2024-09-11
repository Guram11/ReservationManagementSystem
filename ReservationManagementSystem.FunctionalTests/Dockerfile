FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -c Release

ENTRYPOINT ["dotnet", "test", "--logger:console"]