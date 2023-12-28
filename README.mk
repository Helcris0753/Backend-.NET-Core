# Oferta Académica

Este es un proyecto enfocado en la automatización de horarios para una universidad. El objetivo es generar automáticamente los horarios a partir de las preferencias de los profesores. Se han definido tres tipos de horas:

- **Verdes:** Se establece la hora según la preferencia del profesor y evita choques.
- **Amarillos:** Se establece la hora sin considerar la preferencia del profesor, evitando choques.
- **Rojas:** No hay espacio para colocar la hora y se debe colocar en un horario que choque.

Para su funcionamiento, se requieren las siguientes dependencias:

- Dapper.
- Jsonconvert.

El proyecto utiliza microservicios. El esquema de la base de datos para este proyecto en .NET Core se encuentra en este mismo repositorio.


