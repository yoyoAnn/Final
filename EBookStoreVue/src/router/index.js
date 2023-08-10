import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'
import Article from '../views/Article.vue'

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
    },
    {
        path: '/article',
        component: Article

    }
]



const router = createRouter({
    history: createWebHistory(),
    routes: routes
})

export default router