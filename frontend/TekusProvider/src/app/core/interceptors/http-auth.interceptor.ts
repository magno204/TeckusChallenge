import { HttpInterceptorFn, HttpErrorResponse, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

/**
 * Interceptor para agregar el token de autenticación y manejar errores HTTP globalmente
 * 
 * Registra este interceptor en app.config.ts:
 * provideHttpClient(withInterceptors([httpAuthInterceptor]))
 */
export const httpAuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  // Clonar la petición y agregar el token si existe
  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Si el error es 401 (Unauthorized), redirigir al login
      if (error.status === 401) {
        authService.logout();
        // TODO: Redirigir al login si es necesario
        // const router = inject(Router);
        // router.navigate(['/auth/login']);
      }

      // TODO: Implementar manejo de errores según tus necesidades
      // - Mostrar notificaciones
      // - Logging de errores
      
      console.error('HTTP Error:', error);
      
      return throwError(() => error);
    })
  );
};

