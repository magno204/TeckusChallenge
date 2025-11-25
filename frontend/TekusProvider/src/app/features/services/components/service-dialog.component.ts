import { Component, inject, signal, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '@app/material.module';
import { FormsModule } from '@angular/forms';

export interface ServiceDialogData {
  providerId: string;
  providerName: string;
}

@Component({
  selector: 'app-service-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule
  ],
  templateUrl: './service-dialog.component.html',
  styleUrl: './service-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'service-dialog'
  }
})
export class ServiceDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ServiceDialogComponent>);
  data = inject<ServiceDialogData>(MAT_DIALOG_DATA);

  serviceForm: FormGroup;
  filteredCountries = signal<Country[]>([]);
  selectedCountryCodes: string[] = [];

  // Lista de países comunes con códigos ISO Alpha-2
  countries: Country[] = [
    { code: 'CO', name: 'Colombia' },
    { code: 'MX', name: 'México' },
    { code: 'PE', name: 'Perú' },
    { code: 'CL', name: 'Chile' },
    { code: 'AR', name: 'Argentina' },
    { code: 'BR', name: 'Brasil' },
    { code: 'US', name: 'Estados Unidos' },
    { code: 'CA', name: 'Canadá' },
    { code: 'ES', name: 'España' },
    { code: 'FR', name: 'Francia' },
    { code: 'DE', name: 'Alemania' },
    { code: 'IT', name: 'Italia' },
    { code: 'GB', name: 'Reino Unido' },
    { code: 'AU', name: 'Australia' },
    { code: 'NZ', name: 'Nueva Zelanda' },
    { code: 'JP', name: 'Japón' },
    { code: 'CN', name: 'China' },
    { code: 'IN', name: 'India' },
    { code: 'EC', name: 'Ecuador' },
    { code: 'BO', name: 'Bolivia' },
    { code: 'PY', name: 'Paraguay' },
    { code: 'UY', name: 'Uruguay' },
    { code: 'VE', name: 'Venezuela' },
    { code: 'PA', name: 'Panamá' },
    { code: 'CR', name: 'Costa Rica' },
    { code: 'GT', name: 'Guatemala' },
    { code: 'HN', name: 'Honduras' },
    { code: 'NI', name: 'Nicaragua' },
    { code: 'SV', name: 'El Salvador' },
    { code: 'DO', name: 'República Dominicana' }
  ];

  constructor() {
    this.serviceForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      hourlyRate: [0, [Validators.required, Validators.min(0.01)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      countrySearch: ['']
    });
  }

  ngOnInit(): void {
    this.filteredCountries.set([...this.countries]);
  }

  filterCountries(searchTerm: string): void {
    if (!searchTerm || searchTerm.trim() === '') {
      this.filteredCountries.set([...this.countries]);
      return;
    }

    const term = searchTerm.toLowerCase();
    const filtered = this.countries.filter(
      country => 
        country.name.toLowerCase().includes(term) || 
        country.code.toLowerCase().includes(term)
    );
    this.filteredCountries.set(filtered);
  }

  addCountry(countryCode: string): void {
    if (!this.selectedCountryCodes.includes(countryCode)) {
      this.selectedCountryCodes.push(countryCode);
      this.serviceForm.patchValue({ countrySearch: '' });
      this.filteredCountries.set([...this.countries]);
    }
  }

  removeCountry(countryCode: string): void {
    this.selectedCountryCodes = this.selectedCountryCodes.filter(code => code !== countryCode);
  }

  getCountryName(code: string): string {
    const country = this.countries.find(c => c.code === code);
    return country ? country.name : code;
  }

  onSubmit(): void {
    if (this.serviceForm.valid) {
      const result = {
        name: this.serviceForm.value.name,
        hourlyRate: this.serviceForm.value.hourlyRate,
        description: this.serviceForm.value.description,
        providerId: this.data.providerId,
        countryCodes: this.selectedCountryCodes
      };
      this.dialogRef.close(result);
    } else {
      this.markFormGroupTouched(this.serviceForm);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  getErrorMessage(fieldName: string): string {
    const control = this.serviceForm.get(fieldName);
    
    if (control?.hasError('required')) {
      return 'Este campo es requerido';
    }
    
    if (control?.hasError('minlength')) {
      const minLength = control.errors?.['minlength'].requiredLength;
      return `Debe tener al menos ${minLength} caracteres`;
    }
    
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `No puede tener más de ${maxLength} caracteres`;
    }

    if (control?.hasError('min')) {
      const min = control.errors?.['min'].min;
      return `El valor mínimo es ${min}`;
    }
    
    return '';
  }
}

interface Country {
  code: string;
  name: string;
}

