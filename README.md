# 🎬 CineStack – Full-Stack Clean Architecture Reference

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10-blue)](https://dotnet.microsoft.com)
[![Node](https://img.shields.io/badge/Node-20+-green)](https://nodejs.org)

A small but production-minded system built with **ASP.NET Core Minimal API (.NET 10)** and **React (Vite + TypeScript)**.
The focus is on clean architecture, separation of concerns, and maintainability rather than feature completeness.

It is intentionally small (≈900 LOC) so you can read the whole thing in one sitting and see every layer wired end to end — without wading through a 10,000-line boilerplate template.

> [!IMPORTANT]
> **MediatR is under a commercial license.**
> This project uses MediatR 14, which [Lucky Penny Software](https://luckypennysoftware.com)
> licenses commercially. It is free for individuals, open-source projects, non-profits, and
> organizations under $5M annual gross revenue — which covers learning, portfolio, and
> non-production use. **It is not free for commercial use outside that tier.**
>
> Running the app locally prints a license warning; this is expected and harmless for
> development. If you need to use this commercially, see
> [Downgrading to MediatR 12.x](#downgrading-to-mediatr-12x) below.

---

## 📂 Project Structure
This project is organized as a monorepo with separate directories for the client-side (React) and server-side (.NET) applications.

```plaintext
/cine-stack/
├─ client/                  # React + Vite frontend
│  ├─ src/
│  │  ├─ api/               # API connector (Axios)
│  │  ├─ components/        # UI components (MovieForm, MovieTable, etc.)
│  │  ├─ interceptors/      # Axios interceptors
│  │  ├─ models/            # TypeScript DTOs and response models
│  │  ├─ routers/           # React Router configuration
│  │  ├─ App.tsx            # Root component
│  │  └─ main.tsx           # Entry point
│  ├─ config.tsx            # API base URL
│  ├─ vite.config.ts        # Vite configuration
│  └─ package.json
├─ src/                     # .NET backend
│  ├─ Movies.Api/           # Entry point for HTTP endpoints
│  ├─ Movies.Application/   # Business logic, CQRS handlers
│  ├─ Movies.Contracts/     # DTOs and shared contracts
│  ├─ Movies.Domain/        # Core entities and domain rules
│  ├─ Movies.Infrastructure/# EF Core, persistence, migrations
│  └─ Movies.Tests/         # xUnit tests (validators + handlers)
├─ Movies.sln               # Solution file
└─ README.md
```

### Dependency direction
Dependencies point inward only. `Domain` references nothing, `Contracts` references nothing,
and neither knows that HTTP or EF Core exist. `Movies.Tests` is the only project that reaches
into `Infrastructure`, and only to build a throwaway database.

```plaintext
Movies.Api ──────────► Movies.Application ──────────► Movies.Domain
      │                        │      └──────────────► Movies.Contracts
      └──► Movies.Infrastructure ────────────────────► Movies.Domain

Movies.Tests ──► Movies.Application, Movies.Infrastructure
```

---

## ✨ Features
- **CRUD Operations** → Create, Read, Update, Delete movies  
- **API Documentation** → Interactive Scalar/OpenAPI reference explorer
- **CQRS** → MediatR commands and queries with a FluentValidation pipeline behavior
- **Error Handling** → Centralized `IExceptionHandler` returning RFC 7807 `ProblemDetails`
- **Correct Status Codes** → `201 Created` + `Location` on create, `204 No Content` on update/delete, `404` for unknown ids, `400` with per-field errors for validation failures
- **Tests** → 24 xUnit tests covering validators and command/query handlers against a real SQLite database
- **Layered Architecture** → Separation of concerns across Domain, Application, Infrastructure, and API layers  
- **Frontend:** Dynamic user interface for viewing, adding, editing, and deleting movie records using React.
- **Backend:** RESTful API built on ASP.NET Core Minimal API (.NET 10) to handle all business logic and data operations. 
- **Data Persistence:** Lightweight database solution **SQLite** (via EF Core migrations).

---

## 🚀 Technologies Used
- **Frontend:** React 19 (Vite), TypeScript, Semantic UI, Axios, React Router 7
- **Backend:** .NET 10 SDK, ASP.NET Core Minimal API, MediatR, Mapster, FluentValidation, Scalar/OpenAPI
- **Database:** SQLite, Entity Framework Core 10
- **Testing:** xUnit + `Microsoft.NET.Test.Sdk` (plain `Assert` — no third-party assertion library)

> MediatR is commercially licensed (see the note at the top). The other dependencies are
> permissively licensed and carry no such restriction.
---

## ⚙️ Setup Instructions
These instructions will guide you through setting up and running the application locally.

### Prerequisites
You will need the following software installed on your machine:
- [.NET SDK 10.0](https://dotnet.microsoft.com)
- [Node.js 20+](https://nodejs.org) and [npm](https://www.npmjs.com) (Node Package Manager)
- [Git](https://git-scm.com) for cloning the repository
- The EF Core CLI (used to apply migrations):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

### 1. Clone the Repository
First, clone the repository to your local machine using Git and navigate into the project directory:

```bash
git clone https://github.com/corradoisidoro/cine-stack.git
cd cine-stack
```

---

### 2. Set Up the Backend
From the repository root, restore and apply the migrations:

```bash
dotnet restore
dotnet ef database update -s .\src\Movies.Api\ -p .\src\Movies.Infrastructure\
```

Then run the API:

```bash
dotnet run --project ./src/Movies.Api
```

The API listens on `http://localhost:5000` (see `Properties/launchSettings.json`).
The SQLite database file (`movies.db`) is created in the API project directory.

---

### 3. Set Up the Frontend
In a second terminal, install dependencies and start the dev server:

```bash
cd client
npm install
npm run dev
```

---

### 4. Access the Application
Once both backend and frontend are running:
- **Backend API** → `http://localhost:5000`  
- **API Reference (Scalar UI)** → `http://localhost:5000/scalar`  
- **OpenAPI document** → `http://localhost:5000/openapi/v1.json`  
- **Frontend React App** → `http://localhost:5173`  

> Note: the API reference UI is only mapped in the `Development` environment, so run the
> backend with `ASPNETCORE_ENVIRONMENT=Development` (the default launch profile) to see it.

---

## 🧪 Verifying the Install
Build and test both halves — the build should complete with zero warnings and zero errors,
and all 24 tests should pass:

```bash
dotnet build
dotnet test
cd client && npm run build
```

The suite uses a real SQLite database created per test, so handler tests exercise real
persistence and query behaviour rather than a mocked data layer.

---

## 🔮 Next Steps / Customization
Known gaps, in rough priority order if you want to take this further:

- **CI** → A GitHub Actions workflow running `dotnet build`, `dotnet test`, and `npm run build` on every push
- **Frontend Tests** → React Testing Library; the backend has coverage, the frontend has none
- **Authentication** → Add JWT-based authentication & authorization  
- **Pagination & Filtering** → Efficient movie listings for large datasets  
- **Rate Limiting** → Prevent abuse and control API usage  
- **Dockerization** → Containerize backend & frontend for easy deployment
- **Concurrency** → `UpdateMovieCommand` does read-then-write with no optimistic concurrency token, so simultaneous edits silently overwrite each other

---

## 📄 License

Released under the [MIT License](LICENSE).

> ⚠️ The MIT license covers **this repository's own source code only**. It does not relicense
> the third-party packages it depends on. MediatR 14 in particular carries its own commercial
> license and is **not** covered by this repo's MIT grant. If you fork this for commercial use,
> you must comply with MediatR's terms separately — or downgrade to MediatR 12.x, which is
> Apache-2.0 (see below).

### Downgrading to MediatR 12.x

MediatR 12.4.1 is the last Apache-2.0 release and removes all licensing restrictions. The
downgrade is a **one-line change**, verified to build and run cleanly:

1. Set `<PackageReference Include="MediatR" Version="12.4.1" />` in both
   `Movies.Application.csproj` and `Movies.Api.csproj`.
2. In `src/Movies.Application/Behaviours/ValidationBehaviour.cs`, change:
   ```diff
   - var response = await next(cancellationToken);
   + var response = await next();
   ```

That second change is the only incompatibility: MediatR 13+ added a `CancellationToken`
parameter to `RequestHandlerDelegate<TResponse>`, and 12.x has a parameterless delegate.
Everything else — `IMediator`, `IRequestHandler`, `AddMediatR`, and `AddOpenBehavior` —
is identical across both versions, and the FluentValidation pipeline behaviour works the same.

Note that this reintroduces the pre-fix behaviour of dropping the cancellation token when
invoking the next handler in the pipeline. If you care about that, pass the token through a
different mechanism on 12.x.

