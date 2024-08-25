import { createApp } from 'vue'
import App from './ClientApp.vue'
import router from './components/router/router'
import store from './common/store'
import setupInterceptors from "./common/init/setupInterceptors";
import locale from './common/locale/index';

// import 'swiper/swiper-bundle.css';
import './styles.css'

setupInterceptors();

const app = createApp(App);

app.use(store);
app.use(router);
app.use(locale);
app.mount('#app');
