import { Outlet, useLocation } from "react-router-dom";
import "./App.css";
import MovieTable from "./components/movies/MovieTable";
import { Container } from "semantic-ui-react";
import { useEffect } from "react";
import { setupErrorHandlingInterceptor } from "./interceptors/axiosInterceptor";

function App() {
  const location = useLocation();

  useEffect(() => {
    setupErrorHandlingInterceptor();
  }, []);

  return (
    <>
      {location.pathname === "/" ? (
        <MovieTable />
      ) : (
        <Container className="container-style">
          <Outlet />
        </Container>
      )}
    </>
  );
}

export default App;
