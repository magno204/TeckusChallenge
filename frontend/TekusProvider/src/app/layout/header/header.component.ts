import { Component, signal, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

/**
 * Componente de header
 * Muestra el título de la página actual y breadcrumbs
 */
@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {
  private readonly router = inject(Router);

  protected readonly currentRoute = toSignal(
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.getPageTitle())
    ),
    { initialValue: this.getPageTitle() }
  );

  protected readonly currentDate = new Date().toLocaleDateString('es-ES', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });

  /**
   * Obtiene el título de la página actual basado en la ruta
   */
  private getPageTitle(): string {
    const url = this.router.url;
    
    if (url.includes('/dashboard')) {
      return 'Dashboard';
    } else if (url.includes('/providers')) {
      return 'Gestión de Proveedores';
    } else if (url.includes('/services')) {
      return 'Gestión de Servicios';
    }
    
    return 'Tekus Provider';
  }
}

