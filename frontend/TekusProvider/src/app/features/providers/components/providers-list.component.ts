import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProviderService } from '../services/provider.service';
import { Provider } from '../models/provider.models';

@Component({
  selector: 'app-providers-list',
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule
  ],
  templateUrl: './providers-list.component.html',
  styleUrl: './providers-list.component.css'
})
export class ProvidersListComponent implements OnInit {
  private providerService = inject(ProviderService);
  private router = inject(Router);

  displayedColumns: string[] = ['name', 'nit', 'email', 'actions'];
  providers = signal<Provider[]>([]);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.loadProviders();
  }

  loadProviders(): void {
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

  editProvider(provider: Provider): void {
    this.router.navigate(['/providers/edit', provider.id]);
  }
}

