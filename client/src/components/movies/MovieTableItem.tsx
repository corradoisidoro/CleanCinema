import { useState } from "react";
import { Button } from "semantic-ui-react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";
import { NavLink } from "react-router-dom";

interface Props {
  movie: MovieDto;
  onDeleted: (id: number) => void;
}

export default function MovieTableItem({ movie, onDeleted }: Props) {
  const [isDeleting, setIsDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleDelete() {
    setIsDeleting(true);
    setError(null);
    try {
      await apiConnector.deleteMovie(movie.id!);
      onDeleted(movie.id!);
    } catch {
      setError("Failed to delete");
      setIsDeleting(false);
    }
  }

  return (
    <tr className="center aligned">
      <td data-label="Id">{movie.id}</td>
      <td data-label="Title">{movie.title}</td>
      <td data-label="Description">{movie.description}</td>
      <td data-label="CreatedDate">{movie.createdDate}</td>
      <td data-label="Category">{movie.category}</td>
      <td data-label="Action">
        <Button
          as={NavLink}
          to={`editMovie/${movie.id}`}
          color="yellow"
          type="submit"
        >
          Edit
        </Button>
        <Button
          color="red"
          type="submit"
          negative
          loading={isDeleting}
          disabled={isDeleting}
          onClick={handleDelete}
        >
          Delete
        </Button>
        {error && <div className="ui red message">{error}</div>}
      </td>
    </tr>
  );
}
