export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: string;
    name: string;
    email: string;
    accessLevel: string;
  };
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthUser {
  id: string;
  name: string;
  email: string;
  accessLevel: string;
}