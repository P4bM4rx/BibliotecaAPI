# 📚 BibliotecaAPI

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)

**BibliotecaAPI** es una API RESTful desarrollada en **C# y ASP.NET Core** diseñada para gestionar las operaciones fundamentales de una biblioteca, incluyendo el catálogo de libros, gestión de autores/usuarios y el registro de préstamos.

---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C#
* **Framework Backend:** ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Base de Datos:** SQL Server / PostgreSQL / SQLite / PHPMyAdmin
* **Documentación API:** Swagger / OpenAPI UI

---

## 🚀 Características y Funcionalidades

- **Gestión de Libros:** Operaciones CRUD (Crear, Leer, Actualizar y Eliminar) para el catálogo de obras.
- **Gestión de Autores y Usuarios:** Registro y consulta de perfiles.
- **Control de Préstamos:** Administración del estado de los libros y fechas de devolución.
- **Documentación Interactiva:** Interfaz de Swagger para probar los endpoints sin cliente externo.

---

## 📌 Endpoints Principales

| Módulo | Método | Endpoint | Descripción |
| :--- | :--- | :--- | :--- |
| **Libros** | `GET` | `/api/libros` | Consulta la lista completa de libros |
| **Libros** | `GET` | `/api/libros/{id}` | Obtiene los detalles de un libro específico |
| **Libros** | `POST` | `/api/libros` | Registra un nuevo libro |
| **Libros** | `PUT` | `/api/libros/{id}` | Actualiza la información de un libro |
| **Libros** | `DELETE` | `/api/libros/{id}` | Elimina un libro del catálogo |
| **Préstamos** | `POST` | `/api/prestamos` | Registra un nuevo préstamo de libro |

---

## ⚙️ Instalación y Configuración Local

### Requisitos Previos
* [.NET SDK](https://dotnet.microsoft.com/download) (versión 6.0, 7.0 u 8.0 según corresponda).
* Motor de Base de Datos configurado (SQL Server, PostgreSQL, etc.).

### Pasos para Ejecutar

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/P4bM4rx/BibliotecaAPI.git](https://github.com/P4bM4rx/BibliotecaAPI.git)
   cd BibliotecaAPI/BibliotecaAPI

2. **Para la conexión con la BDD**
   Abre el archivo appsettings.json y edita la propiedad ConnectionStrings con los datos de tu base de datos local:
   ```JSON
   "ConnectionStrings": { "DefaultConnection": "Server=LOCALHOST;Database=BibliotecaDb;Trusted_Connection=True;TrustServerCertificate=True;" }
