-- =============================================
-- Script de Inserción de Datos Iniciales
-- TekusChallenge - Sistema de Gestión de Proveedores y Servicios
-- =============================================

USE Tekus;
GO

-- =============================================
-- IMPORTANTE: Los países se sincronizan normalmente desde la API REST Countries
-- mediante el endpoint /api/v1/countries/sync
-- Sin embargo, aquí incluimos algunos países iniciales para facilitar las pruebas
-- =============================================

PRINT 'Insertando datos iniciales...';
GO

-- =============================================
-- Insertar Países Iniciales
-- =============================================
INSERT INTO Countries (Code, CodeAlpha3, Name, Flag) VALUES
('CO', 'COL', 'Colombia', 'https://flagcdn.com/w320/co.png'),
('US', 'USA', 'United States', 'https://flagcdn.com/w320/us.png'),
('MX', 'MEX', 'Mexico', 'https://flagcdn.com/w320/mx.png'),
('PE', 'PER', 'Peru', 'https://flagcdn.com/w320/pe.png'),
('CL', 'CHL', 'Chile', 'https://flagcdn.com/w320/cl.png'),
('AR', 'ARG', 'Argentina', 'https://flagcdn.com/w320/ar.png'),
('BR', 'BRA', 'Brazil', 'https://flagcdn.com/w320/br.png'),
('EC', 'ECU', 'Ecuador', 'https://flagcdn.com/w320/ec.png'),
('VE', 'VEN', 'Venezuela', 'https://flagcdn.com/w320/ve.png'),
('ES', 'ESP', 'Spain', 'https://flagcdn.com/w320/es.png');
GO

PRINT 'Países iniciales insertados: 10 registros';
GO

-- =============================================
-- Insertar Proveedores de Servicios
-- =============================================

DECLARE @Provider1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider3 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider4 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider5 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider6 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider7 UNIQUEIDENTIFIER = NEWID();
DECLARE @Provider8 UNIQUEIDENTIFIER = NEWID();

-- Provider 1: TechSolutions Colombia S.A.S
SET @Provider1 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider1, '9001234567', 'TechSolutions Colombia S.A.S', 'contacto@techsolutions.com.co', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider1, 'Ciudad', 'Bogotá', 'text', 'Ciudad principal de operaciones', 1, GETUTCDATE(), 'System'),
(@Provider1, 'Teléfono', '3001234567', 'text', 'Número de contacto principal', 2, GETUTCDATE(), 'System'),
(@Provider1, 'Sitio Web', 'https://www.techsolutions.com.co', 'url', 'Página web corporativa', 3, GETUTCDATE(), 'System'),
(@Provider1, 'Años de Experiencia', '15', 'number', 'Años en el mercado', 4, GETUTCDATE(), 'System');

-- Provider 2: Consultoría Empresarial Ltda
SET @Provider2 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider2, '8005556789', 'Consultoría Empresarial Ltda', 'info@consultoriaempresarial.com', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider2, 'Especialidad', 'Gestión de Proyectos y Transformación Digital', 'text', 'Área de especialización', 1, GETUTCDATE(), 'System'),
(@Provider2, 'Email Comercial', 'ventas@consultoriaempresarial.com', 'email', 'Correo del departamento comercial', 2, GETUTCDATE(), 'System');

-- Provider 3: CloudServe Pro
SET @Provider3 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider3, '8003214567', 'CloudServe Pro', 'soporte@cloudservepro.io', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider3, 'Servicios Principales', 'AWS, Azure, Google Cloud', 'text', 'Plataformas cloud soportadas', 1, GETUTCDATE(), 'System'),
(@Provider3, 'Certificación ISO', 'true', 'boolean', '¿Cuenta con certificación ISO 27001?', 2, GETUTCDATE(), 'System'),
(@Provider3, 'Fecha de Certificación', '2023-05-15', 'date', 'Fecha de última certificación', 3, GETUTCDATE(), 'System'),
(@Provider3, 'Portal de Soporte', 'https://support.cloudservepro.io', 'url', 'URL del portal de soporte técnico', 4, GETUTCDATE(), 'System');

