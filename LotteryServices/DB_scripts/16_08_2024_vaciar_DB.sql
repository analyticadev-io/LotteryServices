-- Deshabilitar las restricciones de clave foránea para poder eliminar registros sin restricciones
-- Vaciar datos en la tabla 'UsuarioRol'
DELETE FROM UsuarioRol;

-- Vaciar datos en la tabla 'RolPermiso'
DELETE FROM RolPermiso;

-- Vaciar datos en la tabla 'Usuarios'
DELETE FROM Usuarios;

-- Vaciar datos en la tabla 'Rol'
DELETE FROM Rol;

-- Vaciar datos en la tabla 'Permiso'
DELETE FROM Permiso;

-- Vaciar datos en la tabla 'Modules'
DELETE FROM Modules;

EXEC sp_MSforeachtable "ALTER TABLE ? CHECK CONSTRAINT ALL";
