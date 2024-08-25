import axiosInstance from "../services/api.js";
import TokenService from "../services/token.service.js";

const setup = (store) => {
    
  axiosInstance.interceptors.request.use(
    (config) => {
      const token = TokenService.getLocalAccessToken();
      if (token) {
        config.headers["Authorization"] = 'Bearer ' + token;  // for Spring Boot back-end
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

    axiosInstance.interceptors.response.use(
        (res) => {
            return res;
        },
        async (err) => {
            const originalConfig = err.config;

            if(originalConfig.url === "/athy/check"){
                return Promise.reject(err);
            }
            
            if (err.response.status === 401 && !originalConfig._retry) {
                originalConfig._retry = true;

                try {
                    await this.$router.go({ name: 'login' }); // Consistent approach
                    return axiosInstance(originalConfig);
                } catch (_error) {
                    return Promise.reject(_error);
                }
            }

            return Promise.reject(err);
        }
    );
};

export default setup;