import { Injectable, signal } from '@angular/core';

/**
 * Servicio para gestionar el estado del sidebar
 * Permite compartir el estado de colapso entre componentes
 */
@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  /**
   * Estado del sidebar (colapsado o expandido)
   */
  readonly isCollapsed = signal(false);

  /**
   * Alterna el estado de colapso del sidebar
   */
  toggle(): void {
    this.isCollapsed.update(value => !value);
  }

  /**
   * Colapsa el sidebar
   */
  collapse(): void {
    this.isCollapsed.set(true);
  }

  /**
   * Expande el sidebar
   */
  expand(): void {
    this.isCollapsed.set(false);
  }
}

