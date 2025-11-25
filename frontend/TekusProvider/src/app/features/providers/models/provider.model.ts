/**
 * Interface para Provider
 */
import { Service } from '../../services/models/service.models';
import { CustomField } from './custom-field.model';

export interface Provider {
  id: string;
  nit: string;
  name: string;
  email: string;
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
  customFields: CustomField[];
  services: Service[];
}

