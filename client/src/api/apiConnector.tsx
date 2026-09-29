import type { AxiosResponse } from "axios";
import type { MovieDto } from "../models/movieDto";
import type { GetMoviesResponse } from "../models/getMoviesResponse";
import axios from "axios";
import { API_BASE_URL } from "../../config";
import type { GetMovieByIdResponse } from "../models/getMovieByIdResponse";

const apiConnector = {

  getMovies: async (): Promise<MovieDto[]> => {
      const response: AxiosResponse<GetMoviesResponse> = await axios.get(`${API_BASE_URL}/movies`);
      // Timestamps are passed through untouched. Truncating to "yyyy-MM-dd"
      // here would make `new Date()` in formatDate() read it back as UTC
      // midnight and render the previous day for every negative UTC offset.
      return response.data.movieDtos;
  },

  createMovie: async (movie: MovieDto): Promise<void> => {
      await axios.post<number>(`${API_BASE_URL}/movies`, movie);
  },

  editMovie: async (movie: MovieDto): Promise<void> => {
      await axios.put<number>(`${API_BASE_URL}/movies/${movie.id}`, movie);
  },

  deleteMovie: async (movieId: number): Promise<void> => {
      await axios.delete<number>(`${API_BASE_URL}/movies/${movieId}`);
  },

  getMovieById: async (movieId: string): Promise<MovieDto | undefined> => {
      const response = await axios.get<GetMovieByIdResponse>(`${API_BASE_URL}/movies/${movieId}`);
      return response.data.movieDto;
  },
};

export default apiConnector;
