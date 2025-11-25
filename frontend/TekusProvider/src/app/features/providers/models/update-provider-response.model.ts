/**
 * Interface para UpdateProviderResponse
 */
import { Provider } from './provider.model';

export interface UpdateProviderResponse {
  data: Provider;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

