import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ProvidersResponse } from '../models/providers-response.model';
import { ProviderResponse } from '../models/provider-response.model';
import { UpdateProviderDto } from '../models/update-provider-dto.model';
import { UpdateProviderResponse } from '../models/update-provider-response.model';
import { CreateCustomFieldDto } from '../models/create-custom-field-dto.model';
import { UpdateCustomFieldCommandDto } from '../models/update-custom-field-command-dto.model';
import { CustomFieldResponse } from '../models/custom-field-response.model';
import { CustomFieldsResponse } from '../models/custom-fields-response.model';
import { DeleteCustomFieldResponse } from '../models/delete-custom-field-response.model';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  private api = inject(ApiService);

  /**
   * Obtiene el listado de proveedores
   */
  getProviders(): Observable<ProvidersResponse> {
    return this.api.get<ProvidersResponse>('v1/providers');
  }

  /**
   * Obtiene un proveedor por su ID
   */
  getProviderById(id: string): Observable<ProviderResponse> {
    return this.api.get<ProviderResponse>(`v1/providers/${id}`);
  }

  /**
   * Actualiza la información de un proveedor
   */
  updateProvider(id: string, data: UpdateProviderDto): Observable<UpdateProviderResponse> {
    return this.api.put<UpdateProviderResponse>(`v1/providers/${id}`, data);
  }

  // Métodos para campos personalizados

  /**
   * Crea un nuevo campo personalizado para un proveedor
   */
  createCustomField(data: CreateCustomFieldDto): Observable<CustomFieldResponse> {
    return this.api.post<CustomFieldResponse>('v1/ProviderCustomFields', data);
  }

  /**
   * Actualiza un campo personalizado existente
   */
  updateCustomField(id: string, data: UpdateCustomFieldCommandDto): Observable<CustomFieldResponse> {
    return this.api.put<CustomFieldResponse>(`v1/ProviderCustomFields/${id}`, data);
  }

  /**
   * Elimina un campo personalizado
   */
  deleteCustomField(id: string): Observable<DeleteCustomFieldResponse> {
    return this.api.delete<DeleteCustomFieldResponse>(`v1/ProviderCustomFields/${id}`);
  }

  /**
   * Obtiene todos los campos personalizados de un proveedor
   */
  getCustomFieldsByProviderId(providerId: string): Observable<CustomFieldsResponse> {
    return this.api.get<CustomFieldsResponse>(`v1/ProviderCustomFields/provider/${providerId}`);
  }

  /**
   * Obtiene un campo personalizado por su ID
   */
  getCustomFieldById(id: string): Observable<CustomFieldResponse> {
    return this.api.get<CustomFieldResponse>(`v1/ProviderCustomFields/${id}`);
  }
}

