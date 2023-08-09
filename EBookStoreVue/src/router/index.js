import { createRouter, createWebHistory } from "vue-router";
import Users from '../views/Users.vue'
import CartItem from '../views/CartItem.vue'

//路由設定
const routes = [
    {
        path: '/users',
        component: Users
    },
    {
        path: '/CartItem',
        component: CartItem
    }
]



const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router