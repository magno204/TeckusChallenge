import { Component, inject, signal, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { CustomField } from '../models/provider.models';

export interface CustomFieldDialogData {
  field?: CustomField;
  mode: 'create' | 'edit';
  existingDisplayOrders: number[];
}

@Component({
  selector: 'app-custom-field-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule
  ],
  templateUrl: './custom-field-dialog.component.html',
  styleUrl: './custom-field-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomFieldDialogComponent implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CustomFieldDialogComponent>);
  data = inject<CustomFieldDialogData>(MAT_DIALOG_DATA);

  customFieldForm: FormGroup;
  isEditMode = signal(false);

  fieldTypes = [
    { value: 'text', label: 'Texto', icon: 'text_fields' },
    { value: 'url', label: 'URL', icon: 'link' },
    { value: 'date', label: 'Fecha', icon: 'calendar_today' },
    { value: 'boolean', label: 'Verdadero/Falso', icon: 'toggle_on' },
    { value: 'number', label: 'Número', icon: 'numbers' }
  ];

  constructor() {
    this.customFieldForm = this.fb.group({
      fieldName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      fieldValue: ['', Validators.required],
      fieldType: ['text', Validators.required],
      description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(200)]],
      displayOrder: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.isEditMode.set(this.data.mode === 'edit');
    
    if (this.isEditMode() && this.data.field) {
      this.customFieldForm.patchValue({
        fieldName: this.data.field.fieldName,
        fieldValue: this.data.field.fieldValue,
        fieldType: this.data.field.fieldType,
        description: this.data.field.description,
        displayOrder: this.data.field.displayOrder
      });
    } else {
      // Para nuevo campo, sugerir el siguiente displayOrder
      const maxOrder = Math.max(0, ...this.data.existingDisplayOrders);
      this.customFieldForm.patchValue({
        displayOrder: maxOrder + 1
      });
    }
  }

  onSubmit(): void {
    if (this.customFieldForm.valid) {
      const result = {
        ...this.customFieldForm.value,
        id: this.data.field?.id || null
      };
      this.dialogRef.close(result);
    } else {
      this.markFormGroupTouched(this.customFieldForm);
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
    const control = this.customFieldForm.get(fieldName);
    
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

