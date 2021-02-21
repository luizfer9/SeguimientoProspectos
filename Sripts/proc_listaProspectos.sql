CREATE PROCEDURE [dbo].[proc_listaProspectos] (@opc int,@id_pros int)
AS 
BEGIN
	if(@opc = 1)
	BEGIN
		BEGIN TRY
			SELECT id_prospecto,desc_nombre,desc_apellido1,desc_apellido2,b.desc_estatus
			FROM Prospectos a
			LEFT JOIN CatalogoEstatus b ON a.num_estatus = b.id_estatus
		END TRY
		BEGIN CATCH
			INSERT INTO dataErrors
			SELECT 
				ERROR_NUMBER() as numberError,
				ERROR_PROCEDURE() as errorProcedure,
				ERROR_MESSAGE() as errorMessage
			SELECT numberError,errorProcedure,errorMessage FROM dataErrors
		END CATCH
	END
	if(@opc = 2)
	BEGIN
		BEGIN TRY
			SELECT desc_nombre, desc_apellido1, desc_apellido2, desc_calle, desc_numero,
				 desc_colonia, num_codPost, num_telefono, desc_rfc, desc_observ, num_documentoProc, num_estatus
			FROM Prospectos
			WHERE id_prospecto = @id_pros
		END TRY
		BEGIN CATCH
			INSERT INTO dataErrors
			SELECT 
				ERROR_NUMBER() as numberError,
				ERROR_PROCEDURE() as errorProcedure,
				ERROR_MESSAGE() as errorMessage
			SELECT numberError,errorProcedure,errorMessage FROM dataErrors
		END CATCH
	END
	if(@opc = 3)
	BEGIN
		BEGIN TRY
			SELECT desc_nombreDoc,desc_rutaDoc,bin_documento
			FROM Documentos a
			LEFT JOIN Prospectos b on a.id_docpros = b.num_documentoProc
			WHERE b.id_prospecto = @id_pros
		END TRY
		BEGIN CATCH
			INSERT INTO dataErrors
			SELECT 
				ERROR_NUMBER() as numberError,
				ERROR_PROCEDURE() as errorProcedure,
				ERROR_MESSAGE() as errorMessage
			SELECT numberError,errorProcedure,errorMessage FROM dataErrors
		END CATCH
	END
END