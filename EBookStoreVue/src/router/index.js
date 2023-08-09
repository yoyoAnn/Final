import { createRouter, createWebHistory } from "vue-router";
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'

//路由設定
const routes = [
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