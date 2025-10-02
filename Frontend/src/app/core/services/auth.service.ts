import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, AuthUser } from '../../shared/models/Auth/auth.model'; 
import { AccessLevel } from '../../shared/enums/AccessLevel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = environment.apiUrl;
  private currentUserSubject = new BehaviorSubject<AuthUser | null>(null); 
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    try {
      const userData = localStorage.getItem('currentUser');
      if (userData) {
        const user = JSON.parse(userData);
        this.currentUserSubject.next(user);
      }
    } catch (error) {
      console.error('Erro ao carregar usuário do localStorage:', error);
    }
  }

  login(credentials: LoginRequest): Observable<AuthResponse> {
    debugger;
    return this.http.post<AuthResponse>(`${this.API_URL}/auth/login`, credentials)
      .pipe(
        tap(response => {
          debugger;
          localStorage.setItem('token', response.token);
          localStorage.setItem('currentUser', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        }),
        catchError(error => {
          debugger;
          console.error('Erro no login:', error);
          return throwError(() => error);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token && !this.isTokenExpired(token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }


  minimumPermission(requiredLevel: AccessLevel): boolean {
    const currentUser = this.currentUserSubject.value;

    if (!currentUser) {
      return false;
    }
    const hierarchy: { [key in AccessLevel]: number } = {
      [AccessLevel.ADMINISTRATOR]: 4,
      [AccessLevel.MANAGER]: 3,
      [AccessLevel.COMMON_USER]: 2,
      [AccessLevel.VIEWER]: 1
    };

    const userLevel = hierarchy[currentUser.accessLevel];
    const minimumLevel = hierarchy[requiredLevel];

    // Retorna true se o nível do usuário for maior ou igual ao mínimo exigido
    return userLevel >= minimumLevel;
  }

  getCurrentUser(): AuthUser | null { // Atualizado
    return this.currentUserSubject.value;
  }


  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const now = Math.floor(Date.now() / 1000);
      return payload.exp < now;
    } catch (error) {
      return true;
    }
  }
}