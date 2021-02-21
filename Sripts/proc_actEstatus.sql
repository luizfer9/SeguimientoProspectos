CREATE PROCEDURE [dbo].[proc_actEstatus] (@opc int,@id_pros int, @id_estatus int, @observ varchar(200))
AS 
BEGIN
	IF(@opc = 1)
	BEGIN
		BEGIN TRY
			UPDATE Prospectos
				SET desc_observ = LTRIM(RTRIM(@observ))
			WHERE id_prospecto = @id_pros
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
	IF(@opc = 2)
	BEGIN
		BEGIN TRY
			UPDATE Prospectos
				SET num_estatus = @id_estatus
			WHERE id_prospecto = @id_pros
			SELECT 1		END TRY
		BEGIN CATCH
			INSERT INTO dataErrors
			SELECT 
				ERROR_NUMBER() as numberError,
				ERROR_PROCEDURE() as errorProcedure,
				ERROR_MESSAGE() as errorMessage
			SELECT 0
		END CATCH
	END
	IF(@opc = 3)
	BEGIN
		BEGIN TRY
			SELECT id_estatus,desc_estatus FROM CatalogoEstatus
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
END