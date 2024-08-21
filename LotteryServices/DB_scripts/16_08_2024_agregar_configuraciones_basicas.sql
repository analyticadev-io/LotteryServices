-- Insertar datos en la tabla 'Modules'
INSERT INTO Modules (Name, module_name, visibilityStatus, icon)
VALUES
    ('Roles', 'roles', 'false', 'lock'),
    ('Gestor de menus', 'modules', 'false', 'unlock');

-- Insertar datos en la tabla 'Permiso'
-- Habilitar IDENTITY_INSERT para la tabla Permiso si PermisoId es una columna de identidad
SET IDENTITY_INSERT Permiso ON;

INSERT INTO Permiso (PermisoId, Descripcion)
VALUES
    (1, 'browse_roles'),
    (2, 'read_roles'),
    (3, 'edit_roles'),
    (4, 'add_roles'),
    (5, 'delete_roles'),
    (6, 'browse_modules'),
    (7, 'read_modules'),
    (8, 'edit_modules'),
    (9, 'add_modules'),
    (10, 'delete_modules');

-- Desactivar IDENTITY_INSERT para la tabla Permiso
SET IDENTITY_INSERT Permiso OFF;

-- Insertar datos en la tabla 'Rol'
-- Habilitar IDENTITY_INSERT para la tabla Rol si RolId es una columna de identidad
SET IDENTITY_INSERT Rol ON;

INSERT INTO Rol (RolId, Nombre)
VALUES
    (1, 'Administrador'),
    (2, 'Usuario');

-- Desactivar IDENTITY_INSERT para la tabla Rol
SET IDENTITY_INSERT Rol OFF;

-- Insertar datos en la tabla 'Usuarios'
-- Habilitar IDENTITY_INSERT para la tabla Usuarios si UsuarioID es una columna de identidad
SET IDENTITY_INSERT Usuarios ON;

INSERT INTO Usuarios (UsuarioID, Nombre, Email, Contrasena, NombreUsuario)
VALUES
    (1, 'Administrador', 'admin@admin.com', '697e5535dcf7950a01b234207409cdb296684d3d8092ebddd7a7f699f6c21212', '4dministrador'),
    (2, 'Usuario', 'usuario@usuario.com', '697e5535dcf7950a01b234207409cdb296684d3d8092ebddd7a7f699f6c21212', 'usuario1');

-- Desactivar IDENTITY_INSERT para la tabla Usuarios
SET IDENTITY_INSERT Usuarios OFF;

-- Insertar datos en la tabla 'RolPermiso'
-- Asegúrate de que los roles y permisos ya se han insertado antes de este paso
INSERT INTO RolPermiso (RolId, PermisoId)
VALUES
    (1, 1),
    (1, 2),
    (1, 3),
    (1, 4),
    (1, 5),
    (1, 6),
    (1, 7),
    (1, 8),
    (1, 9),
    (1, 10),
    (2, 1),
    (2, 2);

-- Insertar datos en la tabla 'UsuarioRol'
-- Asegúrate de que los usuarios y roles ya se han insertado antes de este paso
INSERT INTO UsuarioRol (UsuarioId, RolId)
VALUES
    (1, 1),
    (2, 2);
