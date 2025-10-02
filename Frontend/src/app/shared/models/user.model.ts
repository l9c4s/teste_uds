import { AccessLevel } from './access-level.model';

export interface User {
  id: number;
  name: string;
  email: string;
  accessLevel?: AccessLevel;
  createdAt: string;
  updatedAt: string;
}


export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  accessLevelId: number;
}

export interface UpdateUserRequest {
  name?: string;
  email?: string;
  password?: string;
  accessLevelId?: number;
}