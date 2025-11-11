import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ProvidersResponse, ProviderResponse, UpdateProviderDto, UpdateProviderResponse } from '../models/provider.models';

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
}

