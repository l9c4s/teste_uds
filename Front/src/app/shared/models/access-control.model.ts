export enum AccessLevel {
  ADMINISTRATOR = 'Administrator',
  MANAGER = 'Manager', 
  COMMON_USER = 'CommonUser',
  VIEWER = 'Viewer'
}

export interface DecodedToken {
  sub: string;
  email: string;
  name: string;
  role: string;
  exp: number;
  iat: number;
}

export interface Permission {
  canViewUsers: boolean;
  canCreateUsers: boolean;
  canEditUsers: boolean;
  canDeleteUsers: boolean;
  canManageSystem: boolean;
  canViewReports: boolean;
  canEditReports: boolean;
  canManageRoles: boolean;
}