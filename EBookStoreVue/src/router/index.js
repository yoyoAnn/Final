import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue'
import Users from '../views/Users.vue'
import Cart from '../views/Cart.vue'
import Article from '../views/Article.vue'
import Login from '../views/Login.vue'
import Register from '../views/Register.vue'
import ConformRegister from '../views/ConformRegister.vue'

//路由設定
const routes = [
    {
        path: '/',
        component: Home
    },
    {
        path: '/login',
        component: Login
    },
    {
        path: '/register',
        component: Register
    },
    {
        path: '/conformregister',
        component: ConformRegister
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