import { Component, inject, ChangeDetectionStrategy, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { SidebarService } from '../services/sidebar.service';

/**
 * Interfaz para los items del menú
 */
interface MenuItem {
  label: string;
  icon: string;
  route: string;
  active?: boolean;
}

/**
 * Componente de sidebar con navegación
 */
@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarComponent {
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly sidebarService = inject(SidebarService);

  protected readonly isCollapsed = computed(() => this.sidebarService.isCollapsed());
  protected readonly username = computed(() => this.authService.getUsername() || 'Usuario');

  protected readonly menuItems: MenuItem[] = [
    {
      label: 'Dashboard',
      icon: '📊',
      route: '/dashboard'
    },
    {
      label: 'Proveedores',
      icon: '👥',
      route: '/providers'
    }
  ];

  /**
   * Alterna el estado de colapso del sidebar
   */
  protected toggleSidebar(): void {
    this.sidebarService.toggle();
  }

  /**
   * Verifica si una ruta está activa
   */
  protected isActiveRoute(route: string): boolean {
    return this.router.url.startsWith(route);
  }

  /**
   * Cierra sesión
   */
  protected logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

