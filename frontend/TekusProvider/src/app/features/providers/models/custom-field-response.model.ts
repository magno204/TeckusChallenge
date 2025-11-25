/**
 * Interface para CustomFieldResponse
 */
import { CustomField } from './custom-field.model';

export interface CustomFieldResponse {
  data: CustomField;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

