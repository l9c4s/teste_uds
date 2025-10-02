# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura as dependências para aproveitar o cache do Docker
COPY src/UserManagement.Api/UserManagement.Api.csproj src/UserManagement.Api/
COPY src/UserManagement.Application/UserManagement.Application.csproj src/UserManagement.Application/
COPY src/UserManagement.Domain/UserManagement.Domain.csproj src/UserManagement.Domain/
COPY src/UserManagement.Infrastructure/UserManagement.Infrastructure.csproj src/UserManagement.Infrastructure/
RUN dotnet restore src/UserManagement.Api/UserManagement.Api.csproj

# Copia o restante do código-fonte
COPY src/ ./src/

# Publica a aplicação
RUN dotnet publish src/UserManagement.Api/UserManagement.Api.csproj -c Release -o /app/publish

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Instala o curl para o healthcheck
# A imagem base do aspnet não inclui curl por padrão
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copia a aplicação publicada do estágio de build
COPY --from=build /app/publish .

# Cria um usuário não-root para segurança
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]