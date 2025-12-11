import type { Axios, AxiosResponse } from "axios";
import type { MovieDto } from "../models/movieDto";
import type { GetMoviesResponse } from "../models/getMoviesResponse";
import axios from "axios";
import { API_BASE_URL } from "../../config";
import type { GetMovieByIdResponse } from "../models/getMovieByIdResponse";

const apiConnector = {
  
  getMovies: async (): Promise<MovieDto[]> => {
      const response: AxiosResponse<GetMoviesResponse> = await axios.get(`${API_BASE_URL}/movies`);
      const movies = response.data.movieDtos.map((movie) => ({
        ...movie,
        createdDate: movie.createdDate?.slice(0, 10) ?? "",
      }));
      return movies;
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
