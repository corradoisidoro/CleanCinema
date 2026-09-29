import { useEffect, useState, type ChangeEvent } from "react";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { Button, Form, Icon } from "semantic-ui-react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";

export default function MovieForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = Boolean(id);

  const [movie, setMovie] = useState<MovieDto>({
    id: undefined,
    title: "",
    description: "",
    createdDate: undefined,
    category: "",
  });
  const [isSaving, setIsSaving] = useState(false);
  const [loadedId, setLoadedId] = useState<number | undefined>(undefined);
  const [saveError, setSaveError] = useState<string | null>(null);
  const [loadError, setLoadError] = useState<{
    id: string;
    message: string;
  } | null>(null);

  // Both of these are derived rather than assigned inside the effect body:
  // calling setState synchronously in an effect causes a cascading render.
  // Scoping the load error to its id means it clears itself when the route
  // changes, with no reset call needed.
  const isLoading = isEditing && loadedId !== Number(id);
  const error =
    loadError !== null && loadError.id === id ? loadError.message : saveError;

  useEffect(() => {
    if (!id) {
      return;
    }

    let cancelled = false;

    apiConnector
      .getMovieById(id)
      .then((loaded) => {
        if (cancelled) {
          return;
        }
        setMovie(loaded!);
        setLoadedId(Number(id));
      })
      .catch(() => {
        if (!cancelled) {
          setLoadError({ id, message: `Could not load movie ${id}.` });
        }
      });

    return () => {
      cancelled = true;
    };
  }, [id]);

  async function handleSubmit() {
    setIsSaving(true);
    setSaveError(null);
    try {
      if (!movie.id) {
        await apiConnector.createMovie(movie);
      } else {
        await apiConnector.editMovie(movie);
      }
      navigate("/");
    } catch {
      setSaveError("Could not save the movie. Check the fields and try again.");
      setIsSaving(false);
    }
  }

  function handleInputChange(
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = event.target;
    setMovie({ ...movie, [name]: value });
  }

  return (
    <div className="container-style">
      <div className="cc-page-head">
        <div>
          <h1 className="cc-page-title">
            {isEditing ? "Edit movie" : "New movie"}
          </h1>
          <p className="cc-page-subtitle">
            {isEditing
              ? "Update the details below."
              : "Add a title to the catalogue."}
          </p>
        </div>
      </div>

      <div className="cc-form-card">
        <Form onSubmit={handleSubmit} autoComplete="off" loading={isSaving}>
          {error && (
            <div className="ui message cc-alert cc-alert-error">{error}</div>
          )}

          <Form.Field className="cc-field" disabled={isSaving || isLoading}>
            <label className="cc-label" htmlFor="title">
              Title
            </label>
            <input
              id="title"
              name="title"
              className="cc-input"
              placeholder="e.g. Blade Runner"
              value={movie.title}
              onChange={handleInputChange}
              disabled={isSaving || isLoading}
            />
          </Form.Field>

          <Form.Field className="cc-field" disabled={isSaving || isLoading}>
            <label className="cc-label" htmlFor="description">
              Description
            </label>
            <textarea
              id="description"
              name="description"
              className="cc-input"
              placeholder="A short synopsis."
              value={movie.description}
              onChange={handleInputChange}
              disabled={isSaving || isLoading}
            />
          </Form.Field>

          <Form.Field className="cc-field" disabled={isSaving || isLoading}>
            <label className="cc-label" htmlFor="category">
              Category
            </label>
            <input
              id="category"
              name="category"
              className="cc-input"
              placeholder="e.g. Sci-Fi"
              value={movie.category}
              onChange={handleInputChange}
              disabled={isSaving || isLoading}
            />
          </Form.Field>

          <div className="cc-form-actions">
            <Button
              as={NavLink}
              to="/"
              type="button"
              className="cc-btn"
              disabled={isSaving}
            >
              Cancel
            </Button>
            <Button
              type="submit"
              className="cc-btn cc-btn-primary"
              loading={isSaving}
              disabled={isSaving}
            >
              <Icon name="check" />
              {isEditing ? "Save changes" : "Create movie"}
            </Button>
          </div>
        </Form>
      </div>
    </div>
  );
}
