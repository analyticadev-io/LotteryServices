-- Insertar datos en la tabla 'Modules'
INSERT INTO Modules (Name)
VALUES
    ('roles'),
    ('modules');

-- Insertar datos en la tabla 'Permiso'
INSERT INTO Permiso (Descripcion)
VALUES
    ('browse_roles'),
    ('read_roles'),
    ('edit_roles'),
    ('add_roles'),
    ('delete_roles'),
	('browse_modules'),
    ('read_modules'),
    ('edit_modules'),
    ('add_modules'),
    ('delete_modules');

-- Insertar datos en la tabla 'Rol'
INSERT INTO Rol (Nombre)
VALUES
    ('Administrador'),
    ('Usuario');

-- Insertar datos en la tabla 'Usuarios'
-- Nota: Asegúrate de que el valor de la contraseña sea el correcto y se ajuste a las necesidades de tu aplicación
INSERT INTO Usuarios (Nombre, Email, Contrasena, NombreUsuario)
VALUES
    ('Administrador', 'admin@admin.com', 'admin123.', '4dministrador'),
    ('Usuario', 'usuario@usuario.com', 'admin123.', 'usuario1');

-- Insertar datos en la tabla 'RolPermiso'
-- Nota: Asegúrate de que los roles y permisos ya se han insertado antes de este paso
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
-- Nota: Asegúrate de que los usuarios y roles ya se han insertado antes de este paso
INSERT INTO UsuarioRol (UsuarioId, RolId)
VALUES
    (1, 1),
    (2, 2);
