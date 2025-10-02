
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