import { AccessLevel } from './access-level.model';

export interface AuthUser {
  id: number;
  name: string;
  email: string;
  accessLevel?: AccessLevel;
  createdAt: string;
  updatedAt: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  accessLevelId?: number;
}

export interface AuthResponse {
  user: AuthUser;
  token: string;
}

export interface TokenPayload {
  sub: number;
  email: string;
  name: string;
  accessLevel: string;
  iat: number;
  exp: number;
}