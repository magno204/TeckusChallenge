/**
 * Modelos para las estadísticas del dashboard
 */

/**
 * Respuesta genérica de la API
 */
export interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  message: string;
  errors: string[];
}

/**
 * Estadísticas por país
 */
export interface CountryStatistic {
  countryCode: string;
  countryName: string;
  providersCount: number;
  servicesCount: number;
}

/**
 * Proveedores por país
 */
export interface ProvidersByCountryResponse {
  countries: CountryStatistic[];
}

/**
 * Servicios por país
 */
export interface ServicesByCountryResponse {
  countries: CountryStatistic[];
}

/**
 * Servicio más caro/barato
 */
export interface ServicePrice {
  id: string;
  name: string;
  providerName: string;
  hourlyRate: number;
}

/**
 * Reporte de resumen completo
 */
export interface SummaryReport {
  countryStatistics: CountryStatistic[];
  totalProviders: number;
  totalServices: number;
  totalCountriesCovered: number;
  averageHourlyRate: number;
  mostExpensiveService: ServicePrice | null;
  cheapestService: ServicePrice | null;
}

