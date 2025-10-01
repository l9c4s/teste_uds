# Próximas Etapas - Desenvolvimento

## 1. Backend (.NET 9) - Clean Architecture + CQRS

### Estrutura de pastas do Backend:
```
Backend/
├── src/
│   ├── Domain/
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   └── ValueObjects/
│   ├── Application/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   ├── DTOs/
│   │   └── Interfaces/
│   ├── Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   └── Services/
│   └── Api/
│       ├── Controllers/
│       ├── Middleware/
│       └── Program.cs
├── tests/
│   ├── Domain.Tests/
│   ├── Application.Tests/
│   └── Api.Tests/
└── Dockerfile
```

### Pacotes NuGet necessários:
- Microsoft.EntityFrameworkCore.Design
- Pomelo.EntityFrameworkCore.MySql
- Microsoft.AspNetCore.Authentication.JwtBearer
- BCrypt.Net-Next
- FluentValidation.AspNetCore
- MediatR
- AutoMapper
- Swashbuckle.AspNetCore

### Endpoints a implementar:
- POST /api/auth/register - Cadastro de usuário
- POST /api/auth/login - Autenticação
- GET /api/users - Listar usuários (protegida)
- GET /api/users/{id} - Obter usuário (protegida)

## 2. Frontend (Angular 19)

### Estrutura de pastas do Frontend:
```
Frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/
│   │   │   ├── interceptors/
│   │   │   └── services/
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   └── models/
│   │   ├── features/
│   │   │   ├── auth/
│   │   │   │   ├── login/
│   │   │   │   └── register/
│   │   │   └── users/
│   │   │       └── user-list/
│   │   └── app.module.ts
│   ├── environments/
│   └── assets/
├── Dockerfile
└── package.json
```

### Pacotes NPM necessários:
- @angular/material ou Bootstrap
- @angular/common/http
- rxjs

### Componentes a implementar:
- LoginComponent
- RegisterComponent
- UserListComponent
- AuthGuard
- AuthInterceptor

## 3. Testes

### Backend:
- UserCreateCommandHandlerTests
- AuthServiceTests
- UserRepositoryTests

### Frontend:
- UserListComponentTests
- AuthServiceTests
- AuthGuardTests

## 4. Docker

### Dockerfiles necessários:
- Backend/Dockerfile (multi-stage build)
- Frontend/Dockerfile (nginx)
- Infra/Dockerfile (já criado)

## 5. Ordem de implementação sugerida:

1. **Backend primeiro** (Domain → Application → Infrastructure → API)
2. **Testes do Backend**
3. **Frontend** (Core → Features → Shared)
4. **Testes do Frontend**
5. **Dockerfiles**
6. **Teste final com docker-compose**

## 6. Validações importantes:

### Backend:
- Email único
- Password hash
- JWT token válido
- Validação de dados de entrada

### Frontend:
- Formulários reativos
- Validação de campos
- Interceptor funcionando
- Guard protegendo rotas

## 7. Configurações de ambiente:

### Backend appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=usersdb;User=apiuser;Password=apipassword;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpiryMinutes": 60
  }
}
```

### Frontend environment.ts:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```