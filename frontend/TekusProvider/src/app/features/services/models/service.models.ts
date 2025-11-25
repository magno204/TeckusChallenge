/**
 * Interfaces para el módulo de Services
 */

export interface Service {
  id: string;
  name: string;
  hourlyRate: number;
  description: string;
  providerId: string;
  providerName: string;
  countries: any[];
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
}

// Interfaces para operaciones de servicios
export interface CreateServiceDto {
  name: string;
  hourlyRate: number;
  description: string;
  providerId: string;
  countryCodes: string[];
}

export interface ServiceResponse {
  data: Service;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

