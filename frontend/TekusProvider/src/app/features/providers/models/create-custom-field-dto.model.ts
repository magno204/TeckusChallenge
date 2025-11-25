/**
 * Interface para CreateCustomFieldDto
 */
export interface CreateCustomFieldDto {
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'email' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
}

