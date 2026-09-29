import { useState } from "react";
import { Button, Icon, Modal } from "semantic-ui-react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";
import { NavLink } from "react-router-dom";
import { formatDate } from "../../utils/formatDate";

interface Props {
  movie: MovieDto;
  onDeleted: (id: number) => void;
}

export default function MovieTableItem({ movie, onDeleted }: Props) {
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleDelete() {
    setIsDeleting(true);
    setError(null);
    try {
      await apiConnector.deleteMovie(movie.id!);
      setIsConfirmOpen(false);
      onDeleted(movie.id!);
    } catch {
      setError("Failed to delete");
      setIsDeleting(false);
    }
  }

  return (
    <tr>
      <td className="cc-cell-id">{movie.id}</td>
      <td className="cc-cell-title">{movie.title}</td>
      <td className="cc-cell-description">{movie.description}</td>
      <td className="cc-cell-date">{formatDate(movie.createdDate)}</td>
      <td>
        <span className="cc-pill">{movie.category}</span>
      </td>
      <td className="cc-cell-actions">
        <span className="cc-actions">
          <Button
            as={NavLink}
            to={`editMovie/${movie.id}`}
            type="button"
            className="cc-btn cc-btn-icon"
            title="Edit movie"
            aria-label={`Edit ${movie.title}`}
          >
            <Icon name="pencil" />
          </Button>
          <Button
            type="button"
            className="cc-btn cc-btn-icon"
            title="Delete movie"
            aria-label={`Delete ${movie.title}`}
            onClick={() => setIsConfirmOpen(true)}
          >
            <Icon name="trash alternate outline" />
          </Button>
        </span>
        {error && <span className="cc-row-error">{error}</span>}
      </td>

      <Modal
        open={isConfirmOpen}
        onClose={() => !isDeleting && setIsConfirmOpen(false)}
        onCancel={() => !isDeleting && setIsConfirmOpen(false)}
        size="tiny"
        className="cc-modal"
      >
        <Modal.Content>
          <div className="cc-modal-title">Delete this movie?</div>
          <div className="cc-modal-body">
            <strong>{movie.title}</strong> will be permanently removed. This cannot
            be undone.
          </div>
        </Modal.Content>
        <Modal.Actions>
          <Button
            type="button"
            className="cc-btn"
            onClick={() => setIsConfirmOpen(false)}
            disabled={isDeleting}
          >
            Cancel
          </Button>
          <Button
            type="button"
            className="cc-btn cc-btn-danger"
            onClick={handleDelete}
            loading={isDeleting}
            disabled={isDeleting}
          >
            Delete
          </Button>
        </Modal.Actions>
      </Modal>
    </tr>
  );
}
