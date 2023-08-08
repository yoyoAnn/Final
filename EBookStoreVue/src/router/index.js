import { createRouter, createWebHistory } from "vue-router";
import Users from '../views/Users.vue'

//路由設定
const routes = [
    {
        path: '/users',
        component: Users
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router