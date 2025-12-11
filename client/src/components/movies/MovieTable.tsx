import { useEffect, useState } from "react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";
import { Button, Container } from "semantic-ui-react";
import MovieTableItem from "./MovieTableItem";
import { NavLink } from "react-router-dom";

export default function MovieTable() {
  const [movies, setMovies] = useState<MovieDto[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const fetchMovies = await apiConnector.getMovies();
      setMovies(fetchMovies);
    };

    fetchData();
  }, []);

  return (
    <>
      <Container className="container-style">
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
            {movies.length !== 0 &&
              movies.map((movie, index) => (
                <MovieTableItem key={index} movie={movie} />
              ))}
          </tbody>
        </table>
        <Button as={NavLink} to="createMovie" floated="right" type="button" content="Create Movie" positive />
      </Container>
    </>
  );
}
