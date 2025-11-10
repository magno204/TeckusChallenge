# Features Module

Esta carpeta contiene los módulos de funcionalidad de la aplicación. Cada feature es independiente y puede tener sus propias rutas, componentes, servicios y modelos.

## Estructura por Feature:
```
feature-name/
├── components/     # Componentes específicos del feature
├── services/       # Servicios específicos del feature
├── models/         # Interfaces y modelos TypeScript
└── feature.routes.ts  # Rutas del feature
```

## Features actuales:
- **providers/**: Gestión de proveedores
- **services/**: Gestión de servicios

## Reglas:
- Cada feature debe ser independiente
- Las features pueden importar de `core` y `shared`
- Las features NO deben importar entre sí directamente
- Cada feature debe tener su propio archivo de rutas

