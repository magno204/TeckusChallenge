/**
 * Interfaces para el módulo de Providers
 */

export interface Provider {
  id: string;
  nit: string;
  name: string;
  email: string;
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
  customFields: any[];
  services: any[];
}

export interface ProvidersResponse {
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  data: Provider[];
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

