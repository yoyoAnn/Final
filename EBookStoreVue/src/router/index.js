import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'

//路由設定
const routes = [
    {
        path: '/',
        component: Home
    },
    {
        path: '/users',
        component: Users
    },
    {
        path: '/cart',
        component: Cart
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router