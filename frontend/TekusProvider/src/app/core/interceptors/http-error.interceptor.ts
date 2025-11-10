import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

/**
 * Interceptor para manejar errores HTTP globalmente
 * 
 * Registra este interceptor en app.config.ts:
 * provideHttpClient(withInterceptors([httpErrorInterceptor]))
 */
export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // TODO: Implementar manejo de errores según tus necesidades
      // - Mostrar notificaciones
      // - Redirigir a páginas de error
      // - Logging de errores
      
      console.error('HTTP Error:', error);
      
      return throwError(() => error);
    })
  );
};

