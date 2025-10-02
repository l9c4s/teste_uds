
export interface User {
  id: number;
  name: string;
  email: string;
  accessLevel: string;
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
  id: number;
  name?: string;
  email?: string;
  password?: string;
  accessLevelId?: number;
}