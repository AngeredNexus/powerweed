import { createRouter, createWebHistory } from 'vue-router'
import StoreView from '../customer/view/components/store-view.vue'

const router = createRouter({
  history: createWebHistory("http://localhost:55525"),
  routes: [
    {
      path: '/',
      name: 'home',
      component: StoreView
    },
    {
      path: '/order',
      name: 'order',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../customer/order/components/customer-order-view.vue')
    }
  ]
})

export default router
