import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router/index.js'
import ElementPlus from 'element-plus'
import BackTop from "./components/Bookbacktop.vue"
import axios from 'axios';
import 'element-plus/dist/index.css'
import 'bootstrap/dist/css/bootstrap.css'; // 引入 Bootstrap 的 CSS
import 'mdb-vue-ui-kit/css/mdb.min.css';
import 'vue3-toastify/dist/index.mjs'
import vue3GoogleLogin from 'vue3-google-login'

const CLIENT_ID = '603051350426-oc0qusrj0rpqnan4ellc86uq2eubnook.apps.googleusercontent.com';
createApp(App).use(vuetify).use(router).use(ElementPlus).use(BackTop).use(vue3GoogleLogin, { clientId: CLIENT_ID }).mount('#app')

