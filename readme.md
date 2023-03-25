# Sistema de gestión de recursos humanos
Este proyecto tiene como objetivo crear un sistema de gestión de recursos humanos utilizando tecnologías de C# y SQL . La aplicación permite el cálculo de salarios y mantiene un registro de los recursos humanos para una empresa , añadiendo un incremento del salario dependiendo de periodos de trabajo de 3 meses cada uno.

## Funcionalidades
Mantenimiento de registros de empleados, incluyendo información como nombre, apellido, correo electrónico, dirección personal, teléfono, fecha de inicio de trabajo, imagen, rol y salario.
Funcionalidad CRUD de usuarios para la gestión de registros de empleados.
Cálculo automático de salarios basado en la fecha de inicio de trabajo y el tipo de rol de los empleados.
Generación de informes en formato CSV, incluyendo una lista de todos los empleados de la empresa y una lista de los salarios históricos de cada usuario.

# Descripción del proyecto
El objetivo del proyecto es mantener un registro de los recursos humanos de una empresa X y proporcionar una herramienta sencilla para el cálculo de salarios.

El sistema debe mantener un registro de todos los trabajadores relacionados con la entidad. La información relevante es la siguiente:

1. Name : String - Requerido SI
2. LastNAme: String - Requerido SI
3. Email: Strings - Requerido SI
4. Personal Address: String - Requerido SI
5. Phone: String - Requerido NO
6. Working Start Date : DateTime - Requerido SI
7. Picture : PNG, JPG - Requerido SI
8. Rol: String - Requerido SI
9. Salary : Float - Requerido SI
Para cada usuario, el sistema debe tener un registro de sus roles dentro de la empresa. Cada usuario puede tener uno o muchos roles. La lista de roles es la siguiente:

- . Trabajador
- . Especialista
- . Gerente

El sistema necesita 10 usuarios, 2 gerentes, 4 especialistas, 4 trabajadores. Además, el sistema debe tener una funcionalidad de administrador de CRUD de usuarios. Crear, leer, actualizar y eliminar los usuarios.Es necesario calcular los salarios para cada persona en función del tiempo que hayan estado trabajando en la empresa. Para lograr esto, para cada usuario, se ingresa un salario inicial en el sistema, y periódicamente (a pedido) el sistema debe volver a calcular el salario en función de las siguientes reglas:

- La revisión salarial debe realizarse cada 3 meses solamente. Si se ejecuta el proceso para un usuario antes de los 3 meses posteriores a la última revisión, no se debe tomar ninguna acción. Si se ejecuta el proceso para un usuario después de 3 meses desde la última revisión, se debe aumentar el salario para todos los períodos de 3 meses que faltan.
Si un usuario es un trabajador, el salario debe aumentar un 5% cada 3 meses. Para los especialistas, el aumento será del 8%, para los gerentes, será del 12%.

- El sistema debe proporcionar la siguiente información en un archivo CSV:

Lista de recursos humanos: una lista de todos los empleados de la empresa, incluyendo toda la información. (Nombre, Apellido, Dirección personal, Teléfono, Fecha de inicio de trabajo, rol, salario)
Salarios históricos por usuario: para un usuario dado, la lista de los aument

## Instalación
Para instalar la aplicación, siga los siguientes pasos:

1. Clone el repositorio en su máquina local:

2. git clone https://github.com/Carlosabogal/TestBackend_HRManagement

3. Configure la base de datos MySQL en MYSQL WORKBENCH: Aquí está la sintaxis Markdown que utilicé para el código SQL:

```sql
CREATE DATABASE CONEXION;
USE CONEXION;
CREATE TABLE users (
id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(255) NOT NULL,
last_name VARCHAR(255) NOT NULL,
email VARCHAR(255) NOT NULL,
personal_address VARCHAR(255) NOT NULL,
phone INT,
working_start_date datetime NOT NULL,
picture LONGBLOB,
rol VARCHAR(255) NOT NULL,
initial_salary FLOAT NOT NULL,
salary FLOAT NOT NULL
);
```
4. Configure las variables de entorno y abralo con el IDE

# ENDPOINTS: api/User
Endpoint 1: Get all employees

