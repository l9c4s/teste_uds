import { AccessLevel } from './access-level.model';

export interface User {
  id: string;
  name: string;
  email: string;
  accessLevel: {
    id: string;
    name: string;
    description: string;
  };
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  accessLevelId: string;
}

export interface UpdateUserRequest {
  name?: string;
  email?: string;
  accessLevelId?: string;
}