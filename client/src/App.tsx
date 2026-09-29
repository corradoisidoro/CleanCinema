import { Outlet } from "react-router-dom";
import "./App.css";
import { useEffect } from "react";
import { setupErrorHandlingInterceptor } from "./interceptors/axiosInterceptor";

function App() {
  useEffect(() => {
    setupErrorHandlingInterceptor();
  }, []);

  // Each route owns its own page container, so the outlet is not wrapped.
  return <Outlet />;
}

export default App;
