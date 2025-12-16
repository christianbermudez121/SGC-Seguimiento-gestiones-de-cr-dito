-- Script para verificar datos en la base de datos de reportes

-- 1. Verificar si hay datos en HistorialGestiones
SELECT COUNT(*) as TotalHistorialGestiones FROM HistorialGestiones;

-- 2. Ver todos los registros de HistorialGestiones
SELECT * FROM HistorialGestiones ORDER BY Fecha DESC;

-- 3. Verificar solicitudes de crédito
SELECT COUNT(*) as TotalSolicitudes FROM SolicitudesCredito;

-- 4. Ver solicitudes
SELECT IdSolicitud, identificacion, monto, Estado, FechaSolicitud FROM SolicitudesCredito;

-- 5. Verificar clientes
SELECT COUNT(*) as TotalClientes FROM Clientes;

-- 6. Si no hay datos de historial pero sí solicitudes, crear algunos registros de ejemplo
-- Descomenta las siguientes líneas para insertar datos de prueba:

/*
-- Insertar historial de gestión de ejemplo (ajusta los IDs según tus datos)
INSERT INTO HistorialGestiones (IdSolicitud, UsuarioId, EstadoAnterior, EstadoNuevo, Comentarios, Fecha)
SELECT 
    IdSolicitud,
    'sistema',
    'Pendiente',
    'En Revisión',
    'Cambio automático de estado - Datos de prueba',
    GETDATE()
FROM SolicitudesCredito
WHERE Estado = 'Pendiente'
LIMIT 5;
*/