## Description
Este endpoint recupera una lista de todos los empleados de la empresa, incluyendo su nombre, apellido, dirección personal, número de teléfono, fecha de inicio de trabajo, rol y salario.
Request
```
GET :api/User
Endpoint 1: Get all employees
```
Response
```
200 OK
[    {        "name": "CARLOS",        "last_name": "SABOGAL",        "personal_address": "CALLE FALSA 123",        "phone": 3312341232,        "working_start_date": "2020-01-01",        "role": "Worker",        "salary": 50000.00    },    {        "name": "Jane",        "last_name": "Shepard",        "personal_address": "Calle Falsa 11",        "phone": 31219876323,        "working_start_date": "2019-01-01",        "role": "Specialist",        "salary": 6000000    }]
```
Endpoint 2: Create a new employee
## Description
Este endpoint permite al usuario crear un nuevo registro de  un empleado en el sistema.

Request
```
POST: api/User
```
Content-Type: application/json
```
{
    "name": "Calos",
    "last_name": "sabogal",
    "email": "carlos@mail.com",
    "personal_address": "calle falsa123",
    "phone": 3012302323,
    "working_start_date": "2022-01-01",
    "picture": "-png-or-jpg",
    "role": "Manager",
    "salary": 7500000
}
```
Response
```
201 Created
```
# Endpoint 3: Update an employee
## Description
Este endpoint permite a un usuario administrador actualizar un registro de empleado existente en el sistema.
```
Request
PUT: api/User{id}
```
Content-Type: application/json
```
{
    "name": "Calos",
    "last_name": "sabogal",
    "email": "carlos@mail.com",
    "personal_address": "calle falsa123",
    "phone": 3012302323,
    "working_start_date": "2022-01-01",
    "picture": "-png-or-jpg",
    "role": "Manager",
    "salary": 7500000
}
```
Response
```
200 OK
```
```
{
    "name": "marco Antonio",
    "last_name": "sabogal",
    "email": "carlos@mail.com",
    "personal_address": "calle falsa123",
    "phone": 3012302323,
    "working_start_date": "2022-01-01",
    "picture": "-png-or-jpg",
    "role": "Manager",
    "salary": 7500000
}

```
# Endpoint 4: Delete an employee
### Description
Este punto permite que un usuario administrador elimine un registro de empleado existente del sistema.

Request
```
DELETE : api/User{id}
```
Response
```
204 No Content
```

# Endpoint 5: Export all users
### Descripción:
Este endpoint extrae datos de todos los usuarios con un aumento de salario y genera un documento CSV con los aumentos de salario conrespondientes  ydevuelve una respuesta 200 OK si se completa correctamente.


Solicitud:
```
GET: api/User/export
```
```
[HttpGet]
[Route("export")]
public async Task<IActionResult> ExportAllUsers()
{
    await _userService.ExtractDataWithSalaryIncrement();

    return Ok();
}
```

Response 
```
200 OK
```
El documento generado se podra encontrar directamente en el escritorio del usuario con el nombre de 
```
users.csv
```

# Endpoint 6: Actualizar salario por id
### Descripción:
Este endpoint permite a un usuario generar la actualizacion de el salario de un usuario de un emepleado existente en el sistema utilizando su ID. 

1. Aumenta el salario atravez de la revisión salarial que debe realizarse cada 3 meses solamente. Si se ejecuta el proceso para un usuario antes de los 3 meses posteriores a la última revisión, no se debe tomar ninguna acción. Si se ejecuta el proceso para un usuario después de 3 meses desde la última revisión, se debe aumentar el salario para todos los períodos de 3 meses que faltan.Devuelve una respuesta 200 OK si se completa correctamente.

Solicitud:
```
PUT: api/User/{id}/salary
```
```
[HttpPut("{id}/salary")]
public async Task<ActionResult<bool>> UpdateSalaryByid(int id)
{

    return await _userService.ValidateSalaryAndDate(id);
}
```
Response:
```
200 OK
```
# Endpoint 7: Obtener detalles de un usuario por id
### Descripción:
Este endpoint permite a un usuario obtener los detalles de un usuario existente en el sistema utilizando su ID. Devuelve una respuesta 200 OK si se completa correctamente.

Solicitud:
```
GET:api/User/{id}
```
```
[HttpGet("{id}")]
public async Task<IActionResult> GetDetails(int id)
{

    return Ok(await _userService.GetDetails(id));
}
```
Respuesta:
```
200 OK
```
