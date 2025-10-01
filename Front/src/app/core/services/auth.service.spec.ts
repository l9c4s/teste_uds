import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { RoleService } from './role.service';
import { LoginRequest, LoginResponse } from '../../shared/models/auth.model';
import { AccessLevel } from '../../shared/models/access-control.model';
import { environment } from '../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let roleService: jasmine.SpyObj<RoleService>;

  const mockLoginResponse: LoginResponse = {
    token: 'mock-jwt-token',
    user: {
      id: '123',
      name: 'Test User',
      email: 'test@example.com',
      accessLevel: 'Administrator'
    }
  };

  const mockDecodedToken = {
    sub: '123',
    email: 'test@example.com',
    name: 'Test User',
    role: 'Administrator',
    exp: Math.floor(Date.now() / 1000) + 3600,
    iat: Math.floor(Date.now() / 1000)
  };

  beforeEach(() => {
    const roleServiceSpy = jasmine.createSpyObj('RoleService', [
      'decodeToken',
      'isTokenExpired',
      'getUserRole',
      'getPermissions',
      'hasPermission',
      'hasMinimumRole',
      'getRoleDisplayName'
    ]);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: RoleService, useValue: roleServiceSpy }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    roleService = TestBed.inject(RoleService) as jasmine.SpyObj<RoleService>;

    // Configurar spies padrão
    roleService.isTokenExpired.and.returnValue(false);
    roleService.decodeToken.and.returnValue(mockDecodedToken);
    roleService.getUserRole.and.returnValue(AccessLevel.ADMINISTRATOR);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('login', () => {
    it('should login successfully and store user data', () => {
      const loginRequest: LoginRequest = {
        email: 'test@example.com',
        password: 'password123'
      };

      service.login(loginRequest).subscribe(response => {
        expect(response).toEqual(mockLoginResponse);
        expect(localStorage.getItem('token')).toBe('mock-jwt-token');
        expect(localStorage.getItem('currentUser')).toBeTruthy();
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(loginRequest);
      req.flush(mockLoginResponse);
    });

    it('should handle login error', () => {
      const loginRequest: LoginRequest = {
        email: 'test@example.com',
        password: 'wrongpassword'
      };

      service.login(loginRequest).subscribe({
        next: () => fail('Should have failed'),
        error: (error) => {
          expect(error.status).toBe(401);
        }
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/auth/login`);
      req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
    });
  });

  describe('register', () => {
    it('should register user successfully', () => {
      const registerRequest = {
        name: 'New User',
        email: 'newuser@example.com',
        password: 'password123',
        confirmPassword: 'password123'
      };

      service.register(registerRequest).subscribe(response => {
        expect(response).toBeTruthy();
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/auth/register`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(registerRequest);
      req.flush({ message: 'User created successfully' });
    });
  });

  describe('logout', () => {
    it('should clear user data on logout', () => {
      localStorage.setItem('token', 'test-token');
      localStorage.setItem('currentUser', JSON.stringify(mockLoginResponse.user));

      service.logout();

      expect(localStorage.getItem('token')).toBeNull();
      expect(localStorage.getItem('currentUser')).toBeNull();
    });
  });

  describe('getToken', () => {
    it('should return token when valid', () => {
      localStorage.setItem('token', 'valid-token');
      roleService.isTokenExpired.and.returnValue(false);

      const token = service.getToken();
      expect(token).toBe('valid-token');
    });

    it('should return null when token is expired', () => {
      localStorage.setItem('token', 'expired-token');
      roleService.isTokenExpired.and.returnValue(true);

      const token = service.getToken();
      expect(token).toBeNull();
      expect(localStorage.getItem('token')).toBeNull();
    });

    it('should return null when no token exists', () => {
      const token = service.getToken();
      expect(token).toBeNull();
    });
  });

  describe('isAuthenticated', () => {
    it('should return true when token is valid', () => {
      localStorage.setItem('token', 'valid-token');
      roleService.isTokenExpired.and.returnValue(false);

      expect(service.isAuthenticated()).toBeTrue();
    });

    it('should return false when token is expired', () => {
      localStorage.setItem('token', 'expired-token');
      roleService.isTokenExpired.and.returnValue(true);

      expect(service.isAuthenticated()).toBeFalse();
    });

    it('should return false when no token exists', () => {
      expect(service.isAuthenticated()).toBeFalse();
    });
  });

  describe('role-based methods', () => {
    beforeEach(() => {
      localStorage.setItem('token', 'valid-token');
    });

    it('should check if user is admin', () => {
      roleService.hasMinimumRole.and.returnValue(true);
      expect(service.isAdmin()).toBeTrue();
      expect(roleService.hasMinimumRole).toHaveBeenCalledWith('valid-token', AccessLevel.ADMINISTRATOR);
    });

    it('should check if user is manager', () => {
      roleService.hasMinimumRole.and.returnValue(true);
      expect(service.isManager()).toBeTrue();
      expect(roleService.hasMinimumRole).toHaveBeenCalledWith('valid-token', AccessLevel.MANAGER);
    });

    it('should check specific permissions', () => {
      roleService.hasPermission.and.returnValue(true);
      
      expect(service.canViewUsers()).toBeTrue();
      expect(service.canCreateUsers()).toBeTrue();
      expect(service.canEditUsers()).toBeTrue();
      expect(service.canDeleteUsers()).toBeTrue();
      expect(service.canManageSystem()).toBeTrue();

      expect(roleService.hasPermission).toHaveBeenCalledWith('valid-token', 'canViewUsers');
      expect(roleService.hasPermission).toHaveBeenCalledWith('valid-token', 'canCreateUsers');
      expect(roleService.hasPermission).toHaveBeenCalledWith('valid-token', 'canEditUsers');
      expect(roleService.hasPermission).toHaveBeenCalledWith('valid-token', 'canDeleteUsers');
      expect(roleService.hasPermission).toHaveBeenCalledWith('valid-token', 'canManageSystem');
    });
  });
});