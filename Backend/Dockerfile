# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/UserManagement.Api/UserManagement.Api.csproj", "src/UserManagement.Api/"]
COPY ["src/UserManagement.Application/UserManagement.Application.csproj", "src/UserManagement.Application/"]
COPY ["src/UserManagement.Domain/UserManagement.Domain.csproj", "src/UserManagement.Domain/"]
COPY ["src/UserManagement.Infrastructure/UserManagement.Infrastructure.csproj", "src/UserManagement.Infrastructure/"]

RUN dotnet restore "src/UserManagement.Api/UserManagement.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/UserManagement.Api"
RUN dotnet build "UserManagement.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "UserManagement.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .

# Install wait-for-it script for database dependency
RUN apt-get update && apt-get install -y wget && \
    wget https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh && \
    chmod +x wait-for-it.sh

ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]