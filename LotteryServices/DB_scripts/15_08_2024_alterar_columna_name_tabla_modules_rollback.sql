-- Revertir el tipo de datos de VARCHAR a CHAR
ALTER TABLE Modules
ALTER COLUMN name CHAR(50); -- Ajusta el tama�o seg�n sea necesario
