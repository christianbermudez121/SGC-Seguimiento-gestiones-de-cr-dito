#!/bin/bash

# Script para verificar datos en la base de datos SQLite

DB_PATH="/Users/hanlei/SGC-Seguimiento-gestiones-de-cr-dito/SGC Seguimiento gestiones de crédito/sgc.db"

echo "=== Verificando datos en la base de datos ==="
echo ""

# Verificar si existe la base de datos
if [ ! -f "$DB_PATH" ]; then
    echo "⚠️  La base de datos no existe en: $DB_PATH"
    exit 1
fi

echo "✅ Base de datos encontrada"
echo ""

# Contar registros en HistorialGestiones
echo "--- Registros en HistorialGestiones ---"
sqlite3 "$DB_PATH" "SELECT COUNT(*) as Total FROM HistorialGestiones;"
echo ""

# Mostrar los últimos registros
echo "--- Últimos 10 registros de HistorialGestiones ---"
sqlite3 "$DB_PATH" -header -column "SELECT Id, IdSolicitud, EstadoAnterior, EstadoNuevo, datetime(Fecha) as Fecha FROM HistorialGestiones ORDER BY Fecha DESC LIMIT 10;"
echo ""

# Contar solicitudes
echo "--- Registros en SolicitudesCredito ---"
sqlite3 "$DB_PATH" "SELECT COUNT(*) as Total FROM SolicitudesCredito;"
echo ""

# Mostrar algunas solicitudes
echo "--- Algunas SolicitudesCredito ---"
sqlite3 "$DB_PATH" -header -column "SELECT IdSolicitud, identificacion, monto, Estado FROM SolicitudesCredito LIMIT 10;"
echo ""

# Verificar si hay solicitudes pero no historial
SOLICITUDES=$(sqlite3 "$DB_PATH" "SELECT COUNT(*) FROM SolicitudesCredito;")
HISTORIAL=$(sqlite3 "$DB_PATH" "SELECT COUNT(*) FROM HistorialGestiones;")

if [ "$SOLICITUDES" -gt 0 ] && [ "$HISTORIAL" -eq 0 ]; then
    echo "⚠️  Hay $SOLICITUDES solicitudes pero no hay historial de gestiones"
    echo ""
    echo "💡 Sugerencia: El historial se crea cuando cambias el estado de una solicitud"
    echo "   o puedes crear registros manualmente en el controlador de Seguimiento"
fi

if [ "$HISTORIAL" -gt 0 ]; then
    echo "✅ Hay $HISTORIAL registros de historial en la base de datos"
fi
