import { Component, signal, inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ProviderService } from '@providers/services/provider.service';
import { Provider } from '@providers/models/provider.model';
import { UpdateProviderDto } from '@providers/models/update-provider-dto.model';
import { CustomField } from '@providers/models/custom-field.model';
import { CreateCustomFieldDto } from '@providers/models/create-custom-field-dto.model';
import { UpdateCustomFieldCommandDto } from '@providers/models/update-custom-field-command-dto.model';
import { CustomFieldDialogComponent, CustomFieldDialogData } from './custom-field-dialog.component';
import { ServiceDialogComponent, ServiceDialogData } from '@services/components/service-dialog.component';
import { ServiceService } from '@services/services/service.service';
import { CreateServiceDto } from '@services/models/service.models';
import { MaterialModule } from '@app/material.module';

@Component({
  selector: 'app-provider-edit',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  templateUrl: './provider-edit.component.html',
  styleUrl: './provider-edit.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProviderEditComponent implements OnInit {
  private providerService = inject(ProviderService);
  private serviceService = inject(ServiceService);
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
      case 'email': return 'email';
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
    this.isSaving.set(true);

    const createDto: CreateCustomFieldDto = {
      providerId: this.providerId(),
      fieldName: fieldData.fieldName,
      fieldValue: fieldData.fieldValue,
      fieldType: fieldData.fieldType,
      description: fieldData.description,
      displayOrder: fieldData.displayOrder
    };

    // Console log para debug
    console.log('📤 Enviando creación de campo personalizado:', {
      url: 'v1/ProviderCustomFields',
      method: 'POST',
      body: createDto
    });

    this.providerService.createCustomField(createDto).subscribe({
      next: (response) => {
        console.log('✅ Respuesta de creación exitosa:', response);
        
        if (response.isSuccess && response.data) {
          // Agregar el campo al FormArray con el ID retornado del servidor
          const fieldValue = this.getFieldValueByTypeFromString(response.data.fieldValue, response.data.fieldType);
          
          this.customFields.push(this.fb.group({
            id: [response.data.id],
            fieldName: [response.data.fieldName, Validators.required],
            fieldValue: [fieldValue, Validators.required],
            fieldType: [response.data.fieldType],
            description: [response.data.description],
            displayOrder: [response.data.displayOrder]
          }));

          // Reordenar campos por displayOrder
          this.sortCustomFields();

          this.snackBar.open('Campo personalizado creado exitosamente', 'Cerrar', {
            duration: 3000
          });
        } else {
          this.snackBar.open(response.message || 'Error al crear el campo', 'Cerrar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
        this.isSaving.set(false);
      },
      error: (error) => {
        console.error('❌ Error al crear campo personalizado:', error);
        
        const errorMsg = error.error?.message || 'Error al conectar con el servidor';
        this.snackBar.open(errorMsg, 'Cerrar', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.isSaving.set(false);
      }
    });
  }

  updateCustomField(index: number, fieldData: any): void {
    const customFieldId = this.customFields.at(index).get('id')?.value;
    
    if (!customFieldId) {
      this.snackBar.open('Error: ID del campo no encontrado', 'Cerrar', {
        duration: 5000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.isSaving.set(true);

    const updateDto: UpdateCustomFieldCommandDto = {
      id: customFieldId,
      providerId: this.providerId(),
      fieldName: fieldData.fieldName,
      fieldValue: fieldData.fieldValue,
      fieldType: fieldData.fieldType,
      description: fieldData.description,
      displayOrder: fieldData.displayOrder
    };

    // Console log para debug
    console.log('📤 Enviando actualización de campo personalizado:', {
      url: `v1/ProviderCustomFields/${customFieldId}`,
      method: 'PUT',
      body: updateDto
    });

    this.providerService.updateCustomField(customFieldId, updateDto).subscribe({
      next: (response) => {
        console.log('✅ Respuesta de actualización exitosa:', response);
        
        if (response.isSuccess && response.data) {
          // Actualizar el campo en el FormArray
          const fieldValue = this.getFieldValueByTypeFromString(response.data.fieldValue, response.data.fieldType);
          
          this.customFields.at(index).patchValue({
            fieldName: response.data.fieldName,
            fieldValue: fieldValue,
            fieldType: response.data.fieldType,
            description: response.data.description,
            displayOrder: response.data.displayOrder
          });

          // Reordenar campos por displayOrder
          this.sortCustomFields();

          this.snackBar.open('Campo personalizado actualizado exitosamente', 'Cerrar', {
            duration: 3000
          });
        } else {
          this.snackBar.open(response.message || 'Error al actualizar el campo', 'Cerrar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
        this.isSaving.set(false);
      },
      error: (error) => {
        console.error('❌ Error al actualizar campo personalizado:', error);
        
        const errorMsg = error.error?.message || 'Error al conectar con el servidor';
        this.snackBar.open(errorMsg, 'Cerrar', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.isSaving.set(false);
      }
    });
  }

  deleteCustomField(index: number): void {
    const fieldName = this.customFields.at(index).get('fieldName')?.value;
    const customFieldId = this.customFields.at(index).get('id')?.value;
    
    if (!customFieldId) {
      this.snackBar.open('Error: ID del campo no encontrado', 'Cerrar', {
        duration: 5000,
        panelClass: ['error-snackbar']
      });
      return;
    }
    
    if (confirm(`¿Está seguro de eliminar el campo "${fieldName}"?`)) {
      this.isSaving.set(true);

      // Console log para debug
      console.log('📤 Enviando eliminación de campo personalizado:', {
        url: `v1/ProviderCustomFields/${customFieldId}`,
        method: 'DELETE',
        customFieldId: customFieldId
      });

      this.providerService.deleteCustomField(customFieldId).subscribe({
        next: (response) => {
          console.log('✅ Respuesta de eliminación exitosa:', response);
          
          if (response.isSuccess) {
            this.customFields.removeAt(index);
            this.snackBar.open('Campo personalizado eliminado exitosamente', 'Cerrar', {
              duration: 3000
            });
          } else {
            this.snackBar.open(response.message || 'Error al eliminar el campo', 'Cerrar', {
              duration: 5000,
              panelClass: ['error-snackbar']
            });
          }
          this.isSaving.set(false);
        },
        error: (error) => {
          console.error('❌ Error al eliminar campo personalizado:', error);
          
          const errorMsg = error.error?.message || 'Error al conectar con el servidor';
          this.snackBar.open(errorMsg, 'Cerrar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
          this.isSaving.set(false);
        }
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

      // Solo actualizar información básica del proveedor
      // Los campos personalizados se manejan con endpoints separados
      const updateData: UpdateProviderDto = {
        nit: this.providerForm.value.nit,
        name: this.providerForm.value.name,
        email: this.providerForm.value.email,
        customFields: [] // No se envían campos personalizados aquí
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

  openCreateServiceDialog(): void {
    if (!this.provider()) {
      return;
    }

    const dialogRef = this.dialog.open(ServiceDialogComponent, {
      width: '700px',
      maxWidth: '90vw',
      disableClose: true,
      data: {
        providerId: this.providerId(),
        providerName: this.provider()!.name
      } as ServiceDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createService(result);
      }
    });
  }

  createService(serviceData: CreateServiceDto): void {
    this.isSaving.set(true);

    console.log('📤 Enviando creación de servicio:', {
      url: 'v1/services',
      method: 'POST',
      body: serviceData
    });

    this.serviceService.createService(serviceData).subscribe({
      next: (response) => {
        console.log('✅ Respuesta de creación exitosa:', response);
        
        if (response.isSuccess && response.data) {
          // Recargar el proveedor para obtener los servicios actualizados
          this.loadProvider(this.providerId());
          
          this.snackBar.open('Servicio creado exitosamente', 'Cerrar', {
            duration: 3000
          });
        } else {
          this.snackBar.open(response.message || 'Error al crear el servicio', 'Cerrar', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
        this.isSaving.set(false);
      },
      error: (error) => {
        console.error('❌ Error al crear servicio:', error);
        
        const errorMsg = error.error?.message || 'Error al conectar con el servidor';
        this.snackBar.open(errorMsg, 'Cerrar', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.isSaving.set(false);
      }
    });
  }
}

