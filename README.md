# 🎬 CleanCinema – Full-Stack Clean Architecture Reference

[![CI](https://github.com/corradoisidoro/CleanCinema/actions/workflows/ci.yml/badge.svg)](https://github.com/corradoisidoro/CleanCinema/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10-blue)](https://dotnet.microsoft.com)
[![Node](https://img.shields.io/badge/Node-20+-green)](https://nodejs.org)

A small Clean Architecture sample: **ASP.NET Core Minimal API (.NET 10)** + **React (Vite +
TypeScript)**. It exists to be read — the whole application is ~1,500 lines, so you can follow
every layer end to end without wading through boilerplate.

> [!IMPORTANT]
> **MediatR 14 is commercially licensed.**

---

## Project Structure

```plaintext
/CleanCinema/
├─ client/                     # React + Vite
│  ├─ src/
│  │  ├─ api/                  # Axios connector
│  │  ├─ components/movies/    # MovieTable, MovieTableItem, MovieForm
│  │  ├─ interceptors/         # Axios error interceptor
│  │  ├─ models/               # TypeScript DTOs
│  │  ├─ routers/              # React Router config
│  │  ├─ utils/                # Helpers
│  │  ├─ App.css               # Design tokens and component styles
│  │  └─ main.tsx
│  ├─ config.tsx               # API base URL
│  └─ package.json
├─ src/
│  ├─ Movies.Api/              # Minimal API endpoints
│  ├─ Movies.Application/      # CQRS handlers, validation, mappings
│  ├─ Movies.Contracts/        # DTOs and responses
│  ├─ Movies.Domain/           # Entities
│  ├─ Movies.Infrastructure/   # EF Core context, migrations
│  └─ Movies.Tests/            # xUnit tests
├─ global.json                 # Pinned .NET SDK version
└─ Movies.sln
```

### Dependency direction

Dependencies point inward only, with one deliberate exception: `Movies.Application` takes a
direct project reference to `Movies.Infrastructure`, so its handlers inject `MoviesDbContext`
rather than talking through repository interfaces. That keeps the sample small enough to read
end to end, but it means the Application layer is not persistence-agnostic. A production version
would declare `IMovieRepository` in Application and implement it in Infrastructure.
`Domain` and `Contracts` reference nothing and know nothing about HTTP or EF Core.

```plaintext
Movies.Api ──────────► Movies.Application ──────────► Movies.Domain
      │                        │      ├──────────────► Movies.Contracts
      │                        │      └──────────────► Movies.Infrastructure ──► Movies.Domain
      └──► Movies.Infrastructure

Movies.Tests ──► Movies.Application, Movies.Infrastructure
```

---

## Stack

| Layer | Technology |
|---|---|
| Frontend | React 18, TypeScript, Vite 7, React Router 7, Axios, Semantic UI |
| Backend | .NET 10, ASP.NET Core Minimal API, MediatR, Mapster, FluentValidation, Scalar |
| Data | SQLite, Entity Framework Core 10 |
| Tests | xUnit (plain `Assert`, no third-party assertion library) |

Notable behaviours: `201 Created` with a `Location` header on create, `204 No Content` on
update and delete, `404` for unknown ids, and `400` with per-field errors from a
FluentValidation pipeline behaviour. Errors are returned as RFC 7807 `ProblemDetails` through
a central `IExceptionHandler`.

---

## Setup

### Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com)
- [Node.js 20+](https://nodejs.org)
- [Git](https://git-scm.com)

### 1. Clone

```bash
git clone https://github.com/corradoisidoro/CleanCinema.git
cd CleanCinema
```

### 2. Backend

From the repository root:

```bash
dotnet restore
dotnet run --project ./src/Movies.Api
```

`movies.db` is created in the API project directory and migrated on startup, so there is no
separate database step. The EF Core CLI is only needed if you want to add a migration of your
own: `dotnet tool install --global dotnet-ef`.

### 3. Frontend

In a second terminal:

```bash
cd client
npm install
npm run dev
```

### 4. Open

| | |
|---|---|
| Frontend | http://localhost:5173 |
| API | http://localhost:5000/api/movies |
| API reference (Scalar) | http://localhost:5000/scalar |
| API reference (Swagger UI) | http://localhost:5000/swagger |

Both API explorers are only mapped in `Development`, which the default launch profile sets.

### Verify

```bash
dotnet build                    # 0 warnings, 0 errors
dotnet test                     # 24 tests
cd client && npm run lint
cd client && npm run build
```

CI runs all four on every push, with `-warnaserror` on the .NET build, so the "0 warnings" claim
is enforced rather than assumed.

The tests run against a real SQLite database created per test, so handlers exercise actual
persistence rather than a mocked data layer.

---

## License

Released under the [MIT License](LICENSE), which covers this repository's own source only.
