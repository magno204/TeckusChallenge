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

## Frontend - TekusProvider

### 🎨 Descripción

**TekusProvider** es una aplicación web moderna desarrollada con **Angular 20.3** que proporciona una interfaz de usuario intuitiva y responsive para la gestión de proveedores, servicios y países. La aplicación implementa las mejores prácticas de Angular con componentes standalone, signals para gestión de estado, y Angular Material para una experiencia de usuario consistente.

### 🔧 Tecnologías Frontend

- **Angular 20.3**: Framework principal de desarrollo
- **Angular Material 20.2**: Biblioteca de componentes UI con Material Design
- **Angular CDK 20.2**: Component Dev Kit para funcionalidades avanzadas
- **TypeScript 5.9**: Lenguaje de programación tipado
- **RxJS 7.8**: Programación reactiva con observables
- **Jasmine/Karma**: Framework de testing

### 🏗️ Arquitectura Frontend

La aplicación sigue una arquitectura modular basada en features:

- **Standalone Components**: Todos los componentes son standalone (sin NgModules)
- **Signals**: Gestión de estado reactivo con signals de Angular
- **Lazy Loading**: Carga diferida de módulos por rutas
- **Change Detection OnPush**: Optimización de rendimiento
- **Reactive Forms**: Formularios reactivos para validación robusta

### 📂 Estructura del Frontend

```
frontend/TekusProvider/
├── src/
│   ├── app/
│   │   ├── core/                    # Funcionalidades core
│   │   │   ├── guards/              # Guards de rutas (autenticación)
│   │   │   ├── interceptors/        # Interceptores HTTP
│   │   │   ├── models/              # Modelos de datos core
│   │   │   └── services/            # Servicios core (auth, api)
│   │   ├── features/                # Módulos por funcionalidad
│   │   │   ├── auth/                # Autenticación y login
│   │   │   ├── dashboard/           # Dashboard y estadísticas
│   │   │   ├── providers/           # Gestión de proveedores
│   │   │   └── services/            # Gestión de servicios
│   │   ├── layout/                  # Componentes de layout
│   │   │   ├── header/              # Cabecera de la aplicación
│   │   │   ├── sidebar/             # Menú lateral
│   │   │   └── main-layout/         # Layout principal
│   │   └── shared/                  # Componentes compartidos
│   │       ├── components/          # Componentes reutilizables
│   │       ├── directives/          # Directivas personalizadas
│   │       └── pipes/               # Pipes personalizados
│   ├── environments/                # Configuración de entornos
│   └── styles.css                   # Estilos globales
└── public/                          # Archivos estáticos
```

### ✨ Características Frontend

- **🔐 Autenticación JWT**: Sistema completo de login con tokens
- **📊 Dashboard Interactivo**: Visualización de estadísticas y métricas
- **👥 Gestión de Proveedores**: CRUD completo con interfaz intuitiva
- **🛠️ Gestión de Servicios**: Administración de servicios y países asociados
- **🎯 Campos Personalizados**: Sistema flexible de campos personalizados
- **🎨 Material Design**: Interfaz moderna y profesional con Angular Material
- **📱 Responsive Design**: Adaptable a diferentes tamaños de pantalla
- **🔄 Estado Reactivo**: Gestión de estado con signals
- **⚡ Carga Optimizada**: Lazy loading de rutas para mejor rendimiento
- **🛡️ Interceptores HTTP**: Manejo automático de autenticación y errores

### 🚀 Comandos de Desarrollo

#### Instalar Dependencias
```bash
cd frontend/TekusProvider
npm install
```

#### Ejecutar en Desarrollo
```bash
npm start
# La aplicación estará disponible en http://localhost:4200
```

#### Compilar para Producción
```bash
npm run build
# Los archivos compilados estarán en la carpeta dist/
```

#### Ejecutar Pruebas
```bash
npm test
```

### ⚙️ Configuración del Frontend

La aplicación utiliza archivos de configuración de entorno ubicados en `src/environments/`:

- **`environment.development.ts`**: Configuración para desarrollo local
- **`environment.ts`**: Configuración para producción

Asegúrese de configurar la URL del API backend en estos archivos antes de ejecutar la aplicación.

## Estructura del Proyecto Completo

```
TekusChallenge/
├── backend/
│   ├── src/
│   │   ├── TekusChallenge.API/          # Capa de presentación
│   │   ├── TekusChallenge.Application/  # Capa de aplicación
│   │   ├── TekusChallenge.Domain/       # Capa de dominio
│   │   ├── TekusChallenge.Infrastructure/ # Capa de infraestructura
│   │   └── Tekus.Transversal/           # Componentes transversales
│   └── tests/
│       └── TekusChallenge.UnitTests/    # Pruebas unitarias
├── frontend/
│   └── TekusProvider/                   # Aplicación Angular
│       ├── src/                         # Código fuente
│       ├── public/                      # Archivos estáticos
│       ├── angular.json                 # Configuración de Angular
│       └── package.json                 # Dependencias y scripts
├── database/                            # Scripts SQL de base de datos
│   ├── CreateDatabase.sql               # Creación de BD, tablas e índices
│   └── InsertData.sql                   # Datos iniciales de prueba
└── .github/
    └── workflows/                       # Pipelines de CI/CD
```

## Requisitos

### Backend
- .NET 9.0 SDK
- SQL Server (o Azure SQL Database)
- Azure Subscription (para despliegue en producción)

### Frontend
- Node.js 18+ y npm
- Angular CLI 20.3+

### DevOps
- GitHub Actions (para CI/CD)

## 🗄️ Scripts SQL de Base de Datos

El proyecto incluye scripts SQL para crear y poblar la base de datos. Los scripts se encuentran en la carpeta `database/`:

### Scripts Disponibles

1. **`CreateDatabase.sql`**: Crea la base de datos, tablas e índices
2. **`InsertData.sql`**: Inserta datos iniciales de prueba

### Instalación

Ejecuta los scripts en orden en SQL Server Management Studio (SSMS):

1. Abre **`CreateDatabase.sql`** y ejecútalo (`F5`)
2. Abre **`InsertData.sql`** y ejecútalo (`F5`)

### Datos Iniciales Incluidos

- ✅ 10 países
- ✅ 8 proveedores de servicios tecnológicos
- ✅ 30 campos personalizados
- ✅ 15 servicios diversos
- ✅ 52 relaciones servicio-país

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