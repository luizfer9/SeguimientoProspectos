# SeguimientoProspectos
Se realiza un sistema de seguimiento a prospectos usando informacion de contacto y documentos

Se crea un sistema donde dan de alta prospectos con su informacon de contacto y documentos para 
ser evaluados por el usuario posteriormente, se busca dar seguimiento al dar de alta, y clasificarlos 
por Enviados, Autorizados y Rechazados.

Se realiza un sistema para escritorio con C#, con conexion a DB en SQL Server mediante txt,
alojado en el mismo directorio del sistema.

Se realiza la subida de documentos a una base de datos mediante conversion de archivos a matriz de bytes, 
posteriormente se visualizan los n prospectos existentes en un listview con informacion resumida con su respectivo 
estatus(Enviado, Autorizado, Rechazado), con el objetivo de seleccionar un prospecto y desplegar la informacion
de contacto como los documentos pertenecientes al prospecto para ser evaluado y asignarle un estatus.
