import { Routes } from '@angular/router';
import { providersRoutes } from './features/providers/providers.routes';
import { servicesRoutes } from './features/services/services.routes';
import { authRoutes } from './features/auth/auth.routes';
import { dashboardRoutes } from './features/dashboard/dashboard.routes';
import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';

export const routes: Routes = [
  // Rutas de autenticación (sin layout)
  {
    path: 'auth',
    children: authRoutes
  },
  {
    path: 'login',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  // Rutas protegidas con layout
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        children: dashboardRoutes
      },
      {
        path: 'providers',
        children: providersRoutes
      },
      {
        path: 'services',
        children: servicesRoutes
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  }
];
