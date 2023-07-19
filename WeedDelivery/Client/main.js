import { createApp } from 'vue'
import App from './ClientApp.vue'
import router from './router/router'


import './assets/main.css'

const app = createApp(App)

app.use(router)

app.mount('#app')
