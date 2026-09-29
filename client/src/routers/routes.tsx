import { createBrowserRouter, type RouteObject } from "react-router-dom";
import App from "../App.tsx";
import MovieForm from "../components/movies/MovieForm.tsx";
import MovieTable from "../components/movies/MovieTable.tsx";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      // `index: true` matches the parent's own path. A splat child ("*")
      // does not: at "/" the remaining segment is empty, so the splat
      // never matches and the outlet renders nothing.
      { index: true, element: <MovieTable /> },
      { path: "createMovie", element: <MovieForm key="create" /> },
      { path: "editMovie/:id", element: <MovieForm key="edit" /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
