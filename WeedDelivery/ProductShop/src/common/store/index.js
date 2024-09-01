import { createStore } from "vuex";
import { auth } from "./auth.module";
import { cart } from "./cart.module";
import { products } from "./products.module";

const store = createStore({
  modules: {
    auth,
    cart,
    products
  },
});

export default store;
