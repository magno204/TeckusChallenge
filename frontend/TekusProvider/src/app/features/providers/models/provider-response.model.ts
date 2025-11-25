/**
 * Interface para ProviderResponse
 */
import { Provider } from './provider.model';

export interface ProviderResponse {
  data: Provider;
  isSuccess: boolean;
  message: string;
  errors: string[] | null;
}

