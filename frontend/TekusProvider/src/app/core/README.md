# Core Module

Esta carpeta contiene servicios, guards e interceptors que son singleton y se cargan una sola vez en la aplicación.

## Estructura:
- **guards/**: Guards de ruta (autenticación, autorización, etc.)
- **interceptors/**: Interceptores HTTP (errores, autenticación, etc.)
- **services/**: Servicios singleton (API, autenticación, configuración, etc.)

## Reglas:
- Solo debe contener servicios que son singleton
- No debe importar módulos de features
- Puede ser importado por cualquier módulo de la aplicación

