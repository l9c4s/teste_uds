import { TestBed } from '@angular/core/testing';
import { RoleService } from './role.service';
import { AccessLevel } from '../../shared/models/access-control.model';

describe('RoleService', () => {
  let service: RoleService;

  // Token JWT de exemplo para testes
  const mockTokenData = {
    sub: '123',
    email: 'test@example.com',
    name: 'Test User',
    role: 'Administrator',
    exp: Math.floor(Date.now() / 1000) + 3600, // 1 hora no futuro
    iat: Math.floor(Date.now() / 1000)
  };

  const validToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.' + 
    btoa(JSON.stringify(mockTokenData)) + 
    '.signature';

  const expiredTokenData = {
    ...mockTokenData,
    exp: Math.floor(Date.now() / 1000) - 3600 // 1 hora no passado
  };

  const expiredToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.' + 
    btoa(JSON.stringify(expiredTokenData)) + 
    '.signature';

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('decodeToken', () => {
    it('should decode a valid JWT token', () => {
      const decoded = service.decodeToken(validToken);
      
      expect(decoded).toBeTruthy();
      expect(decoded?.sub).toBe('123');
      expect(decoded?.email).toBe('test@example.com');
      expect(decoded?.name).toBe('Test User');
      expect(decoded?.role).toBe('Administrator');
    });

    it('should return null for invalid token', () => {
      const decoded = service.decodeToken('invalid-token');
      expect(decoded).toBeNull();
    });

    it('should return null for empty token', () => {
      const decoded = service.decodeToken('');
      expect(decoded).toBeNull();
    });
  });

  describe('isTokenExpired', () => {
    it('should return false for valid token', () => {
      const isExpired = service.isTokenExpired(validToken);
      expect(isExpired).toBeFalse();
    });

    it('should return true for expired token', () => {
      const isExpired = service.isTokenExpired(expiredToken);
      expect(isExpired).toBeTrue();
    });

    it('should return true for invalid token', () => {
      const isExpired = service.isTokenExpired('invalid-token');
      expect(isExpired).toBeTrue();
    });
  });

  describe('getUserRole', () => {
    it('should return correct role from token', () => {
      const role = service.getUserRole(validToken);
      expect(role).toBe(AccessLevel.ADMINISTRATOR);
    });

    it('should return null for invalid token', () => {
      const role = service.getUserRole('invalid-token');
      expect(role).toBeNull();
    });
  });

  describe('getPermissions', () => {
    it('should return all permissions for Administrator', () => {
      const permissions = service.getPermissions(AccessLevel.ADMINISTRATOR);
      
      expect(permissions.canViewUsers).toBeTrue();
      expect(permissions.canCreateUsers).toBeTrue();
      expect(permissions.canEditUsers).toBeTrue();
      expect(permissions.canDeleteUsers).toBeTrue();
      expect(permissions.canManageSystem).toBeTrue();
      expect(permissions.canViewReports).toBeTrue();
      expect(permissions.canEditReports).toBeTrue();
      expect(permissions.canManageRoles).toBeTrue();
    });

    it('should return limited permissions for Manager', () => {
      const permissions = service.getPermissions(AccessLevel.MANAGER);
      
      expect(permissions.canViewUsers).toBeTrue();
      expect(permissions.canCreateUsers).toBeTrue();
      expect(permissions.canEditUsers).toBeTrue();
      expect(permissions.canDeleteUsers).toBeTrue();
      expect(permissions.canManageSystem).toBeFalse();
      expect(permissions.canViewReports).toBeTrue();
      expect(permissions.canEditReports).toBeTrue();
      expect(permissions.canManageRoles).toBeFalse();
    });

    it('should return view-only permissions for CommonUser', () => {
      const permissions = service.getPermissions(AccessLevel.COMMON_USER);
      
      expect(permissions.canViewUsers).toBeTrue();
      expect(permissions.canCreateUsers).toBeFalse();
      expect(permissions.canEditUsers).toBeFalse();
      expect(permissions.canDeleteUsers).toBeFalse();
      expect(permissions.canManageSystem).toBeFalse();
      expect(permissions.canViewReports).toBeTrue();
      expect(permissions.canEditReports).toBeFalse();
      expect(permissions.canManageRoles).toBeFalse();
    });

    it('should return minimal permissions for Viewer', () => {
      const permissions = service.getPermissions(AccessLevel.VIEWER);
      
      expect(permissions.canViewUsers).toBeTrue();
      expect(permissions.canCreateUsers).toBeFalse();
      expect(permissions.canEditUsers).toBeFalse();
      expect(permissions.canDeleteUsers).toBeFalse();
      expect(permissions.canManageSystem).toBeFalse();
      expect(permissions.canViewReports).toBeTrue();
      expect(permissions.canEditReports).toBeFalse();
      expect(permissions.canManageRoles).toBeFalse();
    });
  });

  describe('hasPermission', () => {
    it('should return true when user has permission', () => {
      const hasPermission = service.hasPermission(validToken, 'canViewUsers');
      expect(hasPermission).toBeTrue();
    });

    it('should return false for invalid token', () => {
      const hasPermission = service.hasPermission('invalid-token', 'canViewUsers');
      expect(hasPermission).toBeFalse();
    });
  });

  describe('hasMinimumRole', () => {
    it('should return true when user has minimum role or higher', () => {
      const hasRole = service.hasMinimumRole(validToken, AccessLevel.MANAGER);
      expect(hasRole).toBeTrue();
    });

    it('should return false when user does not have minimum role', () => {
      // Criar token com role Viewer
      const viewerTokenData = { ...mockTokenData, role: 'Viewer' };
      const viewerToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.' + 
        btoa(JSON.stringify(viewerTokenData)) + 
        '.signature';
      
      const hasRole = service.hasMinimumRole(viewerToken, AccessLevel.MANAGER);
      expect(hasRole).toBeFalse();
    });

    it('should return false for invalid token', () => {
      const hasRole = service.hasMinimumRole('invalid-token', AccessLevel.VIEWER);
      expect(hasRole).toBeFalse();
    });
  });

  describe('getRoleDisplayName', () => {
    it('should return correct display names', () => {
      expect(service.getRoleDisplayName(AccessLevel.ADMINISTRATOR)).toBe('Administrador');
      expect(service.getRoleDisplayName(AccessLevel.MANAGER)).toBe('Gerente');
      expect(service.getRoleDisplayName(AccessLevel.COMMON_USER)).toBe('Usuário Comum');
      expect(service.getRoleDisplayName(AccessLevel.VIEWER)).toBe('Visualizador');
    });
  });
});