-- Provider 4: DevFactory Solutions
SET @Provider4 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider4, '8001112223', 'DevFactory Solutions', 'hello@devfactory.dev', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider4, 'Tecnologías', '.NET, React, Angular, Node.js', 'text', 'Stack tecnológico principal', 1, GETUTCDATE(), 'System'),
(@Provider4, 'Equipo de Desarrollo', '50', 'number', 'Número de desarrolladores', 2, GETUTCDATE(), 'System'),
(@Provider4, 'GitHub', 'https://github.com/devfactory', 'url', 'Perfil de GitHub corporativo', 3, GETUTCDATE(), 'System'),
(@Provider4, 'Fecha de Fundación', '2018-03-10', 'date', 'Fecha de inicio de operaciones', 4, GETUTCDATE(), 'System');

-- Provider 5: SecureIT Networks
SET @Provider5 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider5, '9002223334', 'SecureIT Networks', 'security@secureit.com.co', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider5, 'Certificaciones', 'ISO 27001, SOC 2, PCI DSS', 'text', 'Certificaciones de seguridad', 1, GETUTCDATE(), 'System'),
(@Provider5, 'Soporte 24/7', 'true', 'boolean', '¿Ofrece soporte continuo?', 2, GETUTCDATE(), 'System'),
(@Provider5, 'Email SOC', 'soc@secureit.com.co', 'email', 'Centro de Operaciones de Seguridad', 3, GETUTCDATE(), 'System'),
(@Provider5, 'Tiempo de Respuesta (horas)', '2', 'number', 'SLA de respuesta a incidentes', 4, GETUTCDATE(), 'System');

-- Provider 6: Academia Tech Training
SET @Provider6 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider6, '9006667778', 'Academia Tech Training', 'info@academiatech.edu.co', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider6, 'Modalidad', 'Presencial, Virtual, Híbrida', 'text', 'Modalidades de formación disponibles', 1, GETUTCDATE(), 'System'),
(@Provider6, 'Instructores Certificados', '25', 'number', 'Cantidad de instructores certificados', 2, GETUTCDATE(), 'System'),
(@Provider6, 'Plataforma LMS', 'https://lms.academiatech.edu.co', 'url', 'Sistema de gestión de aprendizaje', 3, GETUTCDATE(), 'System'),
(@Provider6, 'Acreditación Educativa', 'true', 'boolean', '¿Cuenta con acreditación oficial?', 4, GETUTCDATE(), 'System');

-- Provider 7: Mantenimiento Integral 360
SET @Provider7 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider7, '8008889990', 'Mantenimiento Integral 360', 'servicios@mantenimiento360.com', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider7, 'Áreas de Servicio', 'Bogotá, Medellín, Cali, Barranquilla', 'text', 'Ciudades donde presta servicios', 1, GETUTCDATE(), 'System'),
(@Provider7, 'Técnicos Disponibles', '80', 'number', 'Personal técnico disponible', 2, GETUTCDATE(), 'System'),
(@Provider7, 'Contacto Emergencias', 'emergencias@mantenimiento360.com', 'email', 'Email para emergencias', 3, GETUTCDATE(), 'System'),
(@Provider7, 'Inicio de Contrato', '2024-01-15', 'date', 'Fecha de inicio de servicios', 4, GETUTCDATE(), 'System'),
(@Provider7, 'Garantía Extendida', 'true', 'boolean', '¿Ofrece garantía extendida?', 5, GETUTCDATE(), 'System');

-- Provider 8: Telecom Connect S.A.
SET @Provider8 = NEWID();
INSERT INTO Providers (Id, Nit, Name, Email, CreatedAt, CreatedBy) 
VALUES (@Provider8, '9009876543', 'Telecom Connect S.A.', 'ventas@telecomconnect.net', GETUTCDATE(), 'System');

INSERT INTO ProviderCustomFields (ProviderId, FieldName, FieldValue, FieldType, Description, DisplayOrder, CreatedAt, CreatedBy) VALUES
(@Provider8, 'Cobertura Nacional', 'true', 'boolean', '¿Tiene cobertura en todo el país?', 1, GETUTCDATE(), 'System'),
(@Provider8, 'Número de Clientes', '5000', 'number', 'Clientes activos', 2, GETUTCDATE(), 'System'),
(@Provider8, 'Sede Principal', 'Medellín', 'text', 'Ubicación de oficina principal', 3, GETUTCDATE(), 'System');

