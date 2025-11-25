/**
 * Interface para ProvidersResponse
 */
import { Provider } from './provider.model';

export interface ProvidersResponse {
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  data: Provider[];
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

