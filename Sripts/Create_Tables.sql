CREATE TABLE [dbo].[Documentos]
(
	id_documento	INT IDENTITY(1,1),
	id_docpros		INT NOT NULL DEFAULT 0,
	desc_nombreDoc	VARCHAR(20) NOT NULL DEFAULT 'Sin nombre',
	desc_rutaDoc	VARCHAR(MAX) NOT NULL DEFAULT 'Sin ruta',
	bin_documento	VARBINARY(MAX),
	PRIMARY KEY(id_documento)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[CatalogoEstatus]
(
	id_estatus		INT NOT NULL DEFAULT 0,
	desc_estatus	VARCHAR(20) NOT NULL DEFAULT 'Sin estatus',
	PRIMARY KEY(id_estatus)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Prospectos]
(
	id_prospecto		INT IDENTITY(1,1),
	desc_nombre			VARCHAR(20) NOT NULL DEFAULT '',
	desc_apellido1		VARCHAR(20) NOT NULL DEFAULT '',
	desc_apellido2		VARCHAR(20) NOT NULL DEFAULT '',
	desc_calle			VARCHAR(30) NOT NULL DEFAULT '',
	desc_numero			VARCHAR(8) NOT NULL DEFAULT '',
	desc_colonia		VARCHAR(30) NOT NULL DEFAULT '',
	num_codPost			INT NOT NULL DEFAULT 0,
	num_telefono		VARCHAR(10) NOT NULL DEFAULT 0,
	desc_rfc			VARCHAR(14) NOT NULL DEFAULT '',
	desc_observ			VARCHAR(200) NOT NULL DEFAULT '',
	num_documentoProc	INT NOT NULL DEFAULT 0,
	num_estatus			INT FOREIGN KEY REFERENCES dbo.CatalogoEstatus(id_estatus),
	PRIMARY KEY(id_prospecto)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[dataErrors]
(
	numberError int,
	errorProcedure NVARCHAR(MAX),
	errorMessage NVARCHAR(MAX),
) ON [PRIMARY]




