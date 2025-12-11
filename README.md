# 🎬 Full-Stack Movies Application (.NET 10 & React + Vite)

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)]()
[![.NET](https://img.shields.io/badge/.NET-10-blue)]()
[![Node](https://img.shields.io/badge/Node-20+-green)]()

A modern **CRUD application** for managing movies, built with **ASP.NET Core Minimal API (.NET 10)** and **React (Vite + TypeScript)**.  
This project demonstrates clean architecture, robust data management, and a responsive frontend powered by modern web technologies.

---

## 📂 Project Structure
This project is organized as a monorepo with separate directories for the client-side (React) and server-side (.NET) applications.

```plaintext
/movies-app/
├─ client/                  # React + Vite frontend
│  ├─ src/
│  │  ├─ api/               # API connector (Axios)
│  │  ├─ components/        # UI components (MovieForm, MovieTable, etc.)
│  │  ├─ interceptors/      # Axios interceptors
│  │  ├─ models/            # TypeScript DTOs and response models
│  │  ├─ routers/           # React Router configuration
│  │  ├─ App.tsx            # Root component
│  │  └─ main.tsx           # Entry point
│  ├─ vite.config.ts        # Vite configuration
│  └─ package.json
├─ src/                     # .NET backend
│  ├─ Movies.Api/           # Entry point for HTTP endpoints
│  ├─ Movies.Application/   # Business logic, CQRS handlers
│  ├─ Movies.Contracts/     # DTOs and shared contracts
│  ├─ Movies.Domain/        # Core entities and domain rules
│  ├─ Movies.Infrastructure/# EF Core, persistence, external services
│  └─ Movies.Tests/         # Unit/integration tests (TODO)
├─ Movies.sln               # Solution file
└─ README.md
```

---

## ✨ Features
- **CRUD Operations** → Create, Read, Update, Delete movies  
- **API Documentation** → Interactive Swagger/OpenAPI explorer
- **Frontend:** Dynamic user interface for viewing, adding, editing, and deleting movie records using React.
- **Backend:** RESTful API built with ASP.NET Core Minimal API (.NET 10) to handle all business logic and data operations. 
- **Data Persistence:** Lightweight database solution **SQLite** (for persistent data).
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
