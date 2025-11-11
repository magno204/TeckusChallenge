import { Component, signal, inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProviderService } from '../services/provider.service';
import { Provider } from '../models/provider.models';

/**
 * Componente del listado de proveedores
 * Muestra una tabla con todos los proveedores registrados
 */
@Component({
  selector: 'app-providers-list',
  imports: [CommonModule],
  templateUrl: './providers-list.component.html',
  styleUrl: './providers-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProvidersListComponent implements OnInit {
  private readonly providerService = inject(ProviderService);
  private readonly router = inject(Router);

  protected readonly providers = signal<Provider[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.loadProviders();
  }

  /**
   * Carga el listado de proveedores
   */
  protected loadProviders(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.providerService.getProviders().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.providers.set(response.data);
        } else {
          this.errorMessage.set(response.message || 'Error al cargar proveedores');
        }
        this.isLoading.set(false);
      },
      error: (error) => {
        this.errorMessage.set(error.error?.message || 'Error al conectar con el servidor');
        this.isLoading.set(false);
      }
    });
  }

  /**
   * Navega a la página de edición del proveedor
   */
  protected editProvider(provider: Provider): void {
    this.router.navigate(['/providers/edit', provider.id]);
  }
}

