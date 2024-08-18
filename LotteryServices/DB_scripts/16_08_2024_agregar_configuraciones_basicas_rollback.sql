-- Eliminar datos de la tabla 'UsuarioRol'
-- Nota: Eliminar primero las relaciones de los usuarios para evitar conflictos con las claves foráneas
DELETE FROM UsuarioRol
WHERE UsuarioId IN (1, 2) AND RolId IN (1, 2);

-- Eliminar datos de la tabla 'RolPermiso'
DELETE FROM RolPermiso
WHERE RolId IN (1, 2) AND PermisoId IN (1, 2, 3, 4, 5);

-- Eliminar datos de la tabla 'Usuarios'
DELETE FROM Usuarios
WHERE UsuarioID IN (1, 2);

-- Eliminar datos de la tabla 'Rol'
DELETE FROM Rol
WHERE RolId IN (1, 2);

-- Eliminar datos de la tabla 'Permiso'
DELETE FROM Permiso
WHERE PermisoId IN (1, 2, 3, 4, 5);

-- Eliminar datos de la tabla 'Modules'
DELETE FROM Modules
WHERE IdModule IN (1, 2);
