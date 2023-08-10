import { createRouter, createWebHistory } from "vue-router";
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'
import Article from '../views/Article.vue'

//路由設定
const routes = [
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