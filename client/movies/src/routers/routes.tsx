import { createBrowserRouter, type RouteObject } from "react-router-dom";
import App from "../App.tsx";
import MovieForm from "../components/movies/MovieForm.tsx";
import MovieTable from "../components/movies/MovieTable.tsx";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "createMovie", element: <MovieForm key="create" /> },
      { path: "editMovie/:id", element: <MovieForm key="edit" /> },
      { path: "*", element: <MovieTable /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
