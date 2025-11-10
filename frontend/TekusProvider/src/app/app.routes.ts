import { Routes } from '@angular/router';
import { providersRoutes } from './features/providers/providers.routes';
import { servicesRoutes } from './features/services/services.routes';
import { authRoutes } from './features/auth/auth.routes';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  // Rutas de autenticación
  {
    path: 'auth',
    children: authRoutes
  },
  {
    path: 'login',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  // Rutas protegidas
  {
    path: 'providers',
    canActivate: [authGuard],
    children: providersRoutes
  },
  {
    path: 'services',
    canActivate: [authGuard],
    children: servicesRoutes
  },
  {
    path: '',
    redirectTo: 'providers',
    pathMatch: 'full'
  }
];
