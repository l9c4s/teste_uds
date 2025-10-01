                 import { Injectable } from '@angular/core';
import { AccessLevel, DecodedToken, Permission } from '../../shared/models/access-control.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor() {}

  /**
   * Decodifica o token JWT para extrair informações do usuário
   */
  decodeToken(token: string): DecodedToken | null {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (error) {
      console.error('Erro ao decodificar token:', error);
      return null;
    }
  }

  /**
   * Verifica se o token está expirado
   */
  isTokenExpired(token: string): boolean {
    const decoded = this.decodeToken(token);
    if (!decoded) return true;
    
    const currentTime = Math.floor(Date.now() / 1000);
    return decoded.exp < currentTime;
  }

  /**
   * Obtém o nível de acesso do usuário do token
   */
  getUserRole(token: string): AccessLevel | null {
    const decoded = this.decodeToken(token);
    if (!decoded) return null;
    
    return decoded.role as AccessLevel;
  }

  /**
   * Define as permissões baseadas no nível de acesso
   * Hierarquia: Administrator > Manager > CommonUser > Viewer
   */
  getPermissions(role: AccessLevel): Permission {
    switch (role) {
      case AccessLevel.ADMINISTRATOR:
        return {
          canViewUsers: true,
          canCreateUsers: true,
          canEditUsers: true,
          canDeleteUsers: true,
          canManageSystem: true,
          canViewReports: true,
          canEditReports: true,
          canManageRoles: true
        };

      case AccessLevel.MANAGER:
        return {
          canViewUsers: true,
          canCreateUsers: true,
          canEditUsers: true,
          canDeleteUsers: true,
          canManageSystem: false, // Não pode gerenciar sistema
          canViewReports: true,
          canEditReports: true,
          canManageRoles: false // Não pode gerenciar roles
        };

      case AccessLevel.COMMON_USER:
        return {
          canViewUsers: true,
          canCreateUsers: false, // Não pode criar usuários
          canEditUsers: false,   // Não pode editar usuários
          canDeleteUsers: false, // Não pode deletar usuários
          canManageSystem: false,
          canViewReports: true,
          canEditReports: false, // Não pode editar relatórios
          canManageRoles: false
        };

      case AccessLevel.VIEWER:
        return {
          canViewUsers: true,
          canCreateUsers: false,
          canEditUsers: false,
          canDeleteUsers: false,
          canManageSystem: false,
          canViewReports: true,
          canEditReports: false,
          canManageRoles: false
        };

      default:
        return {
          canViewUsers: false,
          canCreateUsers: false,
          canEditUsers: false,
          canDeleteUsers: false,
          canManageSystem: false,
          canViewReports: false,
          canEditReports: false,
          canManageRoles: false
        };
    }
  }

  /**
   * Verifica se o usuário tem permissão específica
   */
  hasPermission(token: string, permission: keyof Permission): boolean {
    const role = this.getUserRole(token);
    if (!role) return false;

    const permissions = this.getPermissions(role);
    return permissions[permission];
  }

  /**
   * Verifica se o usuário tem pelo menos um nível de acesso específico
   */
  hasMinimumRole(token: string, minimumRole: AccessLevel): boolean {
    const userRole = this.getUserRole(token);
    if (!userRole) return false;

    const roleHierarchy = {
      [AccessLevel.VIEWER]: 1,
      [AccessLevel.COMMON_USER]: 2,
      [AccessLevel.MANAGER]: 3,
      [AccessLevel.ADMINISTRATOR]: 4
    };

    return roleHierarchy[userRole] >= roleHierarchy[minimumRole];
  }

  /**
   * Obtém o nome amigável do nível de acesso
   */
  getRoleDisplayName(role: AccessLevel): string {
    switch (role) {
      case AccessLevel.ADMINISTRATOR:
        return 'Administrador';
      case AccessLevel.MANAGER:
        return 'Gerente';
      case AccessLevel.COMMON_USER:
        return 'Usuário Comum';
      case AccessLevel.VIEWER:
        return 'Visualizador';
      default:
        return 'Desconhecido';
    }
  }
}