PRINT 'Proveedores insertados: 8 registros';
PRINT 'Campos personalizados insertados: 30 registros';
GO

-- =============================================
-- Insertar Servicios
-- =============================================

DECLARE @Service1 UNIQUEIDENTIFIER, @Service2 UNIQUEIDENTIFIER, @Service3 UNIQUEIDENTIFIER;
DECLARE @Service4 UNIQUEIDENTIFIER, @Service5 UNIQUEIDENTIFIER, @Service6 UNIQUEIDENTIFIER;
DECLARE @Service7 UNIQUEIDENTIFIER, @Service8 UNIQUEIDENTIFIER, @Service9 UNIQUEIDENTIFIER;
DECLARE @Service10 UNIQUEIDENTIFIER, @Service11 UNIQUEIDENTIFIER, @Service12 UNIQUEIDENTIFIER;
DECLARE @Service13 UNIQUEIDENTIFIER, @Service14 UNIQUEIDENTIFIER, @Service15 UNIQUEIDENTIFIER;

-- Obtener los IDs de los proveedores
DECLARE @DevFactory UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '8001112223');
DECLARE @CloudServe UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '8003214567');
DECLARE @SecureIT UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '9002223334');
DECLARE @Consultoria UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '8005556789');
DECLARE @Academia UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '9006667778');
DECLARE @Mantenimiento UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '8008889990');
DECLARE @Telecom UNIQUEIDENTIFIER = (SELECT Id FROM Providers WHERE Nit = '9009876543');

-- Servicios de DevFactory Solutions
SET @Service1 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service1, 'Desarrollo Web Frontend', 45.00, 'Desarrollo de interfaces de usuario con React, Vue.js y Angular. Incluye diseño responsivo y optimización de rendimiento.', @DevFactory, GETUTCDATE(), 'System');

SET @Service2 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service2, 'Desarrollo Backend .NET', 50.00, 'Desarrollo de APIs REST con .NET Core, implementación de arquitectura limpia y patrones de diseño.', @DevFactory, GETUTCDATE(), 'System');

SET @Service3 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service3, 'Testing y Quality Assurance', 35.00, 'Pruebas funcionales, automatización de pruebas con Selenium y Cypress, testing de performance.', @DevFactory, GETUTCDATE(), 'System');

SET @Service4 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service4, 'Desarrollo Mobile Multiplataforma', 55.00, 'Desarrollo de aplicaciones móviles con React Native y Flutter para iOS y Android.', @DevFactory, GETUTCDATE(), 'System');

SET @Service5 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service5, 'Diseño UI/UX', 40.00, 'Diseño de interfaces, experiencia de usuario, prototipado con Figma, investigación de usuarios.', @DevFactory, GETUTCDATE(), 'System');

SET @Service6 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service6, 'Machine Learning e IA', 90.00, 'Desarrollo de modelos de ML, análisis predictivo, procesamiento de lenguaje natural, visión por computadora.', @DevFactory, GETUTCDATE(), 'System');

-- Servicios de CloudServe Pro
SET @Service7 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service7, 'Consultoría Cloud AWS', 75.00, 'Asesoría en arquitectura cloud, migración a AWS, optimización de costos y mejores prácticas de seguridad.', @CloudServe, GETUTCDATE(), 'System');

SET @Service8 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service8, 'DevOps y CI/CD', 65.00, 'Implementación de pipelines CI/CD, containerización con Docker, orquestación con Kubernetes.', @CloudServe, GETUTCDATE(), 'System');

SET @Service9 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service9, 'Administración de Bases de Datos', 60.00, 'Diseño, optimización y administración de bases de datos SQL Server, PostgreSQL y MongoDB.', @CloudServe, GETUTCDATE(), 'System');

