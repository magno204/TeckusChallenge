# Arquitectura del Proyecto Angular

Este proyecto sigue la **Arquitectura Standalone con organización por Features**, recomendada para Angular 14+.

## 📁 Estructura de Carpetas

```
src/app/
├── core/                    # Servicios singleton y recursos globales
│   ├── guards/             # Guards de ruta (auth, autorización)
│   ├── interceptors/       # Interceptores HTTP
│   └── services/           # Servicios singleton (API, config)
│
├── shared/                  # Recursos compartidos
│   ├── components/         # Componentes reutilizables
│   ├── pipes/              # Pipes personalizados
│   └── directives/         # Directivas personalizadas
│
├── features/                # Módulos de funcionalidad
│   ├── providers/          # Feature de Proveedores
│   │   ├── components/     # Componentes específicos
│   │   ├── services/       # Servicios específicos
│   │   ├── models/         # Interfaces y modelos
│   │   └── providers.routes.ts
│   └── services/            # Feature de Servicios
│       ├── components/
│       ├── services/
│       ├── models/
│       └── services.routes.ts
│
├── layout/                  # Componentes de layout
│   ├── header/
│   ├── sidebar/
│   └── footer/
│
├── app.config.ts           # Configuración de la aplicación
└── app.routes.ts           # Rutas principales
```

## 🎯 Principios de la Arquitectura

### Core
- **Propósito**: Servicios y recursos que se cargan una sola vez (singleton)
- **Reglas**:
  - Solo servicios singleton
  - No debe importar módulos de features
  - Puede ser importado por cualquier módulo

### Shared
- **Propósito**: Recursos reutilizables sin lógica de negocio
- **Reglas**:
  - Componentes, pipes y directivas genéricas
  - Sin lógica de negocio específica
  - Independiente de features

### Features
- **Propósito**: Módulos de funcionalidad independientes
- **Reglas**:
  - Cada feature es independiente
  - Pueden importar de `core` y `shared`
  - NO deben importar entre sí directamente
  - Cada feature tiene sus propias rutas

### Layout
- **Propósito**: Componentes de estructura visual
- **Uso**: Header, sidebar, footer, etc.

## 🚀 Cómo Usar

### Agregar un nuevo Feature

1. Crear la estructura de carpetas:
```bash
mkdir -p src/app/features/nuevo-feature/{components,services,models}
```

2. Crear el archivo de rutas:
```typescript
// features/nuevo-feature/nuevo-feature.routes.ts
import { Routes } from '@angular/router';

export const nuevoFeatureRoutes: Routes = [
  // tus rutas aquí
];
```

3. Importar en `app.routes.ts`:
```typescript
import { nuevoFeatureRoutes } from './features/nuevo-feature/nuevo-feature.routes';

export const routes: Routes = [
  {
    path: 'nuevo-feature',
    children: nuevoFeatureRoutes
  }
];
```

### Usar el servicio API base

```typescript
import { ApiService } from '@app/core/services/api.service';

constructor(private api: ApiService) {}

getProviders() {
  return this.api.get('providers');
}
```

### Usar Guards

```typescript
import { authGuard } from '@app/core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'dashboard',
    canActivate: [authGuard],
    component: DashboardComponent
  }
];
```

## 📝 Notas Importantes

- Todos los componentes son **standalone** (sin módulos)
- Usa **lazy loading** para features grandes
- Los interceptores están configurados en `app.config.ts`
- El servicio API base está listo para usar (ajusta la URL según tu backend)

