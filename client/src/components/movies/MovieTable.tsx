import { useCallback, useEffect, useState } from "react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";
import { Button, Container, Loader, Message } from "semantic-ui-react";
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
    <Container className="container-style">
      {isLoading && <Loader active inline="centered" size="large" />}
      {!isLoading && error && <Message negative>{error}</Message>}

      {!isLoading && !error && (
        <table className="ui inverted table">
          <thead style={{ textAlign: "center" }}>
            <tr>
              <th>Id</th>
              <th>Title</th>
              <th>Description</th>
              <th>CreatedDate</th>
              <th>Category</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {movies.length === 0 ? (
              <tr>
                <td colSpan={6} style={{ textAlign: "center" }}>
                  No movies yet.
                </td>
              </tr>
            ) : (
              movies.map((movie) => (
                <MovieTableItem key={movie.id} movie={movie} onDeleted={handleDeleted} />
              ))
            )}
          </tbody>
        </table>
      )}

      <Button as={NavLink} to="createMovie" floated="right" type="button" content="Create Movie" positive />
    </Container>
  );
}