-- Servicios de SecureIT Networks
SET @Service10 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service10, 'Auditoría de Seguridad', 80.00, 'Análisis de vulnerabilidades, pentesting, implementación de políticas de seguridad y cumplimiento normativo.', @SecureIT, GETUTCDATE(), 'System');

SET @Service11 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service11, 'Monitoreo y Seguridad 24/7', 85.00, 'Monitoreo continuo de infraestructura, detección de amenazas, respuesta a incidentes de seguridad.', @SecureIT, GETUTCDATE(), 'System');

-- Servicios de Consultoría Empresarial
SET @Service12 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service12, 'Análisis de Datos y Business Intelligence', 55.00, 'Análisis de datos, creación de dashboards con Power BI y Tableau, ETL y data warehousing.', @Consultoria, GETUTCDATE(), 'System');

SET @Service13 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service13, 'Arquitectura de Software Enterprise', 120.00, 'Diseño de arquitecturas empresariales, microservicios, event-driven architecture, soluciones escalables.', @Consultoria, GETUTCDATE(), 'System');

-- Servicios de Academia Tech Training
SET @Service14 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service14, 'Capacitación en Tecnologías Emergentes', 50.00, 'Cursos de formación en cloud computing, contenedores, microservicios y DevOps.', @Academia, GETUTCDATE(), 'System');

-- Servicios de Mantenimiento Integral 360
SET @Service15 = NEWID();
INSERT INTO Services (Id, Name, HourlyRate, Description, ProviderId, CreatedAt, CreatedBy) 
VALUES (@Service15, 'Mantenimiento Preventivo de Infraestructura', 45.00, 'Mantenimiento programado de servidores, redes y equipos de cómputo.', @Mantenimiento, GETUTCDATE(), 'System');

PRINT 'Servicios insertados: 15 registros';
GO

-- =============================================
-- Insertar Relaciones Servicio-País
-- =============================================

-- Obtener los IDs de los servicios
DECLARE @DevFrontend UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Desarrollo Web Frontend');
DECLARE @DevBackend UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Desarrollo Backend .NET');
DECLARE @Testing UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Testing y Quality Assurance');
DECLARE @Mobile UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Desarrollo Mobile Multiplataforma');
DECLARE @UIUX UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Diseño UI/UX');
DECLARE @ML UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Machine Learning e IA');
DECLARE @CloudAWS UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Consultoría Cloud AWS');
DECLARE @DevOps UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'DevOps y CI/CD');
DECLARE @Database UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Administración de Bases de Datos');
DECLARE @Security UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Auditoría de Seguridad');
DECLARE @Monitoring UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Monitoreo y Seguridad 24/7');
DECLARE @BI UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Análisis de Datos y Business Intelligence');
DECLARE @Architecture UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Arquitectura de Software Enterprise');
DECLARE @Training UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Capacitación en Tecnologías Emergentes');
DECLARE @Maintenance UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Services WHERE Name = 'Mantenimiento Preventivo de Infraestructura');

-- Desarrollo Web Frontend - Disponible en Colombia, México, Perú
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@DevFrontend, 'CO', GETUTCDATE(), 'System'),
(@DevFrontend, 'MX', GETUTCDATE(), 'System'),
(@DevFrontend, 'PE', GETUTCDATE(), 'System');

-- Desarrollo Backend .NET - Disponible en Colombia, USA, Chile
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@DevBackend, 'CO', GETUTCDATE(), 'System'),
(@DevBackend, 'US', GETUTCDATE(), 'System'),
(@DevBackend, 'CL', GETUTCDATE(), 'System');

-- Testing y QA - Disponible en Colombia, México
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Testing, 'CO', GETUTCDATE(), 'System'),
(@Testing, 'MX', GETUTCDATE(), 'System');

-- Desarrollo Mobile - Disponible en Colombia, USA, Argentina, España
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Mobile, 'CO', GETUTCDATE(), 'System'),
(@Mobile, 'US', GETUTCDATE(), 'System'),
(@Mobile, 'AR', GETUTCDATE(), 'System'),
(@Mobile, 'ES', GETUTCDATE(), 'System');

