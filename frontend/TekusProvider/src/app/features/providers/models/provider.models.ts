/**
 * Archivo de re-exportación para mantener compatibilidad
 * Todas las interfaces están ahora en archivos separados
 */

// Re-exportar Service para mantener compatibilidad
export type { Service } from '../../services/models/service.models';

// Re-exportar todas las interfaces desde sus archivos individuales
export type { CustomField } from './custom-field.model';
export type { Provider } from './provider.model';
export type { ProvidersResponse } from './providers-response.model';
export type { ProviderResponse } from './provider-response.model';
export type { UpdateCustomFieldDto } from './update-custom-field-dto.model';
export type { UpdateProviderDto } from './update-provider-dto.model';
export type { UpdateProviderResponse } from './update-provider-response.model';
export type { CreateCustomFieldDto } from './create-custom-field-dto.model';
export type { UpdateCustomFieldCommandDto } from './update-custom-field-command-dto.model';
export type { CustomFieldResponse } from './custom-field-response.model';
export type { CustomFieldsResponse } from './custom-fields-response.model';
export type { DeleteCustomFieldResponse } from './delete-custom-field-response.model';

// Re-exportar interfaces de servicios para mantener compatibilidad
export type { CreateServiceDto, ServiceResponse } from '../../services/models/service.models';
