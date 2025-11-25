import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { CreateServiceDto, ServiceResponse } from '../models/service.models';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  private api = inject(ApiService);

  /**
   * Crea un nuevo servicio
   */
  createService(data: CreateServiceDto): Observable<ServiceResponse> {
    return this.api.post<ServiceResponse>('v1/services', data);
  }
}

