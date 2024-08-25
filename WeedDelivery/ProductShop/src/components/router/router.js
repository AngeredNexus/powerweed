import { createRouter, createWebHistory } from 'vue-router';
import guestComponent from "../auth/guest.vue";
import awaitAuthComponent from "../auth/authAwait.vue";
import orderPageComponent from "../order/order.vue";

const routes = [
  { path: '/', name: 'app', component: () => import("../../ClientApp.vue") },
  { path: '/login', name: 'login', component: guestComponent },
  { path: '/wait', name: 'loginWait', component: awaitAuthComponent },
  { path: '/store', name: 'store', component: () => import("../shop/shopView.vue") },
  { path: '/order', name: 'order', component: orderPageComponent }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;