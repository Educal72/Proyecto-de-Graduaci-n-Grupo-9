USE ProyectoBD_Molino

/*ZONA PARA LOS PROCEDIMIENTOS ALMACENADOS CORRESPONDIENTES A LAS HISTORIAS: CTE002.*/
GO

/*
Este procedimiento es para la eliminación de datos, para ser más precisos es parte del desarrollo de la historia: CTE002, el cual indica:
	"Como usuario administrador, quiero gestionar la eliminación de empleados en el sistema, -
	  esto para mantener un control adecuado y facilitar la administración de recursos humanos."
*/
CREATE PROCEDURE EliminarListadoEmpleados
	/*Aqui se pide el id del rol y usuario porque es más sencillo eliminar un empleado -
	   por el rol que antes desempeñaba.

	   Además de que puede suceder que varios empleados puede tener el mismo rol, -
	   entonces por eso se pone estos 2 parámetros.*/
	@IdRol INT,
	@IdUsuario INT
AS
BEGIN
	/*Aqui borra el empleado siempre y cuando sea el que se le indique por parámetro, -
	  además de que también se hace un INNER JOIN por lo mencionado anteriormente en los -
	  comentarios de los procedimientos anteriores.
	  
	  Nota: Se hace 2 deletes porque si bien, borra perfectamente el dato en la tabla de empleado, dicho -
	  dato queda estando en la tabla de usuario, como un residuo de su existencia, cosa que por buenas prácticas -
	  no es algo recomendable, ya que afecta la disponibilidad y integridad de los datos del restaurante, por eso se -
	  hace otro delete pero que borre ese dato a la tabla: Usuario, para así mantener la integridad y disponibilidad.*/
	DELETE 
		Emp
	FROM
		Empleado Emp INNER JOIN Usuario Us
		On Emp.IdUsuario = Us.Id
	WHERE 
		@IdRol = IdRol AND @IdUsuario = Emp.IdUsuario


	DELETE 
		Usuario
	FROM
		Usuario
	WHERE 
		@IdRol = IdRol AND @IdUsuario = Id
END
GO


/*ZONA DE PRUEBAS*/
Exec EliminarListadoEmpleados 'Id rol que quiere colocar para eliminar el empleado en la tabla de: Empleado', 
'Id usuario que quiere eliminar en la tabla de usuario que estaba en la tabla de empleado.'
	