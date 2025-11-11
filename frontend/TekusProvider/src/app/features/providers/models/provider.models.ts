/**
 * Interfaces para el módulo de Providers
 */

export interface CustomField {
  id: string;
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
  createdAt: string;
  updatedAt: string | null;
}

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

export interface Provider {
  id: string;
  nit: string;
  name: string;
  email: string;
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
  customFields: CustomField[];
  services: Service[];
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

export interface ProviderResponse {
  data: Provider;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

export interface UpdateCustomFieldDto {
  fieldName: string;
  fieldValue: string;
  fieldType: string;
  description: string;
  displayOrder: number;
}

export interface UpdateProviderDto {
  nit: string;
  name: string;
  email: string;
  customFields: UpdateCustomFieldDto[];
}

export interface UpdateProviderResponse {
  data: Provider;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

// Interfaces para operaciones de campos personalizados
export interface CreateCustomFieldDto {
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'email' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
}

export interface UpdateCustomFieldCommandDto {
  id: string;
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'email' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
}

export interface CustomFieldResponse {
  data: CustomField;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

export interface CustomFieldsResponse {
  data: CustomField[];
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

export interface DeleteCustomFieldResponse {
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

