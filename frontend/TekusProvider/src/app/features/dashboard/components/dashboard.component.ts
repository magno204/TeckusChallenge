import { Component, signal, inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsService } from '../services/statistics.service';
import { SummaryReport } from '../models/statistics.models';

/**
 * Componente principal del dashboard
 * Muestra las estadísticas y métricas clave del sistema
 */
@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit {
  private readonly statisticsService = inject(StatisticsService);
  
  protected readonly summaryReport = signal<SummaryReport | null>(null);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadSummaryReport();
  }

  /**
   * Carga el reporte de resumen
   */
  private loadSummaryReport(): void {
    this.loading.set(true);
    this.error.set(null);

    this.statisticsService.getSummaryReport().subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.summaryReport.set(response.data);
        } else {
          this.error.set(response.message || 'Error al cargar las estadísticas');
        }
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error al conectar con el servidor');
        this.loading.set(false);
        console.error('Error loading summary report:', err);
      }
    });
  }

  /**
   * Recarga las estadísticas
   */
  protected refresh(): void {
    this.loadSummaryReport();
  }
}

