import { useEffect, useState, type ChangeEvent } from "react";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { Button, Form, Message, Segment } from "semantic-ui-react";
import type { MovieDto } from "../../models/movieDto";
import apiConnector from "../../api/apiConnector";

export default function MovieForm() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [movie, setMovie] = useState<MovieDto>({
    id: undefined,
    title: "",
    description: "",
    createdDate: undefined,
    category: "",
  });
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      apiConnector
        .getMovieById(id)
        .then((movie) => setMovie(movie!))
        .catch(() => setError(`Could not load movie ${id}.`));
    }
  }, [id]);

  async function handleSubmit() {
    setIsSaving(true);
    setError(null);
    try {
      if (!movie.id) {
        await apiConnector.createMovie(movie);
      } else {
        await apiConnector.editMovie(movie);
      }
      navigate("/");
    } catch {
      setError("Could not save the movie. Check the fields and try again.");
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
    <Segment clearing inverted>
      <Form
        onSubmit={handleSubmit}
        autoComplete="off"
        className="ui inverted form"
        loading={isSaving}
      >
        <Form.Input
          placeholder="Title"
          name="title"
          value={movie.title}
          onChange={handleInputChange}
          disabled={isSaving}
        ></Form.Input>

        <Form.TextArea
          placeholder="Description"
          name="description"
          value={movie.description}
          onChange={handleInputChange}
          disabled={isSaving}
        ></Form.TextArea>

        <Form.Input
          placeholder="Category"
          name="category"
          value={movie.category}
          onChange={handleInputChange}
          disabled={isSaving}
        ></Form.Input>

        {error && <Message negative>{error}</Message>}

        <Button
          floated="right"
          positive
          type="submit"
          content="Submit"
          loading={isSaving}
          disabled={isSaving}
        />
        <Button
          as={NavLink}
          to="/"
          floated="right"
          positive
          type="button"
          content="Cancel"
          disabled={isSaving}
        />
      </Form>
    </Segment>
  );
}
