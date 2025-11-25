/**
 * Interface para CustomFieldsResponse
 */
import { CustomField } from './custom-field.model';

export interface CustomFieldsResponse {
  data: CustomField[];
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

