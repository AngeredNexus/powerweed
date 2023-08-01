import { createRouter, createWebHistory } from 'vue-router'
import StoreView from '../customer/view/components/store-view.vue'
import CustomerOrderView from '../customer/order/components/customer-order-view.vue'

const router = createRouter({
  history: createWebHistory("/"),
  routes: [
    {
      path: '/',
      name: 'home',
      component: StoreView
    },
    {
      path: '/order',
      name: 'order',
      component: CustomerOrderView
    }
  ]
})

export default router
