#!/bin/sh

# Ejecutar las migraciones
echo "Applying database migrations..."
dotnet ef database update --no-build -s LotteryServices.dll

# Iniciar la aplicación
echo "Starting application..."
exec dotnet LotteryServices.dll
