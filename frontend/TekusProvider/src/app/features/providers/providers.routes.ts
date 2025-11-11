import { Routes } from '@angular/router';
import { ProvidersListComponent } from './components/providers-list.component';
import { ProviderEditComponent } from './components/provider-edit.component';

/**
 * Rutas del módulo de Providers
 */
export const providersRoutes: Routes = [
  {
    path: '',
    component: ProvidersListComponent
  },
  {
    path: 'edit/:id',
    component: ProviderEditComponent
  }
];

