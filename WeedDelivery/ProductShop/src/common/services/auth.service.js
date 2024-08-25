import api from "./api";
import TokenService from "./token.service";

class AuthService {
  login(code) {
    return api
      .get("/athy/await", { params: { code } })
      .then((response) => {
        if (response.data) {
          TokenService.setUser(response.data);
        }
        return response.data;
      });
  }

  logout() {
    TokenService.removeUser();
  }

  register({ username, email, password }) {
    return api.post("/auth/signup", {
      username,
      email,
      password
    });
  }
}

export default new AuthService();
