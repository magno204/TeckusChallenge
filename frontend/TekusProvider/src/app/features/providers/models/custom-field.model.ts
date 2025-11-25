/**
 * Interface para CustomField
 */
export interface CustomField {
  id: string;
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
  createdAt: string;
  updatedAt: string | null;
}

