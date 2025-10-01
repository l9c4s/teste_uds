# User Management Frontend - Angular 19

Este é o frontend da aplicação de gerenciamento de usuários, desenvolvido em Angular 19 com Angular Material.

## 🚀 Tecnologias Utilizadas

- **Angular 19** - Framework principal
- **Angular Material** - Biblioteca de componentes UI
- **TypeScript** - Linguagem de programação
- **RxJS** - Programação reativa
- **SCSS** - Pré-processador CSS

## 📁 Estrutura do Projeto

```
src/
├── app/
│   ├── core/                 # Serviços principais, guards e interceptors
│   │   ├── guards/           # Guards de autenticação
│   │   ├── interceptors/     # Interceptors HTTP
│   │   └── services/         # Serviços da aplicação
│   ├── shared/               # Componentes e modelos compartilhados
│   │   ├── components/       # Componentes reutilizáveis
│   │   └── models/           # Interfaces e modelos de dados
│   ├── features/             # Funcionalidades da aplicação
│   │   ├── auth/             # Módulo de autenticação
│   │   │   ├── login/        # Componente de login
│   │   │   └── register/     # Componente de registro
│   │   └── users/            # Módulo de usuários
│   │       └── user-list/    # Lista de usuários
│   ├── app.component.ts      # Componente raiz
│   ├── app.config.ts         # Configurações da aplicação
│   └── app.routes.ts         # Rotas da aplicação
├── environments/             # Arquivos de ambiente
└── assets/                   # Recursos estáticos
```

## 🎯 Funcionalidades

### ✅ Implementadas
- **Autenticação**
  - Login de usuários
  - Registro de novos usuários
  - Logout
  - Guard de proteção de rotas
  - Interceptor para tokens JWT

- **Gerenciamento de Usuários**
  - Listagem de usuários
  - Visualização de detalhes
  - Controle de acesso baseado em roles

- **Interface**
  - Design responsivo com Angular Material
  - Formulários reativos com validação
  - Notificações com MatSnackBar
  - Navegação intuitiva

### 🔄 Em Desenvolvimento
- Edição de usuários
- Criação de novos usuários (admin)
- Filtros e busca na lista
- Paginação
- Testes unitários completos

## 🛠 Configuração e Instalação

### Pré-requisitos
- Node.js 18+ 
- npm ou yarn
- Angular CLI 19

### Instalação
```bash
# Instalar dependências
npm install

# Instalar Angular CLI globalmente (se necessário)
npm install -g @angular/cli@19
```

### Executar em Desenvolvimento
```bash
# Servidor de desenvolvimento
npm start
# ou
ng serve

# Aplicação estará disponível em http://localhost:4200
```

### Build para Produção
```bash
# Build de produção
npm run build
# ou
ng build --configuration production
```

### Executar Testes
```bash
# Testes unitários
npm test
# ou
ng test

# Testes e2e
npm run e2e
# ou
ng e2e
```

## 🐳 Docker

### Build da Imagem
```bash
docker build -t user-management-frontend .
```

### Executar Container
```bash
docker run -p 80:80 user-management-frontend
```

## ⚙️ Configuração

### Ambientes
Os arquivos de ambiente estão localizados em `src/environments/`:

- `environment.ts` - Desenvolvimento
- `environment.prod.ts` - Produção

### Variáveis de Ambiente
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

## 🔐 Autenticação e Autorização

A aplicação utiliza autenticação JWT com sistema completo de níveis de acesso:

- **AuthService**: Gerencia login, logout e estado de autenticação
- **RoleService**: Decodifica tokens JWT e gerencia permissões
- **AuthGuard**: Protege rotas que requerem autenticação
- **AdminGuard/ManagerGuard**: Protege rotas por nível de acesso
- **AuthInterceptor**: Adiciona automaticamente o token JWT nas requisições HTTP
- **HasPermissionDirective**: Controla visibilidade de elementos baseada em permissões

### Níveis de Acesso (Hierárquicos)
1. **Administrator** - Acesso total ao sistema
   - Pode gerenciar usuários, roles e sistema
   - Acesso a todas as funcionalidades

2. **Manager** - Gerência de usuários
   - Pode criar, editar e excluir usuários
   - Pode visualizar relatórios e editá-los
   - Não pode gerenciar configurações do sistema

3. **CommonUser** - Usuário comum
   - Pode visualizar usuários e relatórios
   - Não pode criar, editar ou excluir usuários
   - Não pode editar relatórios

4. **Viewer** - Apenas visualização
   - Pode apenas visualizar usuários e relatórios
   - Nenhuma permissão de edição

### Fluxo de Autenticação
1. Usuário faz login com email e senha
2. Backend retorna JWT token com role do usuário
3. Token é decodificado para extrair informações do usuário
4. Sistema determina permissões baseadas na hierarquia de roles
5. Guards e diretivas controlam acesso baseado nas permissões

### Sistema de Permissões
```typescript
interface Permission {
  canViewUsers: boolean;
  canCreateUsers: boolean;
  canEditUsers: boolean;
  canDeleteUsers: boolean;
  canManageSystem: boolean;
  canViewReports: boolean;
  canEditReports: boolean;
  canManageRoles: boolean;
}
```

### Uso da Diretiva de Permissões
```html
<!-- Mostrar apenas para usuários com permissão específica -->
<button *appHasPermission="'canCreateUsers'">Criar Usuário</button>

<!-- Mostrar apenas para usuários com nível mínimo -->
<div *appHasMinRole="AccessLevel.MANAGER">Conteúdo do Manager</div>
```

## 📱 Componentes Principais

### LoginComponent
- Formulário de login com validação
- Integração com AuthService
- Redirecionamento após login

### RegisterComponent
- Formulário de registro
- Validação de senhas
- Confirmação de senha

### UserListComponent
- Tabela de usuários com Angular Material
- Ações de editar/excluir (apenas admin)
- Integração com UserService

## 🎨 Estilização

- **Angular Material** para componentes base
- **SCSS** para estilos personalizados
- **Responsive Design** com flexbox e grid
- **Tema personalizado** do Material Design

## 🔧 Scripts Disponíveis

```bash
npm start          # Servidor de desenvolvimento
npm run build      # Build de produção
npm test           # Testes unitários
npm run lint       # Linting do código
npm run e2e        # Testes end-to-end
```

## 📝 Próximos Passos

1. **Implementar CRUD completo de usuários**
   - Criar componente de edição
   - Criar componente de criação
   - Adicionar confirmações de exclusão

2. **Melhorar UX/UI**
   - Adicionar loading states
   - Implementar skeleton loading
   - Melhorar responsividade

3. **Testes**
   - Aumentar cobertura de testes unitários
   - Implementar testes e2e
   - Adicionar testes de integração

4. **Performance**
   - Implementar lazy loading
   - Otimizar bundle size
   - Adicionar Service Worker

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.