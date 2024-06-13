USE ProyectoBD_Molino

/*ZONA PARA LOS PROCEDIMIENTOS ALMACENADOS CORRESPONDIENTES A LAS HISTORIAS: CTE001.*/
GO

/*
Este procedimiento almacenado sirve como conjunto para la actualización de datos del listado de los empleados (valgame la reduncia).

Para ser más precisos es parte del desarrollo de la historia: CTE001, el cual indica:
	"Como usuario administrador, quiero gestionar la actualización de la información de los empleados que están laborando, -
	  esto para mantener un control adecuado y facilitar la administración de recursos humanos."

Esto se debe a que en una actualización, se requiere primero, consultar antes de poder hacer el proceso de actualización, por eso se hace dicha consulta, -
para ahora asi poder hacer la actualización sin ningún tipo de problema.
*/
CREATE PROCEDURE VistaPreliminarListadoEmpleados
/*Busca el rol que esta asociado al empleado que se -
quiere hacer, o una actualización o eliminación.*/
@IdRol INT
AS
BEGIN
	/* Busca la cédula, nombre, los apellidos, el puesto, la fecha, el salario, el correo, su teléfono -
		y por ultimo el rol asociado a dicho empleado, eso si, siempre y cuando el IdRol sea el que se -
		esta pasando por parámetro, ya que se simula que esta entrando a un empleado para editarlo o -
		eliminarlo.
	
		Esto por medio de una unión interna de tablas, precisamente 2 tablas:
			> Empleado.
			>Usuario.
		Ya que la tabla: "Empleado" tiene los campos: Puesto, FechaContratacion y Salario, y la tabla: "Usuario" tiene -
		los campos: Cedula, Nombre, PrimerApellido, SegundoApellido, Correo, Telefono y IdRol, por eso se hace la unión -
		interna de las tablas.*/
	SELECT 
		E.Id, Cedula, Nombre,
		PrimerApellido, SegundoApellido,
		Puesto, FechaContratacion,
		Salario, Correo, Telefono, IdRol
	From 
		Empleado E 
		INNER JOIN
		Usuario U 
		On E.IdUsuario = U.Id

	Where
			@IdRol = IdRol
END
GO


/*
Este procedimiento también forma parte de la actualización de datos, para ser más precisos es parte del desarrollo de la historia: CTE001, el cual indica:
	"Como usuario administrador, quiero gestionar la actualización de la información de los empleados que están laborando, -
	  esto para mantener un control adecuado y facilitar la administración de recursos humanos."

Esto se debe a que en una actualización, se requiere primero, consultar antes de poder hacer el proceso de actualización, por eso se hace dicha consulta, -
para ahora asi poder hacer la actualización sin ningún tipo de problema.
*/
CREATE PROCEDURE ActualizarListadoEmpleados
/* Aqui pasamos por parametro estos datos ya que como el usuario es un administrador -
	entonces puede editar cualquier tipo de dato, excepto el IdUsuario, ya que ese se usara -
	para poder identificar cual dato es el que se tiene que actualizar.
	
	Para que se entienda, el IdUsuario evitara que un administrador edite una información -
	de un empleado equivocado, entonces si el quiere editar al empleado: Jorge, entonces lo -
	podrá hacer con seguridad.
	*/
	@IdUsuario INT,
	@Cedula NVARCHAR(20),
	@Nombre NVARCHAR(50),
	@PrimerApellido NVARCHAR(50),
	@SegundoApellido NVARCHAR(50),
	@Puesto NVARCHAR(100),
	@FechaContratacion DATE,
	@Salario DECIMAL(10, 2),
	@Correo NVARCHAR(100),
	@Telefono NVARCHAR(20),
	@IdRol INT
AS
BEGIN
/*
Aqui tenemos dos sentencias de tipo: Update, esto porque en SQL Server no permite editar 2 tablas en una misma sentencia.

Además, dichas sentencias actualizaran los campos que habiamos puesto por parámetro, eso si, ambas conservan tanto el INNER -
JOIN de Empleado y Usuario, como la condición del where, esto para evitar que los nuevos datos ingresados se pierdan o se mezclen -
de manera errónea. 
*/
UPDATE Emp
	SET 
	Puesto = @Puesto,
	FechaContratacion = @FechaContratacion,
	Salario = @Salario
	FROM
		Empleado Emp INNER JOIN Usuario Us
		On Emp.Id = Us.Id
	WHERE
			@IdUsuario = Emp.IdUsuario

UPDATE Us
	SET
	Cedula = @Cedula,
	Nombre = @Nombre,
	PrimerApellido = @PrimerApellido,
	SegundoApellido = @SegundoApellido,
	Correo = @Correo,
	Telefono = @Telefono,
	IdRol  = @IdRol
	FROM
		Empleado Emp INNER JOIN Usuario Us
		On Emp.Id = Us.Id
	WHERE
			@IdUsuario = Emp.IdUsuario
END
GO


/*ZONA DE PRUEBAS*/
Exec VistaPreliminarListadoEmpleados 'Aqui se tiene que poner el id del rol al cual quieres consultar.'

Exec ActualizarListadoEmpleados 'Id del usuario a actualizar o mantener', 'Cedula a actualizar o mantener',
'Nombre a actualizar o mantener','Primer apellido a actualizar o mantener', 'Segundo apellido a actualizar o mantener',
'Puesto a actualizar o mantener', 'Fecha de contratación', 'Salario a actualizar o mantener',
'Correo a actualizar o mantener', 'Teléfono a actualizar o mantener',
'Rol  a actualizar o mantener'
