CREATE PROCEDURE [dbo].[proc_capturaProspecto] 
(
	@nombre		VARCHAR(20),
	@apellido1	VARCHAR(20),
	@apellido2	VARCHAR(20),
	@calle		VARCHAR(30),
	@numero		VARCHAR(8),
	@colonia	VARCHAR(30),
	@codPost	int,
	@tel		VARCHAR(10),
	@rfc		VARCHAR(14),
	@idDoc		int,
	@idestatus	int
)
AS 
BEGIN
	BEGIN TRY
		INSERT INTO Prospectos
		(desc_nombre,desc_apellido1,desc_apellido2,desc_calle,desc_numero,
			desc_colonia,num_codPost,num_telefono,desc_rfc,num_documentoProc,num_estatus)
		SELECT @nombre,@apellido1,@apellido2,@calle,@numero,@colonia,@codPost,@tel,
			@rfc,@idDoc,@idestatus
		SELECT 1
	END TRY
	BEGIN CATCH
		INSERT INTO dataErrors
		SELECT 
			ERROR_NUMBER() as numberError,
			ERROR_PROCEDURE() as errorProcedure,
			ERROR_MESSAGE() as errorMessage
		SELECT 0
	END CATCH
END