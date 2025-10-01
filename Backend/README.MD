
-------- PROJETO BACKAND ------------


🏗️ Arquitetura Clean Architecture + CQRS
Domain Layer: Entidades, Interfaces, Exceptions
Application Layer: Commands, Queries, Handlers, DTOs, Validators
Infrastructure Layer: EF Core, Repositories, Services
API Layer: Controllers, Middleware, Configuration



🔒 Segurança Completa
JWT Authentication implementado
Password Hashing com BCrypt
Email único validação
Middleware de Exception handling
CORS configurado

📊 Banco de Dados
MariaDB com EF Core
Migrations criadas e funcionais
Repository Pattern implementado
Connection String configurada

🚀 Endpoints da API
POST /api/auth/register - Cadastro de usuários
POST /api/auth/login - Autenticação JWT
GET /api/users - Listagem de usuários (protegida)
GET /api/users/{id} - Obter usuário (protegida)

🧪 Testes Implementados
4 testes unitários criados e funcionais
Moq e FluentAssertions configurados
Testes passando com 100% de sucesso
🐳 Docker Ready
Dockerfile multi-stage build criado
docker-compose.yml configurado
Pronto para deploy em containers
📚 Documentação
Swagger/OpenAPI integrado
README.md completo com instruções
appsettings configurado para desenvolvimento e produção