import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { ProvidersResponse } from '../models/provider.models';

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
}

