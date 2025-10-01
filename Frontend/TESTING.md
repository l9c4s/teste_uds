# Guia de Testes - User Management Frontend

Este documento descreve como executar e escrever testes para a aplicação Angular.

## 🧪 Tipos de Testes

### Unit Tests (Testes Unitários)
- **Localização**: `*.spec.ts` files
- **Framework**: Jasmine + Karma
- **Objetivo**: Testar componentes, serviços e funções isoladamente

### Integration Tests (Testes de Integração)
- **Localização**: `*.integration.spec.ts` files
- **Objetivo**: Testar interação entre múltiplos componentes

### E2E Tests (Testes End-to-End)
- **Framework**: Protractor/Cypress (futuro)
- **Objetivo**: Testar fluxos completos da aplicação

## 🚀 Executando os Testes

### Comandos Disponíveis

```bash
# Executar testes em modo watch (desenvolvimento)
npm test

# Executar testes uma vez (CI/CD)
npm run test:headless

# Executar testes com relatório de cobertura
npm run test:coverage

# Executar testes em modo watch
npm run test:watch
```

### Configuração do Karma

O arquivo `karma.conf.js` está configurado com:
- **Browsers**: Chrome, ChromeHeadless
- **Frameworks**: Jasmine, Angular Testing
- **Coverage**: Istanbul
- **Reporters**: Progress, Coverage, HTML

### Metas de Cobertura

- **Statements**: 80%
- **Branches**: 70%
- **Functions**: 80%
- **Lines**: 80%

## 📝 Estrutura dos Testes

### Exemplo de Teste de Serviço

```typescript
describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should login successfully', () => {
    const mockResponse = { token: 'fake-token', user: {...} };
    
    service.login({ email: 'test@test.com', password: '123456' })
      .subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

    const req = httpMock.expectOne('/api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });
});
```

### Exemplo de Teste de Componente

```typescript
describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('AuthService', ['login']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent, ReactiveFormsModule, NoopAnimationsModule],
      providers: [{ provide: AuthService, useValue: spy }]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    mockAuthService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
```

## 🛠 Ferramentas e Utilitários

### TestBed
- Configura módulo de teste
- Injeta dependências
- Cria instâncias de componentes

### HttpClientTestingModule
- Mock para requisições HTTP
- Controla respostas de API
- Verifica chamadas realizadas

### Spies (Espiões)
```typescript
// Criar spy para serviço
const serviceSpy = jasmine.createSpyObj('ServiceName', ['method1', 'method2']);

// Configurar retorno
serviceSpy.method1.and.returnValue(of(mockData));

// Verificar se foi chamado
expect(serviceSpy.method1).toHaveBeenCalled();
expect(serviceSpy.method1).toHaveBeenCalledWith(expectedParam);
```

### Debugging
```typescript
// Debug de componente
console.log(fixture.debugElement.nativeElement.innerHTML);

// Debug de formulário
console.log(component.form.value);
console.log(component.form.errors);
```

## 📋 Checklist para Novos Testes

### Para Serviços:
- [ ] Teste de criação (`should be created`)
- [ ] Teste de métodos públicos
- [ ] Teste de tratamento de erros
- [ ] Teste de chamadas HTTP (se aplicável)
- [ ] Teste de estados/observables

### Para Componentes:
- [ ] Teste de criação (`should create`)
- [ ] Teste de inicialização
- [ ] Teste de interações do usuário
- [ ] Teste de validações de formulário
- [ ] Teste de emissão de eventos
- [ ] Teste de navegação

### Para Guards:
- [ ] Teste de autorização (permitir acesso)
- [ ] Teste de negação (negar acesso)
- [ ] Teste de redirecionamento

## 🎯 Boas Práticas

### Nomenclatura
- Use nomes descritivos: `should return user data when login is successful`
- Agrupe testes relacionados com `describe`
- Use `it` para casos específicos

### Organização
```typescript
describe('ComponentName', () => {
  describe('method1', () => {
    it('should do something when condition is met', () => {});
    it('should handle error when something fails', () => {});
  });
  
  describe('method2', () => {
    it('should...', () => {});
  });
});
```

### Mocks e Stubs
- Mock dependências externas
- Use dados fixos para testes
- Evite dependências reais (APIs, banco de dados)

### Coverage (Cobertura)
- Mantenha cobertura acima das metas definidas
- Foque em cenários críticos
- Teste casos de erro e edge cases

## 🔍 Relatórios

### Coverage Report
Após executar `npm run test:coverage`, acesse:
```
./coverage/user-management-frontend/index.html
```

### Estrutura do Relatório:
- **Statements**: Linhas de código executadas
- **Branches**: Condições (if/else) testadas
- **Functions**: Funções chamadas
- **Lines**: Linhas físicas executadas

## 🐛 Debugging de Testes

### Problemas Comuns:

1. **Testes não encontram elementos DOM**
   ```typescript
   fixture.detectChanges(); // Força detecção de mudanças
   await fixture.whenStable(); // Aguarda operações assíncronas
   ```

2. **Formulários não validam**
   ```typescript
   component.form.markAllAsTouched();
   fixture.detectChanges();
   ```

3. **Observables não completam**
   ```typescript
   // Use fakeAsync/tick para controlar tempo
   it('should...', fakeAsync(() => {
     // test code
     tick();
     // assertions
   }));
   ```

## 📚 Recursos Adicionais

- [Angular Testing Guide](https://angular.io/guide/testing)
- [Jasmine Documentation](https://jasmine.github.io/)
- [Karma Documentation](http://karma-runner.github.io/)
- [Testing Best Practices](https://angular.io/guide/testing-code-coverage)