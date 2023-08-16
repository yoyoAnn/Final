import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router/index.js'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import 'bootstrap/dist/css/bootstrap.css'; // 引入 Bootstrap 的 CSS
import 'mdb-vue-ui-kit/css/mdb.min.css';

createApp(App).use(vuetify).use(router).use(ElementPlus).mount('#app')
