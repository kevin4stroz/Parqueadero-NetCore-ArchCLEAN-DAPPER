USE master
GO

-- BORRAR BASE DE DATOS SI EXISTE
IF EXISTS(SELECT * FROM sys.databases WHERE name = 'PruebaDB')
BEGIN 
    DROP DATABASE [PruebaDB]
END 
GO

-- CREACION DE BASE DE DATOS
CREATE DATABASE [PruebaDB]
GO 
USE [PruebaDB]
GO

-- CREACION DE ESQUEMA
CREATE SCHEMA Parqueadero;
GO

-- CREACION DE TABLA TIPO VEHICULO
CREATE TABLE Parqueadero.TipoVehiculo(
    IdTipoVehiculo int not null identity(1,1),
    Descripcion varchar(max) not null,
    Tarifa money not null,
    PRIMARY KEY (IdTipoVehiculo),
)
GO

-- CREACION DE TABLA VEHICULO
CREATE TABLE Parqueadero.Vehiculo(
    IdVehiculo int not null identity(1,1),
    IdentificacionVehiculo varchar(max) not null,
    Marca varchar(max) not null,
    Color varchar(max) not null,
    IdTipoVehiculo int not null,
    PRIMARY KEY (IdVehiculo),
    FOREIGN KEY (IdTipoVehiculo) REFERENCES Parqueadero.TipoVehiculo(IdTipoVehiculo)
)
GO

-- CREACION DE TABLA APARCAMIENTO
CREATE TABLE Parqueadero.Aparcamiento(
    IdAparcamiento int not null identity(1,1),
    Nomeclatura varchar(max) not null,
    Estado bit not null,
    PRIMARY KEY (IdAparcamiento)
)
GO

-- CREACION DE TABLA ESTABLECIMIENTO
CREATE TABLE Parqueadero.Establecimiento(
    IdEstablecimiento int not null identity(1,1),
    NitCedula varchar(max) not null,
    NombreEstablecimiento varchar(max) not null,
    TarifaDescuento real not null,
    PRIMARY KEY (IdEstablecimiento)
)
GO

-- CREACION DE TABLA VEHICULOAPARCAMIENTO
CREATE TABLE Parqueadero.VehiculoAparcamiento(
    IdVehiculoAparcamiento int not null identity(1,1),
    IdVehiculo int not null,
    IdAparcamiento int noT null,
    HoraLLegada datetime not null,
    HoraSalida datetime,
    TiempoParqueo float,
    ValorPagar money,
    IdEstablecimiento int,
    NroFactura varchar(max),
    PRIMARY KEY (IdVehiculoAparcamiento),
    FOREIGN KEY (IdVehiculo) REFERENCES Parqueadero.Vehiculo(IdVehiculo),
    FOREIGN KEY (IdAparcamiento) REFERENCES Parqueadero.Aparcamiento(IdAparcamiento),
    FOREIGN KEY (IdEstablecimiento) REFERENCES Parqueadero.Establecimiento(IdEstablecimiento)

)
GO

-- INSERTAR DATOS DE PRUEBA
INSERT INTO Parqueadero.TipoVehiculo VALUES 
('automovil', 110), -- 1
('motocicleta', 50), -- 2
('bicicleta', 10) -- 3
GO

-- INSERTAR VEHICULOS
INSERT INTO Parqueadero.Vehiculo VALUES
('aaa-111','twingo','negro',1), -- 1
('bbb-222','yamaha','blanco',2), -- 2
('ccc-222','mrx victory','blanco',2), -- 2
('FAG2304K20089304','gw','rojo',3) -- 3
GO

-- INSERTAR APARCAMIENTO 1 = OCUPADO 0 = LIBRE
INSERT INTO Parqueadero.Aparcamiento VALUES 
('A1',1), -- 1
('A2',1), -- 2
('A3',1), -- 3
('B1',1), -- 4
('B2',0), -- 5
('B3',0)  -- 6

-- INSERTAR ESTABLECIMIENTO
INSERT INTO Parqueadero.Establecimiento VALUES
('66666666','Chorizos vecinos',30.0),
('77777777','Tamales vecinos',30.0),
('88888888','El estanco vecinos',30.0)

-- INSERTAR RECIEN LLEGADOS SIN SALIDA
INSERT INTO Parqueadero.VehiculoAparcamiento VALUES
(1, 1, '2022-02-10 07:41:36.577', NULL, NULL, NULL, NULL, NULL),
(2, 2, '2022-02-11 07:41:36.577', NULL, NULL, NULL, NULL, NULL),
(3, 3, '2022-02-12 07:41:36.577', NULL, NULL, NULL, NULL, NULL),
(4, 4, '2022-02-13 07:41:36.577', NULL, NULL, NULL, NULL, NULL)

