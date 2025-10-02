# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/UserManagement.Api/UserManagement.Api.csproj", "src/UserManagement.Api/"]
COPY ["src/UserManagement.Application/UserManagement.Application.csproj", "src/UserManagement.Application/"]
COPY ["src/UserManagement.Domain/UserManagement.Domain.csproj", "src/UserManagement.Domain/"]
COPY ["src/UserManagement.Infrastructure/UserManagement.Infrastructure.csproj", "src/UserManagement.Infrastructure/"]

RUN dotnet restore "src/UserManagement.Api/UserManagement.Api.csproj"

# Copy source code
COPY . .
WORKDIR "/src/src/UserManagement.Api"

# Build and publish
RUN dotnet build "UserManagement.Api.csproj" -c Release -o /app/build
RUN dotnet publish "UserManagement.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]