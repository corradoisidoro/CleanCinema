import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
// Order matters: Semantic first, then the project design layer, so that
// App.css wins on specificity ties instead of being overridden by it.
import "semantic-ui-css/semantic.min.css";
import "./App.css";
import { RouterProvider } from "react-router-dom";
import { router } from "./routers/routes.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
