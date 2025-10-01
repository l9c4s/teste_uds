import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, RegisterRequest, AuthUser } from '../../shared/models/auth.model';
import { RoleService } from './role.service';
import { AccessLevel, Permission } from '../../shared/models/access-control.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;
  private currentUserSubject = new BehaviorSubject<AuthUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private roleService: RoleService
  ) {
    // Carregar usuário do localStorage se existir e token for válido
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const token = localStorage.getItem('token');
    const savedUser = localStorage.getItem('currentUser');
    
    if (token && savedUser && !this.roleService.isTokenExpired(token)) {
      this.currentUserSubject.next(JSON.parse(savedUser));
    } else {
      // Token expirado ou inválido, limpar dados
      this.clearStorage();
    }
  }

  private clearStorage(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          // Salvar token
          localStorage.setItem('token', response.token);
          
          // Decodificar token para obter informações do usuário
          const decodedToken = this.roleService.decodeToken(response.token);
          if (decodedToken) {
            const userWithRole: AuthUser = {
              id: decodedToken.sub,
              name: decodedToken.name,
              email: decodedToken.email,
              accessLevel: decodedToken.role
            };
            
            localStorage.setItem('currentUser', JSON.stringify(userWithRole));
            this.currentUserSubject.next(userWithRole);
          }
        })
      );
  }

  register(userData: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, userData);
  }

  logout(): void {
    this.clearStorage();
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    if (token && this.roleService.isTokenExpired(token)) {
      this.clearStorage();
      return null;
    }
    return token;
  }

  getCurrentUser(): AuthUser | null {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token && !this.roleService.isTokenExpired(token);
  }

  // Métodos de verificação de roles baseados na hierarquia
  getUserRole(): AccessLevel | null {
    const token = this.getToken();
    return token ? this.roleService.getUserRole(token) : null;
  }

  getUserPermissions(): Permission | null {
    const role = this.getUserRole();
    return role ? this.roleService.getPermissions(role) : null;
  }

  hasPermission(permission: keyof Permission): boolean {
    const token = this.getToken();
    return token ? this.roleService.hasPermission(token, permission) : false;
  }

  hasMinimumRole(minimumRole: AccessLevel): boolean {
    const token = this.getToken();
    return token ? this.roleService.hasMinimumRole(token, minimumRole) : false;
  }

  // Métodos de conveniência para verificações específicas
  isAdmin(): boolean {
    return this.hasMinimumRole(AccessLevel.ADMINISTRATOR);
  }

  isManager(): boolean {
    return this.hasMinimumRole(AccessLevel.MANAGER);
  }

  isCommonUser(): boolean {
    return this.hasMinimumRole(AccessLevel.COMMON_USER);
  }

  canViewUsers(): boolean {
    return this.hasPermission('canViewUsers');
  }

  canCreateUsers(): boolean {
    return this.hasPermission('canCreateUsers');
  }

  canEditUsers(): boolean {
    return this.hasPermission('canEditUsers');
  }

  canDeleteUsers(): boolean {
    return this.hasPermission('canDeleteUsers');
  }

  canManageSystem(): boolean {
    return this.hasPermission('canManageSystem');
  }
}