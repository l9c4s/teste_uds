export interface AccessLevel {
  id: string;
  name: string;
  description: string;
}

export enum AccessLevelEnum {
  Admin = 'Admin',
  User = 'User',
  Guest = 'Guest'
}