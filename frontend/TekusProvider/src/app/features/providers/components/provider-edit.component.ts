import { Component, signal, inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProviderService } from '../services/provider.service';
import { Provider, UpdateProviderDto, CustomField, UpdateCustomFieldDto } from '../models/provider.models';
import { CustomFieldDialogComponent, CustomFieldDialogData } from './custom-field-dialog.component';

@Component({
  selector: 'app-provider-edit',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule,
    MatDialogModule,
    MatTooltipModule
  ],
  templateUrl: './provider-edit.component.html',
  styleUrl: './provider-edit.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProviderEditComponent implements OnInit {
  private providerService = inject(ProviderService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  providerId = signal<string>('');
  provider = signal<Provider | null>(null);
  isLoading = signal(false);
  isSaving = signal(false);
  errorMessage = signal<string | null>(null);

  providerForm: FormGroup;

  constructor() {
    this.providerForm = this.fb.group({
      nit: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      customFields: this.fb.array([])
    });
  }

  get customFields(): FormArray {
    return this.providerForm.get('customFields') as FormArray;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.providerId.set(id);
      this.loadProvider(id);
    } else {
      this.errorMessage.set('ID de proveedor no encontrado');
    }
  }

  loadProvider(id: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.providerService.getProviderById(id).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.provider.set(response.data);
          this.providerForm.patchValue({
            nit: response.data.nit,
            name: response.data.name,
            email: response.data.email
          });
          
          // Cargar campos personalizados ordenados
          const sortedFields = [...response.data.customFields].sort((a, b) => a.displayOrder - b.displayOrder);
          this.loadCustomFields(sortedFields);
        } else {
          this.errorMessage.set(response.message || 'Error al cargar el proveedor');
        }
        this.isLoading.set(false);
      },
      error: (error) => {
        this.errorMessage.set(error.error?.message || 'Error al conectar con el servidor');
        this.isLoading.set(false);
      }
    });
  }

  loadCustomFields(fields: CustomField[]): void {
    this.customFields.clear();
    fields.forEach(field => {
      const fieldValue = this.getFieldValueByType(field);
      this.customFields.push(this.fb.group({
        id: [field.id],
        fieldName: [field.fieldName, Validators.required],
        fieldValue: [fieldValue, Validators.required],
        fieldType: [field.fieldType],
        description: [field.description],
        displayOrder: [field.displayOrder]
      }));
    });
  }

  getFieldValueByType(field: CustomField): any {
    switch (field.fieldType) {
      case 'boolean':
        return field.fieldValue === 'true';
      case 'date':
        return field.fieldValue ? new Date(field.fieldValue) : null;
      case 'number':
        return field.fieldValue ? parseFloat(field.fieldValue) : null;
      default:
        return field.fieldValue;
    }
  }

  getFieldIcon(fieldType: string): string {
    switch (fieldType) {
      case 'text': return 'text_fields';
      case 'url': return 'link';
      case 'date': return 'calendar_today';
      case 'boolean': return 'toggle_on';
      case 'number': return 'numbers';
      default: return 'settings';
    }
  }

  openCreateCustomFieldDialog(): void {
    const existingOrders = this.customFields.controls.map(
      control => control.get('displayOrder')?.value || 0
    );

    const dialogRef = this.dialog.open(CustomFieldDialogComponent, {
      width: '600px',
      maxWidth: '90vw',
      disableClose: true,
      data: {
        mode: 'create',
        existingDisplayOrders: existingOrders
      } as CustomFieldDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.addCustomField(result);
      }
    });
  }

  openEditCustomFieldDialog(index: number): void {
    const field = this.customFields.at(index);
    const existingOrders = this.customFields.controls.map(
      control => control.get('displayOrder')?.value || 0
    );

    const currentFieldData: CustomField = {
      id: field.get('id')?.value,
      providerId: this.providerId(),
      fieldName: field.get('fieldName')?.value,
      fieldValue: this.convertFieldValueToString(field.get('fieldValue')?.value, field.get('fieldType')?.value),
      fieldType: field.get('fieldType')?.value,
      description: field.get('description')?.value,
      displayOrder: field.get('displayOrder')?.value,
      createdAt: '',
      updatedAt: null
    };

    const dialogRef = this.dialog.open(CustomFieldDialogComponent, {
      width: '600px',
      maxWidth: '90vw',
      disableClose: true,
      data: {
        field: currentFieldData,
        mode: 'edit',
        existingDisplayOrders: existingOrders
      } as CustomFieldDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateCustomField(index, result);
      }
    });
  }

  addCustomField(fieldData: any): void {
    const fieldValue = this.getFieldValueByTypeFromString(fieldData.fieldValue, fieldData.fieldType);
    
    this.customFields.push(this.fb.group({
      id: [null], // null para nuevos campos
      fieldName: [fieldData.fieldName, Validators.required],
      fieldValue: [fieldValue, Validators.required],
      fieldType: [fieldData.fieldType],
      description: [fieldData.description],
      displayOrder: [fieldData.displayOrder]
    }));

    // Reordenar campos por displayOrder
    this.sortCustomFields();

    this.snackBar.open('Campo personalizado agregado', 'Cerrar', {
      duration: 2000
    });
  }

  updateCustomField(index: number, fieldData: any): void {
    const fieldValue = this.getFieldValueByTypeFromString(fieldData.fieldValue, fieldData.fieldType);
    
    this.customFields.at(index).patchValue({
      fieldName: fieldData.fieldName,
      fieldValue: fieldValue,
      fieldType: fieldData.fieldType,
      description: fieldData.description,
      displayOrder: fieldData.displayOrder
    });

    // Reordenar campos por displayOrder
    this.sortCustomFields();

    this.snackBar.open('Campo personalizado actualizado', 'Cerrar', {
      duration: 2000
    });
  }

  deleteCustomField(index: number): void {
    const fieldName = this.customFields.at(index).get('fieldName')?.value;
    
    if (confirm(`¿Está seguro de eliminar el campo "${fieldName}"?`)) {
      this.customFields.removeAt(index);
      this.snackBar.open('Campo personalizado eliminado', 'Cerrar', {
        duration: 2000
      });
    }
  }

  sortCustomFields(): void {
    const fieldsArray = this.customFields.controls
      .map((control, index) => ({ control, index }))
      .sort((a, b) => {
        const orderA = a.control.get('displayOrder')?.value || 0;
        const orderB = b.control.get('displayOrder')?.value || 0;
        return orderA - orderB;
      });

    this.customFields.clear();
    fieldsArray.forEach(item => {
      this.customFields.push(item.control);
    });
  }

  getFieldValueByTypeFromString(value: string, fieldType: string): any {
    switch (fieldType) {
      case 'boolean':
        return value === 'true';
      case 'date':
        return value ? new Date(value) : null;
      case 'number':
        return value ? parseFloat(value) : null;
      default:
        return value;
    }
  }

  onSubmit(): void {
    if (this.providerForm.valid && !this.isSaving()) {
      this.isSaving.set(true);
      this.errorMessage.set(null);

      const customFieldsData: UpdateCustomFieldDto[] = this.customFields.value.map((field: any) => ({
        fieldName: field.fieldName,
        fieldValue: this.convertFieldValueToString(field.fieldValue, field.fieldType),
        fieldType: field.fieldType,
        description: field.description,
        displayOrder: field.displayOrder
      }));

      const updateData: UpdateProviderDto = {
        nit: this.providerForm.value.nit,
        name: this.providerForm.value.name,
        email: this.providerForm.value.email,
        customFields: customFieldsData
      };

      this.providerService.updateProvider(this.providerId(), updateData).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.snackBar.open('Proveedor actualizado exitosamente', 'Cerrar', {
              duration: 3000,
              horizontalPosition: 'center',
              verticalPosition: 'top'
            });
            this.router.navigate(['/providers']);
          } else {
            this.errorMessage.set(response.message || 'Error al actualizar el proveedor');
            this.snackBar.open(response.message || 'Error al actualizar', 'Cerrar', {
              duration: 5000,
              panelClass: ['error-snackbar']
            });
          }
          this.isSaving.set(false);
        },
        error: (error) => {
          const errorMsg = error.error?.message || 'Error al conectar con el servidor';
          this.errorMessage.set(errorMsg);
          this.snackBar.open(errorMsg, 'Cerrar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
          this.isSaving.set(false);
        }
      });
    } else {
      this.markFormGroupTouched(this.providerForm);
    }
  }

  onCancel(): void {
    this.router.navigate(['/providers']);
  }

  convertFieldValueToString(value: any, fieldType: string): string {
    if (value === null || value === undefined) return '';
    
    switch (fieldType) {
      case 'boolean':
        return value ? 'true' : 'false';
      case 'date':
        return value instanceof Date ? value.toISOString().split('T')[0] : value;
      default:
        return value.toString();
    }
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else if (control instanceof FormArray) {
        control.controls.forEach((arrayControl) => {
          if (arrayControl instanceof FormGroup) {
            this.markFormGroupTouched(arrayControl);
          } else {
            arrayControl.markAsTouched();
          }
        });
      } else {
        control?.markAsTouched();
      }
    });
  }

  getErrorMessage(fieldName: string): string {
    const control = this.providerForm.get(fieldName);
    
    if (control?.hasError('required')) {
      return 'Este campo es requerido';
    }
    
    if (control?.hasError('email')) {
      return 'Por favor ingrese un email válido';
    }
    
    if (control?.hasError('minlength')) {
      const minLength = control.errors?.['minlength'].requiredLength;
      return `Debe tener al menos ${minLength} caracteres`;
    }
    
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `No puede tener más de ${maxLength} caracteres`;
    }
    
    return '';
  }
}

