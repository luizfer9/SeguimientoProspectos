CREATE PROCEDURE proc_obtenDataDocs(@opc int)
AS
BEGIN
	if(@opc = 1)
	BEGIN
		--Regresa valor para insertar nueva carga de Documentos
		SELECT ISNULL(MAX(id_docpros),0) FROM Documentos
	END
	IF(@opc = 2)
	BEGIN
		--Regresa esquema de tabla documentos
		SELECT id_documento,id_docpros, desc_nombreDoc, desc_rutaDoc, bin_documento
		FROM Documentos
	END
END