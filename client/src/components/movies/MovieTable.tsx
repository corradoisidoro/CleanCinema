import { useCallback, useEffect, useState } from "react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";
import { Button, Icon, Loader } from "semantic-ui-react";
import MovieTableItem from "./MovieTableItem";
import { NavLink } from "react-router-dom";

export default function MovieTable() {
  const [movies, setMovies] = useState<MovieDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadMovies = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      setMovies(await apiConnector.getMovies());
    } catch {
      setError("Could not load movies. Is the API running on http://localhost:5000?");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadMovies();
  }, [loadMovies]);

  function handleDeleted(id: number) {
    setMovies((current) => current.filter((movie) => movie.id !== id));
  }

  return (
    <div className="container-style">
      <div className="cc-page-head">
        <div>
          <h1 className="cc-page-title">Movies</h1>
          <p className="cc-page-subtitle">
            <span className="cc-count">
              {isLoading || error
                ? "Loading…"
                : `${movies.length} ${movies.length === 1 ? "title" : "titles"}`}
            </span>
          </p>
        </div>
        <Button
          as={NavLink}
          to="createMovie"
          type="button"
          className="cc-btn cc-btn-primary"
        >
          <Icon name="plus" />
          Create Movie
        </Button>
      </div>

      {isLoading && <Loader active inline="centered" size="large" />}

      {!isLoading && error && (
        <div className="ui message cc-alert cc-alert-error">{error}</div>
      )}

      {!isLoading && !error && (
        <div className="cc-card">
          <table className="cc-table">
            <thead>
              <tr>
                <th>Id</th>
                <th>Title</th>
                <th>Description</th>
                <th>Added</th>
                <th>Category</th>
                <th className="cc-col-actions">Actions</th>
              </tr>
            </thead>
            <tbody>
              {movies.length === 0 ? (
                <tr>
                  <td colSpan={6}>
                    <div className="cc-empty">
                      <strong>No movies yet</strong>
                      Create the first one to get started.
                    </div>
                  </td>
                </tr>
              ) : (
                movies.map((movie) => (
                  <MovieTableItem key={movie.id} movie={movie} onDeleted={handleDeleted} />
                ))
              )}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
