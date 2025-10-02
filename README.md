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
- **Backend API**: http://localhost:5252
- **Frontend**: http://localhost:3000

### 4. Verificar se tudo está funcionando
- Acesse http://localhost:3000 no navegador
- A aplicação deve carregar a tela inicial

 - Logar
 - Usuario:  admin@sistema.com
 - Password: admin123


## se por algum destino que deus quiz na hora de logar der usuario e senha incorretos.

1 - Rode o teste no backand 'HashPassword_ShouldGenerateValidHash'
2 - pegue o valor do hash gerado no output do teste
3 - Conecte no banco de dados e execute o update
4 - UPDATE Users SET PasswordHash = 'hash do output do teste' WHERE Id = 1;



## 👨‍💻 Desenvolvido por Lucas carvalho

Este projeto foi desenvolvido como parte de um teste prático para demonstrar habilidades em:
- Arquitetura de software
- Clean Architecture e CQRS
- Segurança em APIs
- Containerização
- Testes automatizados
- Boas práticas de desenvolvimento

---
**Tecnologias**: .NET 9, Angular 19, MariaDB, Docker