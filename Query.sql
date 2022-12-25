
CREATE DATABASE CRUD_Imagen

USE CRUD_Imagen

CREATE TABLE Imagenes
(
    Id_Imagen INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50),
    Imagen VARCHAR(MAX)
)

CREATE PROCEDURE sp_listar_imagenes
AS
BEGIN
    SELECT *
    FROM Imagenes
END

CREATE PROCEDURE sp_buscar_imagen
    @Id INT
AS
BEGIN
    SELECT *
    FROM Imagenes
    WHERE Id_Imagen=@Id
END

CREATE PROCEDURE sp_insertar_imagen
    @Nombre VARCHAR(50),
    @Imagen VARCHAR(MAX)
AS
BEGIN
    INSERT INTO Imagenes
    VALUES(@Nombre, @Imagen)
END

CREATE PROCEDURE sp_eliminar_imagen
    @Id INT
AS
BEGIN
    DELETE Imagenes WHERE Id_Imagen=@Id
END

CREATE PROCEDURE sp_actualizar_imagen
    @Id INT,
    @Nombre VARCHAR(50),
    @Imagen VARCHAR(MAX)
AS
BEGIN

    IF @Imagen ='null'
UPDATE Imagenes SET Nombre=@Nombre WHERE Id_Imagen=@Id
ELSE
UPDATE Imagenes SET Nombre=@Nombre, Imagen=@Imagen WHERE Id_Imagen=@Id
END