import axios from "axios";

const instance = axios.create({
  baseURL: "http://localhost:55525/api/1",
  headers: {
    "Content-Type": "application/json",
  },
});

export default instance;
