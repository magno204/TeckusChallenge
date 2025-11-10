/**
 * Interfaces para el módulo de autenticación
 */

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponseData {
  token: string;
  expiresAt: string;
  username: string;
}

export interface LoginResponse {
  data: LoginResponseData;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

