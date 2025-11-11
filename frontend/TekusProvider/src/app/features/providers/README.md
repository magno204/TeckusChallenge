# Módulo de Proveedores

Este módulo gestiona la funcionalidad relacionada con los proveedores del sistema.

## Componentes

### ProvidersListComponent
**Ubicación:** `components/providers-list.component.ts`

Muestra un listado de todos los proveedores en formato de tabla con las siguientes columnas:
- Nombre
- NIT
- Email
- Acciones (botón para editar)

**Características:**
- Carga dinámica de datos desde el backend
- Estados de carga y error
- Navegación a la página de edición mediante botón de acción

**Ruta:** `/providers`

---

### ProviderEditComponent
**Ubicación:** `components/provider-edit.component.ts`

Permite editar la información de un proveedor existente.

---

### CustomFieldDialogComponent
**Ubicación:** `components/custom-field-dialog.component.ts`

Componente modal para crear y editar campos personalizados.

**Características:**
- Formulario reactivo con validaciones
- Selector de tipo de campo (text, url, date, boolean, number)
- Validación de orden de visualización
- Sugerencia automática de displayOrder para nuevos campos
- Modos: creación y edición

**Campos:**
- **Nombre del Campo**: Identificador del campo (3-100 caracteres)
- **Tipo de Campo**: Tipo de dato del campo
- **Valor Inicial**: Valor por defecto del campo
- **Descripción**: Descripción del propósito (5-200 caracteres)
- **Orden de Visualización**: Número para ordenar campos (mínimo 1)

**Características:**
- Formulario reactivo con validaciones
- Validación en tiempo real de campos
- Mensajes de error descriptivos
- Estados de carga durante la obtención y guardado de datos
- Notificaciones mediante snackbar
- Información de auditoría (creación y última actualización)
- **Gestión completa de campos personalizados:**
  - Crear nuevos campos personalizados
  - Editar campos existentes
  - Eliminar campos personalizados
  - Reordenamiento automático por displayOrder
- Componente modal para gestión de campos personalizados
- Diseño responsivo

**Campos editables:**

**Información Básica:**
- **NIT**: Número de identificación tributaria (5-20 caracteres, requerido)
- **Nombre**: Nombre del proveedor (3-100 caracteres, requerido)
- **Email**: Correo electrónico (formato email válido, requerido)

**Campos Personalizados (Custom Fields):**
El componente maneja dinámicamente los campos personalizados según su tipo:
- **text**: Campo de texto simple
- **url**: Campo de URL con validación
- **date**: Selector de fecha con datepicker
- **boolean**: Toggle switch (sí/no)
- **number**: Campo numérico

Los campos personalizados se muestran en un panel expansible ordenados por `displayOrder`.

**Servicios Asociados:**
Se muestra una lista de solo lectura de los servicios asociados al proveedor con:
- Nombre del servicio
- Descripción
- Tarifa por hora
- Fecha de creación

**Ruta:** `/providers/edit/:id`

**Parámetros de ruta:**
- `id`: ID único del proveedor a editar

**Acciones:**
- **Guardar Cambios**: Actualiza el proveedor y redirige al listado
- **Cancelar**: Descarta los cambios y vuelve al listado
- **Agregar Campo Personalizado**: Abre modal para crear un nuevo campo
- **Editar Campo**: Abre modal para modificar un campo existente
- **Eliminar Campo**: Elimina un campo personalizado con confirmación

---

## Modelos

### Provider
```typescript
interface Provider {
  id: string;
  nit: string;
  name: string;
  email: string;
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
  customFields: CustomField[];
  services: Service[];
}
```

### CustomField
```typescript
interface CustomField {
  id: string;
  providerId: string;
  fieldName: string;
  fieldValue: string;
  fieldType: 'text' | 'url' | 'date' | 'boolean' | 'number';
  description: string;
  displayOrder: number;
  createdAt: string;
  updatedAt: string | null;
}
```

### Service
```typescript
interface Service {
  id: string;
  name: string;
  hourlyRate: number;
  description: string;
  providerId: string;
  providerName: string;
  countries: any[];
  createdAt: string;
  createdBy: string;
  updatedAt: string | null;
  updatedBy: string | null;
}
```

### UpdateProviderDto
```typescript
interface UpdateProviderDto {
  nit: string;
  name: string;
  email: string;
  customFields: UpdateCustomFieldDto[];
}
```

### UpdateCustomFieldDto
```typescript
interface UpdateCustomFieldDto {
  fieldName: string;
  fieldValue: string;
  fieldType: string;
  description: string;
  displayOrder: number;
}
```

---

## Servicios

### ProviderService
**Ubicación:** `services/provider.service.ts`

Gestiona las peticiones HTTP relacionadas con proveedores.

**Métodos:**
- `getProviders()`: Obtiene el listado paginado de proveedores
- `getProviderById(id: string)`: Obtiene un proveedor específico por su ID
- `updateProvider(id: string, data: UpdateProviderDto)`: Actualiza la información de un proveedor

---

## Rutas

```typescript
const providersRoutes: Routes = [
  {
    path: '',
    component: ProvidersListComponent
  },
  {
    path: 'edit/:id',
    component: ProviderEditComponent
  }
];
```

---

## Flujo de navegación

1. Usuario accede a `/providers` → Se muestra el listado de proveedores
2. Usuario hace clic en el botón de editar de un proveedor
3. Navegación a `/providers/edit/{id}`
4. Se carga la información del proveedor
5. Usuario modifica los campos necesarios
6. Opciones:
   - **Guardar**: Se actualiza el proveedor y se redirige al listado con mensaje de éxito
   - **Cancelar**: Se descarta los cambios y se vuelve al listado

---

## Validaciones

### Campo NIT
- Requerido
- Longitud mínima: 5 caracteres
- Longitud máxima: 20 caracteres

### Campo Nombre
- Requerido
- Longitud mínima: 3 caracteres
- Longitud máxima: 100 caracteres

### Campo Email
- Requerido
- Debe ser un email válido
- Longitud máxima: 100 caracteres

---

## Tecnologías utilizadas

- **Angular** (Standalone Components)
- **Angular Material** (UI Components):
  - MatCard, MatFormField, MatInput
  - MatButton, MatIcon, MatTooltip
  - MatExpansionPanel
  - MatDatepicker
  - MatSlideToggle
  - MatSnackBar
  - MatSpinner
- **Reactive Forms** (Formularios con FormArray)
- **Signals** (State Management)
- **RxJS** (Manejo de observables)
- **TypeScript** (Tipado estático)

---

## Mejoras futuras

- [ ] Implementar paginación en el listado
- [ ] Agregar filtros de búsqueda
- [ ] Implementar funcionalidad de eliminación de proveedores
- [ ] Agregar funcionalidad para crear nuevos proveedores
- [x] Gestionar campos personalizados (customFields) - ✅ Completado
- [ ] Gestionar servicios del proveedor (crear, editar, eliminar)
- [ ] Agregar confirmación antes de salir con cambios sin guardar
- [ ] Validación de URLs en campos tipo URL
- [ ] Drag and drop para reordenar campos personalizados
- [ ] Importar/Exportar configuración de campos personalizados

