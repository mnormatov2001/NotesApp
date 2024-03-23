# NoteXpress - web application for note-taking (Backend)

Welcome to the repository of my project, which is a full-fledged web application for note-taking. My application consists of three main components: a web API, an authentication server, and a client-side frontend web application.

## Web API

The Web API is developed using `ASP.NET Core` and provides HTTP methods for working with notes. The entire API functionality is documented using `Swagger`, making it easy and convenient for developers to use. `PostgreSQL` is used for interacting with the database, and `EntityFramework Core` is used for ORM. The business logic is implemented using the `MediatR` library in the form of commands and queries, and `Fluent Validation` is used for data validation. `Automapper` is used for object mapping.

## Authentication Server

The authentication server is built using `ASP.NET Core MVC` and the `IdentityServer4` library. It implements all the necessary functionality, including registration, login, logout, email confirmation, password reset, etc. Authentication and authorization are built using the `OpenID Connect` and `OAuth2.0` protocols, ensuring reliable interaction with the application and protecting data from unauthorized access. `PostgreSQL` and `EntityFramework Core` are used for storing user, client, session, and other data.

## CI/CD

To facilitate testing and deployment, a continuous integration and delivery `(CI/CD)` process is set up. I use `Dockerfile` for building and creating runnable images, and `Docker Compose` for managing containers. `GitHub Workflow` automates the process of building runnable images and publishing them to `Dockerhub`.

## Frontend

The frontend part of the project is located in a separate repository. You can familiarize yourself with it by following the [link](https://github.com/mnormatov2001/notexpress).

## How to test?

A fully functional version of the product is deployed on the internet and available [here](https://www.notexpress.ru).  
Interactive Swagger page for the Web API is available [here](https://api.notexpress.ru).  
Interactive Swagger page for the authentication server is available [here](https://auth.notexpress.ru).  
