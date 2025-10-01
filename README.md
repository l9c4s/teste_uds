# Teste Prático - Desenvolvedor Fullstack

Este projeto é uma aplicação fullstack completa desenvolvida com:
- **Backend**: .NET 9 API com Clean Architecture e CQRS
- **Frontend**: Angular 19 com modularização e segurança
- **Banco de Dados**: MariaDB
- **Containerização**: Docker e Docker Compose

## 🏗️ Estrutura do Projeto

```
teste_uds/
├── Infra/                  # Configuração do banco de dados
│   ├── Dockerfile         # MariaDB container
│   └── init.sql          # Script de inicialização do BD
├── Backend/              # API .NET 9
│   ├── src/              # Código fonte
│   ├── tests/            # Testes automatizados
│   └── Dockerfile        # Container da API
├── Frontend/             # Aplicação Angular 19
│   ├── src/              # Código fonte
│   └── Dockerfile        # Container do frontend
├── docker-compose.yml    # Orquestração dos containers
└── README.md            # Este arquivo
```

## 🚀 Como Executar

### Pré-requisitos
- Docker Desktop instalado
- Git (para clonar o repositório)

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd teste_uds
```

### 2. Execute com Docker Compose
```bash
# Construir e iniciar todos os serviços
docker-compose up --build

# Ou executar em background
docker-compose up --build -d
```

### 3. Aguarde a inicialização
- **Banco de dados**: http://localhost:3306
- **Backend API**: http://localhost:5000
- **Frontend**: http://localhost:4200

### 4. Verificar se tudo está funcionando
- Acesse http://localhost:4200 no navegador
- A aplicação deve carregar a tela inicial

## 🔗 URLs dos Serviços

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:5000
- **MariaDB**: localhost:3306
- **Swagger/OpenAPI**: http://localhost:5000/swagger (após iniciar o backend)

## 📋 Funcionalidades

### Backend
- ✅ POST /api/users - Cadastro de usuários
- ✅ GET /api/users - Listagem de usuários
- ✅ POST /api/auth/login - Autenticação JWT
- ✅ Arquitetura limpa com CQRS
- ✅ Validação de dados
- ✅ Segurança JWT
- ✅ EF Core com MariaDB
- ✅ Testes automatizados

### Frontend
- ✅ Tela de cadastro de usuários
- ✅ Tela de listagem de usuários
- ✅ Tela de login
- ✅ Interceptor para JWT
- ✅ Guard para proteção de rotas
- ✅ Modularização por funcionalidade
- ✅ Testes unitários

## 🛠️ Desenvolvimento Local

### Backend (.NET 9)
```bash
cd Backend
dotnet restore
dotnet build
dotnet run --project src/Api
```

### Frontend (Angular 19)
```bash
cd Frontend
npm install
ng serve
```

### Banco de Dados (MariaDB)
```bash
cd Infra
docker build -t mariadb-users .
docker run -p 3306:3306 mariadb-users
```

## 🧪 Executar Testes

### Backend
```bash
cd Backend
dotnet test
```

### Frontend
```bash
cd Frontend
npm test
```

## 🔧 Comandos Úteis

### Docker
```bash
# Ver logs dos containers
docker-compose logs

# Parar todos os serviços
docker-compose down

# Remover volumes (limpar banco de dados)
docker-compose down -v

# Reconstruir apenas um serviço
docker-compose up --build backend
```

### Banco de Dados
```bash
# Conectar ao MariaDB
docker exec -it mariadb_users mysql -u apiuser -p usersdb
```

## 📚 Documentação da API

Após iniciar o backend, acesse:
- **Swagger UI**: http://localhost:5000/swagger
- **OpenAPI JSON**: http://localhost:5000/swagger/v1/swagger.json

## 🔐 Usuários de Teste

Por padrão, o sistema será criado sem usuários. Use a tela de cadastro ou os endpoints da API para criar usuários.

### Exemplo de usuário para teste:
```json
{
  "name": "João Silva",
  "email": "joao@teste.com",
  "password": "123456"
}
```

## 🏛️ Arquitetura

### Backend - Clean Architecture
- **Domain**: Entidades e regras de negócio
- **Application**: Casos de uso, CQRS handlers
- **Infrastructure**: EF Core, repositórios
- **API**: Controllers, middlewares

### Frontend - Modular
- **AuthModule**: Autenticação e guards
- **UsersModule**: Gestão de usuários
- **SharedModule**: Componentes compartilhados
- **CoreModule**: Serviços principais

## 🔒 Segurança Implementada

- ✅ JWT Authentication
- ✅ Password hashing (bcrypt)
- ✅ Email único validation
- ✅ Input sanitization
- ✅ CORS configurado
- ✅ Route guards no frontend
- ✅ HTTP interceptors

## 📝 Logs e Monitoramento

Os logs são exibidos no console do Docker. Para ver logs específicos:

```bash
# Logs do backend
docker-compose logs backend

# Logs do frontend
docker-compose logs frontend

# Logs do banco
docker-compose logs database
```

## 🐛 Troubleshooting

### Problema: Container não inicia
```bash
# Verificar logs
docker-compose logs [service-name]

# Reconstruir containers
docker-compose down && docker-compose up --build
```

### Problema: Banco não conecta
```bash
# Verificar se o MariaDB está rodando
docker-compose ps

# Verificar logs do banco
docker-compose logs database
```

### Problema: Frontend não carrega
```bash
# Verificar se a API está respondendo
curl http://localhost:5000/api/health

# Verificar logs do frontend
docker-compose logs frontend
```

## 👨‍💻 Desenvolvido por

Este projeto foi desenvolvido como parte de um teste prático para demonstrar habilidades em:
- Arquitetura de software
- Clean Architecture e CQRS
- Segurança em APIs
- Containerização
- Testes automatizados
- Boas práticas de desenvolvimento

---
**Tecnologias**: .NET 9, Angular 19, MariaDB, Docker