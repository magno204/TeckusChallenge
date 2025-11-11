import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import {
  ApiResponse,
  ProvidersByCountryResponse,
  ServicesByCountryResponse,
  SummaryReport
} from '../models/statistics.models';

/**
 * Servicio para gestionar las estadísticas del sistema
 */
@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private readonly api = inject(ApiService);
  private readonly baseEndpoint = 'v1/Statistics';

  /**
   * Obtiene el reporte de resumen con todos los indicadores clave
   */
  getSummaryReport(): Observable<ApiResponse<SummaryReport>> {
    return this.api.get<ApiResponse<SummaryReport>>(`${this.baseEndpoint}/summary`);
  }

  /**
   * Obtiene la cantidad de proveedores agrupados por país
   */
  getProvidersByCountry(): Observable<ApiResponse<ProvidersByCountryResponse>> {
    return this.api.get<ApiResponse<ProvidersByCountryResponse>>(`${this.baseEndpoint}/providers-by-country`);
  }

  /**
   * Obtiene la cantidad de servicios agrupados por país
   */
  getServicesByCountry(): Observable<ApiResponse<ServicesByCountryResponse>> {
    return this.api.get<ApiResponse<ServicesByCountryResponse>>(`${this.baseEndpoint}/services-by-country`);
  }
}

