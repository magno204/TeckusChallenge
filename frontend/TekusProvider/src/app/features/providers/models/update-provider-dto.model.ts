/**
 * Interface para UpdateProviderDto
 */
import { UpdateCustomFieldDto } from './update-custom-field-dto.model';

export interface UpdateProviderDto {
  nit: string;
  name: string;
  email: string;
  customFields: UpdateCustomFieldDto[];
}

