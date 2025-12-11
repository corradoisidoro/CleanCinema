import axios, { Axios, type AxiosResponse } from "axios";

let isInterceptorSetup = false;

export const setupErrorHandlingInterceptor = () => {
  if (!isInterceptorSetup) {
    axios.interceptors.response.use(
      (response: AxiosResponse) => response,
      (error) => {
        if (error.response) {
          const statusCode = error.response.statusCode;
          const data = error.response.data;

          switch (statusCode) {
            case 400:
              if (data.errors) {
                const modelStateErrors = [];

                for (const item of data.errors) {
                  const property = item.property;
                  const errorMessage = item.errorMessage;

                  if (property && errorMessage) {
                    modelStateErrors.push({ property, errorMessage });
                  }
                }

                console.log(modelStateErrors);
              }

              break;

            case 401:
              console.log("Unauthorized");
              break;
            case 403:
              console.log("Forbidden");
              break;
            case 404:
              console.log("Not found");
              break;
            default:
              console.log("Generic Error");
              break;
          }
        }

        return Promise.reject(error);
      }
    );

    isInterceptorSetup = true;
  }
};
