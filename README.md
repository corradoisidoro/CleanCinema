# 🎬 .NET 10 + React Full‑Stack Movies App  

### A modern CRUD (Create, Read, Update, Delete) application for managing movies, built with .NET 10 Minimal API and React.

A modern **CRUD application** for managing movies, built with **.NET 10 Minimal API** and **React (Vite)**.  
This project demonstrates clean architecture, robust data management, and a responsive frontend powered by modern web technologies.

---

## ✨ Features
- **CRUD Operations** → Create, Read, Update, Delete movies  
- **API Documentation** → Interactive Swagger/OpenAPI explorer
- **Frontend:** Dynamic user interface for viewing, adding, editing, and deleting movie records using React.
- **Backend:** A RESTful API built with .NET 10 Minimal API to handle all business logic and data operations. 
- **Data Persistence:** Lightweight database solution with options for both **SQLite** (for persistent data) and **In-Memory** (for ephemeral data during development/testing) storage.
- **Scalable Architecture** → Separation of concerns across Domain, Application, Infrastructure, and API layers 

---

## 🚀 Technologies Used
- **Frontend:** React (Vite), TypeScript, Semantic UI, Axios
- **Backend:** .NET 10 SDK, ASP.NET Core Minimal API, MediatR, Mapster, FluentValidation, Swagger, OpenAPI
- **Database:** SQLite, Entity Framework Core

---

## ⚙️ Setup Instructions
These instructions will guide you through setting up and running the application locally.

### Prerequisites
You will need the following software installed on your machine:
- [.NET SDK 10.0](https://dotnet.microsoft.com)
- [Node.js 20+](https://nodejs.org) and [npm](https://www.npmjs.com) (Node Package Manager)
- [Git](https://git-scm.com) for cloning the repository

---

### 1. Clone the Repository
First, clone the repository to your local machine using Git and navigate into the project directory:

```bash
git clone https://github.com/your-username/movies-app.git
cd movies-app
```

---

### 2. Set Up the Backend
Navigate to the backend directory to set up the API:

```bash
dotnet restore
cd src
dotnet ef database update -s .\Movies.Api\ -p .\Movies.Infrastructure\
dotnet run --project ./Movies.Api
```

---

### 3. Set Up the Frontend
Navigate to the frontend directory and install dependencies:

```bash
cd client
npm install
npm run dev
```

---

### 4. Access the Application
Once both backend and frontend are running:
- **Backend API** → `http://localhost:5000`  
- **Swagger UI** → `http://localhost:5000/swagger`  
- **Scalar UI** → `http://localhost:5000/scalar`  
- **Frontend React App** → `http://localhost:5173`  

---

## 🔮 Next Steps / Customization
- **Authentication** → Add JWT‑based authentication & authorization  
- **Rate Limiting** → Prevent abuse and control API usage  
- **Pagination & Filtering** → Efficient movie listings for large datasets  
- **Dockerization** → Containerize backend & frontend for easy deployment
- **Testing** → Backend (xUnit tests for Application & Domain logic) and Frontend(React Testing Library + Jest)

---

