/**
 * Interface para UpdateCustomFieldCommandDto
 */
export interface UpdateCustomFieldCommandDto {
  id: string;
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'email' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
}

