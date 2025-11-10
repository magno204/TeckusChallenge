# TekusChallenge

## Descripción del Proyecto

TekusChallenge es una API REST desarrollada con **.NET 9.0** que implementa una arquitectura limpia (Clean Architecture) para la gestión de proveedores, servicios y países. El proyecto está diseñado siguiendo las mejores prácticas de desarrollo de software, con separación de responsabilidades y escalabilidad como principios fundamentales.

## Características Principales

### 🏗️ Arquitectura

- **Clean Architecture**: Implementación de arquitectura limpia con separación en capas:
  - **Domain Layer**: Entidades y contratos del dominio de negocio
  - **Application Layer**: Casos de uso, DTOs y lógica de aplicación
  - **Infrastructure Layer**: Implementaciones de repositorios, servicios externos y acceso a datos
  - **API Layer**: Controladores, middleware y configuración de la aplicación
  - **Transversal Layer**: Componentes compartidos y respuestas genéricas

### 🔧 Tecnologías y Frameworks

- **.NET 9.0**: Framework principal de desarrollo
- **Entity Framework Core 9.0**: ORM para acceso a datos con SQL Server
- **MediatR 12.5.0**: Implementación del patrón CQRS (Command Query Responsibility Segregation)
- **AutoMapper 14.0**: Mapeo automático entre entidades y DTOs
- **FluentValidation 12.1**: Validación de datos mediante reglas fluidas
- **JWT Bearer Authentication**: Autenticación basada en tokens JWT
- **API Versioning**: Control de versiones de API con Asp.Versioning.Mvc
- **Swagger/OpenAPI**: Documentación interactiva de la API con Swashbuckle y ReDoc
- **Rate Limiting**: Control de límites de solicitudes para protección de la API

### ☁️ Integración con Azure

- **Azure Key Vault**: Gestión segura de secretos y configuraciones sensibles
- **Azure Web App**: Despliegue automatizado en Azure App Service
- **Azure Identity**: Autenticación con credenciales de Azure

### 🔄 CI/CD

- **GitHub Actions**: Pipeline de integración y despliegue continuo
  - Compilación automática en cada push a la rama `master`
  - Ejecución de pruebas unitarias
  - Publicación de artefactos
  - Despliegue automático a Azure Web App

### 🗄️ Base de Datos

- **SQL Server**: Base de datos relacional
- **Migraciones Automáticas**: Aplicación automática de migraciones al iniciar la aplicación
- **Entity Framework Migrations**: Gestión de esquema de base de datos mediante código

### 🛡️ Seguridad y Protección

- **Autenticación JWT**: Sistema de autenticación basado en tokens
- **Rate Limiting**: Protección contra abuso y sobrecarga
- **CORS**: Configuración de políticas de origen cruzado
- **HTTPS Redirection**: Redirección automática a conexiones seguras
- **Global Exception Handling**: Manejo centralizado de excepciones

### 📚 Documentación

- **Swagger UI**: Interfaz interactiva para explorar y probar la API
- **ReDoc**: Documentación alternativa con diseño limpio y legible
- **Documentación XML**: Generación automática de documentación desde comentarios XML

### 🧪 Testing

- **Unit Tests**: Suite de pruebas unitarias para validar la lógica de negocio

### 🌐 Funcionalidades de Negocio

- **Gestión de Proveedores**: CRUD completo para proveedores con campos personalizados
- **Gestión de Servicios**: Administración de servicios asociados a proveedores
- **Gestión de Países**: Integración con RestCountries API para obtener información de países
- **Campos Personalizados**: Sistema flexible de campos personalizados para proveedores
- **Relaciones Servicio-País**: Asociación de servicios con múltiples países

## Estructura del Proyecto

```
TekusChallenge/
├── backend/
│   ├── src/
│   │   ├── TekusChallenge.API/          # Capa de presentación
│   │   ├── TekusChallenge.Application/  # Capa de aplicación
│   │   ├── TekusChallenge.Domain/       # Capa de dominio
│   │   └── TekusChallenge.Infrastructure/ # Capa de infraestructura
│   └── tests/
│       └── TekusChallenge.UnitTests/    # Pruebas unitarias
├── Tekus.Transversal/                   # Componentes transversales
└── .github/
    └── workflows/                        # Pipelines de CI/CD
```

## Requisitos

- .NET 9.0 SDK
- SQL Server (o Azure SQL Database)
- Azure Subscription (para despliegue)
- GitHub Actions (para CI/CD)

## Configuración

La aplicación utiliza Azure Key Vault para la gestión de configuraciones sensibles. Asegúrese de configurar las siguientes variables:

- `KeyVaultName`: Nombre del Key Vault de Azure
- Credenciales de Azure para autenticación con Key Vault

## Despliegue

El despliegue se realiza automáticamente mediante GitHub Actions cuando se hace push a la rama `master`. El pipeline incluye:

1. Restauración de dependencias
2. Compilación del proyecto
3. Ejecución de pruebas
4. Publicación de artefactos
5. Despliegue a Azure Web App

## Documentación de la API

Una vez desplegada la aplicación, la documentación interactiva está disponible en:
- Swagger UI: `/swagger`
- ReDoc: `/redoc`

## Licencia

Este proyecto es parte del desafío técnico de Tekus.