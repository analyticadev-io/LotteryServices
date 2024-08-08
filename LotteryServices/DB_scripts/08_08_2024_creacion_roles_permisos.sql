---- Crear la tabla Rol
--CREATE TABLE Rol (
--    RolId integer PRIMARY KEY IDENTITY(1,1),
--    Nombre varchar(100) NOT NULL
--);

---- Crear la tabla Permiso
--CREATE TABLE Permiso (
--    PermisoId integer PRIMARY KEY IDENTITY(1,1),
--    Descripcion varchar(255) NOT NULL
--);

---- Crear la tabla RolPermiso
--CREATE TABLE RolPermiso (
--    RolId integer NOT NULL,
--    PermisoId integer NOT NULL,
--    PRIMARY KEY (RolId, PermisoId),
--    FOREIGN KEY (RolId) REFERENCES Rol(RolId),
--    FOREIGN KEY (PermisoId) REFERENCES Permiso(PermisoId)
--);

---- Crear la tabla UsuarioRol
--CREATE TABLE UsuarioRol (
--    UsuarioId integer NOT NULL,
--    RolId integer NOT NULL,
--    PRIMARY KEY (UsuarioId, RolId),
--    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId),
--    FOREIGN KEY (RolId) REFERENCES Rol(RolId)
--);