-- Diseño UI/UX - Disponible en Colombia, México, Chile
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@UIUX, 'CO', GETUTCDATE(), 'System'),
(@UIUX, 'MX', GETUTCDATE(), 'System'),
(@UIUX, 'CL', GETUTCDATE(), 'System');

-- Machine Learning - Disponible en USA, Colombia, Brasil
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@ML, 'US', GETUTCDATE(), 'System'),
(@ML, 'CO', GETUTCDATE(), 'System'),
(@ML, 'BR', GETUTCDATE(), 'System');

-- Consultoría Cloud AWS - Disponible en todos los países
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@CloudAWS, 'CO', GETUTCDATE(), 'System'),
(@CloudAWS, 'US', GETUTCDATE(), 'System'),
(@CloudAWS, 'MX', GETUTCDATE(), 'System'),
(@CloudAWS, 'PE', GETUTCDATE(), 'System'),
(@CloudAWS, 'CL', GETUTCDATE(), 'System'),
(@CloudAWS, 'AR', GETUTCDATE(), 'System'),
(@CloudAWS, 'BR', GETUTCDATE(), 'System'),
(@CloudAWS, 'ES', GETUTCDATE(), 'System');

-- DevOps y CI/CD - Disponible en Colombia, USA, México, España
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@DevOps, 'CO', GETUTCDATE(), 'System'),
(@DevOps, 'US', GETUTCDATE(), 'System'),
(@DevOps, 'MX', GETUTCDATE(), 'System'),
(@DevOps, 'ES', GETUTCDATE(), 'System');

-- Administración de Bases de Datos - Disponible en Colombia, USA, Chile, Argentina
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Database, 'CO', GETUTCDATE(), 'System'),
(@Database, 'US', GETUTCDATE(), 'System'),
(@Database, 'CL', GETUTCDATE(), 'System'),
(@Database, 'AR', GETUTCDATE(), 'System');

-- Auditoría de Seguridad - Disponible en Colombia, USA, México, Brasil, España
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Security, 'CO', GETUTCDATE(), 'System'),
(@Security, 'US', GETUTCDATE(), 'System'),
(@Security, 'MX', GETUTCDATE(), 'System'),
(@Security, 'BR', GETUTCDATE(), 'System'),
(@Security, 'ES', GETUTCDATE(), 'System');

-- Monitoreo y Seguridad 24/7 - Disponible en Colombia, USA, España
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Monitoring, 'CO', GETUTCDATE(), 'System'),
(@Monitoring, 'US', GETUTCDATE(), 'System'),
(@Monitoring, 'ES', GETUTCDATE(), 'System');

-- Análisis de Datos y BI - Disponible en Colombia, México, Chile
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@BI, 'CO', GETUTCDATE(), 'System'),
(@BI, 'MX', GETUTCDATE(), 'System'),
(@BI, 'CL', GETUTCDATE(), 'System');

-- Arquitectura de Software Enterprise - Disponible en USA, Colombia, España, Brasil
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Architecture, 'US', GETUTCDATE(), 'System'),
(@Architecture, 'CO', GETUTCDATE(), 'System'),
(@Architecture, 'ES', GETUTCDATE(), 'System'),
(@Architecture, 'BR', GETUTCDATE(), 'System');

-- Capacitación en Tecnologías - Disponible en Colombia, México, Perú
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Training, 'CO', GETUTCDATE(), 'System'),
(@Training, 'MX', GETUTCDATE(), 'System'),
(@Training, 'PE', GETUTCDATE(), 'System');

-- Mantenimiento Preventivo - Disponible en Colombia
INSERT INTO ServiceCountries (ServiceId, CountryCode, CreatedAt, CreatedBy) VALUES
(@Maintenance, 'CO', GETUTCDATE(), 'System');

PRINT 'Relaciones Servicio-País insertadas: 52 registros';
GO

PRINT '';
PRINT '=============================================';
PRINT 'Resumen de datos iniciales:';
PRINT '- Países: 10';
PRINT '- Proveedores: 8';
PRINT '- Campos personalizados: 30';
PRINT '- Servicios: 15';
PRINT '- Relaciones Servicio-País: 52';
PRINT '=============================================';
PRINT 'Datos iniciales insertados exitosamente';
GO

